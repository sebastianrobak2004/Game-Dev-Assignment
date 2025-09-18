using UnityEngine;

public class PacmanMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2Int gridPos; 
    private Vector2Int currentDir = Vector2Int.zero;
    private Vector2Int nextDir = Vector2Int.zero;

    private Vector3 targetWorldPos;

    [SerializeField] private Animator animator;
    [SerializeField] private Grid grid; 
    void Start()
    {
        animator.SetBool("dead", false);
        targetWorldPos = GridToWorld(gridPos);
        transform.position = targetWorldPos;
    }

    void Update()
    {
        HandleInput();

      
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            gridPos += currentDir;
            targetWorldPos = GridToWorld(gridPos);

           
            if (nextDir != Vector2Int.zero)
            {
                currentDir = nextDir;
                nextDir = Vector2Int.zero;
            }
        }

        
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWorldPos,
            moveSpeed * Time.deltaTime
        );

        UpdateAnimator();
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
}
