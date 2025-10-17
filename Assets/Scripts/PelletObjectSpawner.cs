using UnityEngine;
using UnityEngine.Tilemaps;

public class PelletTilemapSpawner : MonoBehaviour
{
    [Header("Tilemap and Prefabs")]
    [SerializeField] private Tilemap pelletTilemap;
    [SerializeField] private TileBase normalPelletTile;
    [SerializeField] private TileBase powerPelletTile;
    [SerializeField] private GameObject normalPelletPrefab;
    [SerializeField] private GameObject powerPelletPrefab;

    [Header("Position Offset")]
    [Tooltip("Adjust this if pellets appear slightly off-center from their tiles")]
    [SerializeField] private Vector3 offset = Vector3.zero;

    void Start()
    {
        if (pelletTilemap == null)
        {
            Debug.LogError("PelletTilemapSpawner: No Tilemap assigned!");
            return;
        }

        foreach (Vector3Int pos in pelletTilemap.cellBounds.allPositionsWithin)
        {
            if (!pelletTilemap.HasTile(pos))
                continue;

            TileBase tile = pelletTilemap.GetTile(pos);
            GameObject prefabToSpawn = null;

            if (tile == normalPelletTile) prefabToSpawn = normalPelletPrefab;
            else if (tile == powerPelletTile) prefabToSpawn = powerPelletPrefab;

            if (prefabToSpawn != null)
            {
                Vector3 worldPos = pelletTilemap.GetCellCenterWorld(pos) + offset;
                GameObject pellet = Instantiate(prefabToSpawn, worldPos, Quaternion.identity, transform);

                // Force Z to 0 to stay in 2D plane
                Vector3 p = pellet.transform.position;
                p.z = 0;
                pellet.transform.position = p;

                pelletTilemap.SetTile(pos, null);
            }
        }
    }
}
