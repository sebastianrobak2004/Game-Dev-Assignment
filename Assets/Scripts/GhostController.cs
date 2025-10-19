using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GhostController : MonoBehaviour
{
    public float moveSpeed = 4.5f;
    private Vector2Int currentDir = Vector2Int.right;
    private Vector3 targetWorldPos;

    [SerializeField] private Vector3 spawn;

    private float timer;

    private enum behaviour{Red, Pink, Purple, Yellow};
    private enum GhostStates{Normal, Dead, Scared, Recovering};
    [SerializeField] private GhostStates state = GhostStates.Normal;


    [SerializeField] private Grid grid;
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private List<TileBase> walkableTiles;
    [SerializeField] private Transform pacman;
    [SerializeField] private Animator animator;


    private Queue<Vector3Int> pathQueue = new Queue<Vector3Int>();

    void Start()
    {
        targetWorldPos = transform.position;
    }

    void Update()
    {
        if(state == GhostStates.Scared)
        {
            animator.SetBool("Scared", true);
        }
        if(state == GhostStates.Dead)
        {
            animator.SetBool("Dead", true);
        }
        if(state == GhostStates.Normal)
        {
            animator.SetBool("Dead", false);
            animator.SetBool("Scared", false);
        }

        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0 & state == GhostStates.Scared)
        {
            state = GhostStates.Normal;
        }




        if(state == GhostStates.Normal)
        {
            Ghost1Behaviour();
        }
        if(state == GhostStates.Scared)
        {
            GhostScared();
        }
        if(state == GhostStates.Dead)
        {
            GhostDead();
        }

        if(grid.WorldToCell(gameObject.transform.position) == grid.WorldToCell(spawn))
        {
            state = GhostStates.Normal;
        }

        
    }

    void GhostDead()
    {
        moveSpeed = 2.5f;
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                
                FindPathTox(grid.WorldToCell(spawn)); 
                

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

    void GhostScared()
    {
        moveSpeed = 2.5f;

        // When close enough to target, choose a new direction
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            Vector3Int currentCell = grid.WorldToCell(transform.position);

            // Check all four directions
            Vector3Int[] dirs = {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right
            };

            Vector3Int bestCell = currentCell;
            float bestDistance = -1f;

            foreach (var dir in dirs)
            {
                Vector3Int next = currentCell + dir;
                if (IsWalkable(next))
                {
                    // Compute distance to Pac-Man
                    float distToPacman = Vector3.Distance(
                        grid.CellToWorld(next),
                        pacman.position
                    );

                    // Pick the tile that is farthest away
                    if (distToPacman > bestDistance)
                    {
                        bestDistance = distToPacman;
                        bestCell = next;
                    }
                }
            }

            // Move toward that "farthest" tile
            targetWorldPos = grid.CellToWorld(bestCell) + (Vector3)grid.cellSize / 2f;
    }

    // Continue movement
    transform.position = Vector3.MoveTowards(
        transform.position,
        targetWorldPos,
        moveSpeed * Time.deltaTime
    );
    }


    void Ghost1Behaviour(){
        moveSpeed = 4.5f;
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                
                FindPathTox(grid.WorldToCell(pacman.position)); 
                

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

    void OnEnable()
    {
        PelletTilemapSpawner.OnPowerPelletEaten += HandlePowerPelletEaten;
    }

    void OnDisable()
    {
        PelletTilemapSpawner.OnPowerPelletEaten -= HandlePowerPelletEaten;
    }

    private void HandlePowerPelletEaten()
    {
        timer = 5;
        state = GhostStates.Scared;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(state == GhostStates.Scared)
            {
                state = GhostStates.Dead;
            }
            if(state == GhostStates.Normal){
                //kill HIM
            }
        }
    }
    

    void FindPathTox(Vector3Int goal)
    {
        Vector3Int start = grid.WorldToCell(transform.position);
        

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
            return;

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
