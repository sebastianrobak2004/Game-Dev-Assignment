using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PacmanMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2Int gridPos; 
    private Vector2Int currentDir = Vector2Int.zero;
    private Vector2Int nextDir = Vector2Int.zero;

    private Vector3 targetWorldPos;
    [SerializeField] private AudioSource audioSource;


    [SerializeField] private Animator animator;
    [SerializeField] private Grid grid; 
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private List<TileBase> walkableTiles;


    void Start()
    {
        targetWorldPos = GridToWorld(gridPos);
        transform.position = targetWorldPos;
    }

    void Update()
    {
        HandleInput();

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            if (nextDir != Vector2Int.zero && IsWalkable(gridPos + nextDir))
            {
                currentDir = nextDir;
                nextDir = Vector2Int.zero;
            }
            if (IsWalkable(gridPos + currentDir))
            {
                gridPos += currentDir;
                targetWorldPos = GridToWorld(gridPos);
            }
        }
        float distance = Vector3.Distance(transform.position, targetWorldPos);
            if (distance > 0f)
            {
                float t = moveSpeed * Time.deltaTime / distance;
                transform.position = Vector3.Lerp(transform.position, targetWorldPos, t);
            }

        UpdateAnimator();

        TileBase tileAhead = GetTileAt(gridPos + currentDir);
        Debug.Log($"GridPos: {gridPos} CurrentDir: {currentDir} NextDir: {nextDir} TileAhead: {tileAhead}");

    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) nextDir = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) nextDir = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) nextDir = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) nextDir = Vector2Int.right;
    }

    void UpdateAnimator()
    {
        animator.SetFloat("MoveX", currentDir.x);
        animator.SetFloat("MoveY", currentDir.y);
    }

    Vector3 GridToWorld(Vector2Int gridPosition)
    {
        Vector3Int cell = new Vector3Int(gridPosition.x, gridPosition.y, 0);
        return grid.CellToWorld(cell) + (Vector3)grid.cellSize / 2f;
    }

    Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector3Int cell = grid.WorldToCell(worldPosition);
        return new Vector2Int(cell.x, cell.y);
    }

    bool IsWalkable(Vector2Int pos)
{
    TileBase tile = GetTileAt(pos);
    return walkableTiles.Contains(tile);
}

    TileBase GetTileAt(Vector2Int pos)
    {
        Vector3Int cellPos = new Vector3Int(pos.x, pos.y, 0);
        foreach (Tilemap tm in tilemaps)
        {
            TileBase tile = tm.GetTile(cellPos);
            if (walkableTiles.Contains(tile))
                return tile;
        }
        return null;
    }
}
