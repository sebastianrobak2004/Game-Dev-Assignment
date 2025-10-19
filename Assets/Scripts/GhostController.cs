using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GhostController : MonoBehaviour
{
    public float moveSpeed = 4.5f;
    private Vector2Int currentDir = Vector2Int.right;
    private Vector3 targetWorldPos;

    [SerializeField] private Grid grid;
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private List<TileBase> walkableTiles;
    [SerializeField] private Transform pacman; // reference to Pac-Man

    private Queue<Vector3Int> pathQueue = new Queue<Vector3Int>();

    void Start()
    {
        targetWorldPos = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            if (pathQueue.Count == 0)
            {
                FindPathToPacman(); // find new path if empty
            }

            if (pathQueue.Count > 0)
            {
                Vector3Int nextCell = pathQueue.Dequeue();
                targetWorldPos = grid.CellToWorld(nextCell) + (Vector3)grid.cellSize / 2f;
            }
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWorldPos,
            moveSpeed * Time.deltaTime
        );
    }

    // ────────────────────────────────
    // BFS Shortest Path to Pac-Man
    // ────────────────────────────────
    void FindPathToPacman()
    {
        Vector3Int start = grid.WorldToCell(transform.position);
        Vector3Int goal = grid.WorldToCell(pacman.position);

        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        frontier.Enqueue(start);

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        cameFrom[start] = start;

        Vector3Int[] dirs =
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();
            if (current == goal) break;

            foreach (var dir in dirs)
            {
                Vector3Int next = current + dir;
                if (IsWalkable(next) && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
            return; // no path

        // Reconstruct path
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int step = goal;

        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }

        path.Reverse();
        pathQueue = new Queue<Vector3Int>(path);
    }

    // ────────────────────────────────
    // Tile helper functions
    // ────────────────────────────────
    bool IsWalkable(Vector3Int cellPos)
    {
        foreach (Tilemap tm in tilemaps)
        {
            if (tm == null) continue;
            TileBase tile = tm.GetTile(cellPos);
            if (tile != null && walkableTiles.Contains(tile))
                return true;
        }
        return false;
    }

    bool IsIntersection(Vector2Int cell)
    {
        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        int openPaths = 0;
        foreach (var dir in directions)
        {
            if (IsWalkable((Vector3Int)(cell + dir)))
                openPaths++;
        }

        return openPaths >= 3;
    }
}
