using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using TMPro;

public class GhostController : MonoBehaviour
{
    public float moveSpeed = 4.5f;
    private Vector2Int currentDir = Vector2Int.right;
    private Vector3 targetWorldPos;
    [SerializeField]private TextMeshProUGUI stb;

    [SerializeField] private Vector3 spawn;

    private float timer;

    private enum BehaviourStates{one, two, three, four};
    [SerializeField] private BehaviourStates behaviour;
    public enum GhostStates{Normal, Dead, Scared, Recovering};
    [SerializeField] public GhostStates state = GhostStates.Normal;


    [SerializeField] private Grid grid;
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private List<TileBase> walkableTiles;
    [SerializeField] private Transform pacman;
    [SerializeField] private Animator animator;
    

    [SerializeField] private AudioSource scaredSound;
    [SerializeField] private AudioSource deadSound;


    private Queue<Vector3Int> pathQueue = new Queue<Vector3Int>();

    [SerializeField] private List<Vector3Int> cornerCells = new List<Vector3Int>();
    private int currentCornerIndex = 0;

    

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
            stb.text = "" + (int)timer;
        }

        if(timer <= 0 & state == GhostStates.Scared)
        {
            state = GhostStates.Normal;
            stb.text = "";
        }



        if(state == GhostStates.Normal & behaviour == BehaviourStates.one)
        {
            Ghost1Behaviour();
        }
        if(state == GhostStates.Normal & behaviour == BehaviourStates.two)
        {
            Ghost2Behaviour();
        }
        if(state == GhostStates.Normal & behaviour == BehaviourStates.three)
        {
            Ghost3Behaviour();
        }
        if(state == GhostStates.Normal & behaviour == BehaviourStates.four)
        {
            Ghost4Behaviour();
        }





        if(state == GhostStates.Scared)
        {
            GhostScared();
            if(!scaredSound.isPlaying){
                scaredSound.Play();
            }
            
            deadSound.Stop();

        }
        if(state == GhostStates.Dead)
        {
            GhostDead();
            if(!deadSound.isPlaying){
                deadSound.Play();
            }
            scaredSound.Stop();
        }
        if(state == GhostStates.Normal){
            scaredSound.Stop();
            deadSound.Stop();
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

    void Ghost1Behaviour()
    {
        moveSpeed = 4.5f;

        // Move ghost toward current target position
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        // When close enough to target, choose a new direction
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            Vector3Int currentCell = grid.WorldToCell(transform.position);

            Vector3Int[] dirs = {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right
            };

            float currentDist = Vector3.Distance(transform.position, pacman.position);

            // Collect all directions where distance to Pac-Man increases or stays equal
            List<Vector3Int> validDirs = new List<Vector3Int>();

            foreach (var dir in dirs)
            {
                Vector3Int next = currentCell + dir;
                if (IsWalkable(next))
                {
                    float distToPacman = Vector3.Distance(grid.CellToWorld(next), pacman.position);

                    if (distToPacman >= currentDist)
                        validDirs.Add(next);
                }
            }

            // Choose random from valid or fallback to any walkable
            Vector3Int chosenCell;

            if (validDirs.Count > 0)
                chosenCell = validDirs[Random.Range(0, validDirs.Count)];
            else
            {
                // fallback to any random walkable
                List<Vector3Int> fallback = new List<Vector3Int>();
                foreach (var dir in dirs)
                {
                    Vector3Int next = currentCell + dir;
                    if (IsWalkable(next)) fallback.Add(next);
                }
                chosenCell = fallback.Count > 0 ? fallback[Random.Range(0, fallback.Count)] : currentCell;
            }

            // Update target position
            targetWorldPos = grid.CellToWorld(chosenCell) + (Vector3)grid.cellSize / 2f;
        }


    }
    
    void Ghost2Behaviour(){
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

    void Ghost3Behaviour()
    {
        moveSpeed = 2.5f;

        // Move ghost toward the current target
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        // When close enough to the target tile, pick a new random direction
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            Vector3Int currentCell = grid.WorldToCell(transform.position);

            // Possible directions
            Vector3Int[] dirs = {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right
            };

            // Collect all walkable directions
            List<Vector3Int> walkableDirs = new List<Vector3Int>();
            foreach (var dir in dirs)
            {
                Vector3Int next = currentCell + dir;
                if (IsWalkable(next))
                    walkableDirs.Add(next);
            }

            // Pick a random valid direction (if any)
            if (walkableDirs.Count > 0)
            {
                Vector3Int chosenCell = walkableDirs[Random.Range(0, walkableDirs.Count)];
                targetWorldPos = grid.CellToWorld(chosenCell) + (Vector3)grid.cellSize / 2f;
            }
        }

    }

    void Ghost4Behaviour()
    {
        moveSpeed = 4.5f;

        // Move toward current target
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        // When close enough to target, decide next step
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            // If no corners defined, do nothing
            if (cornerCells.Count == 0)
                return;

            // If we finished the current path, go to the next corner
            if (pathQueue.Count == 0)
            {
                // Move to next corner in list (looping)
                currentCornerIndex = (currentCornerIndex + 1) % cornerCells.Count;
                Vector3Int nextCorner = cornerCells[currentCornerIndex];

                // Generate a path to that corner
                FindPathTox(nextCorner);
            }

            // If a path exists, keep walking along it
            if (pathQueue.Count > 0)
            {
                Vector3Int nextCell = pathQueue.Dequeue();
                targetWorldPos = grid.CellToWorld(nextCell) + (Vector3)grid.cellSize / 2f;
            }
            else
            {
                // If pathQueue still empty (no path found or already there), 
                // force move to next corner to keep cycling
                currentCornerIndex = (currentCornerIndex + 1) % cornerCells.Count;
                Vector3Int nextCorner = cornerCells[currentCornerIndex];
                FindPathTox(nextCorner);

                if (pathQueue.Count > 0)
                {
                    Vector3Int nextCell = pathQueue.Dequeue();
                    targetWorldPos = grid.CellToWorld(nextCell) + (Vector3)grid.cellSize / 2f;
                }
            }
        }
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
