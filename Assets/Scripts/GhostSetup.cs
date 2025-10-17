using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostSetup : MonoBehaviour
{

    [SerializeField] private RuntimeAnimatorController redGhostController;
    [SerializeField] private RuntimeAnimatorController pinkGhostController;
    [SerializeField] private RuntimeAnimatorController blueGhostController;
    [SerializeField] private RuntimeAnimatorController yellowGhostController;

    private Animator animator;

    [SerializeField] private Grid grid;
    [SerializeField] private Vector2Int gridSpawnPos = new Vector2Int(0,0);
    [SerializeField] private bool snapToGridOnStart = true;

    private Vector3 targetWorldPos;

    
    public enum GhostType { Red, Pink, Blue, Yellow }
    [SerializeField] private GhostType ghostType;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        AssignAnimatorController();
    }

    void Start()
    {
        if (snapToGridOnStart)
        {
            SnapToGrid();
        }
    }

    public void SnapToGrid()
    {
        if (grid == null)
        {
            Debug.LogWarning("Grid reference missing on GhostSpawn!");
            return;
        }

        Vector3Int cell = new Vector3Int(gridSpawnPos.x, gridSpawnPos.y, 0);
        targetWorldPos = grid.CellToWorld(cell) + (Vector3)grid.cellSize / 2f;
        transform.position = targetWorldPos;
    }

    // Optional: set the spawn position from another script
    public void SetSpawn(Vector2Int newGridPos)
    {
        gridSpawnPos = newGridPos;
        SnapToGrid();
    }

    private void AssignAnimatorController()
    {
        switch (ghostType)
        {
            case GhostType.Red:
                animator.runtimeAnimatorController = redGhostController;
                break;
            case GhostType.Pink:
                animator.runtimeAnimatorController = pinkGhostController;
                break;
            case GhostType.Blue:
                animator.runtimeAnimatorController = blueGhostController;
                break;
            case GhostType.Yellow:
                animator.runtimeAnimatorController = yellowGhostController;
                break;
        }
    }
}
