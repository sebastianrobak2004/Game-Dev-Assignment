using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]public Tilemap map;
    [SerializeField]private TileBase[] tiles;
    
    
    private int[,] levelMap = { 
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4}, 
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5}, 
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4}, 
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3}, 
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4}, 
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4}, 
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3}, 
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0}, 
        {0,0,0,0,0,2,5,4,4,0,3,4,4,8}, 
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0}, 
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}, };
    
    void Start()
    {
        map.ClearAllTiles();
        levelMap = GenerateFullMap(levelMap);
        GenerateLevel();
    }

    void Update()
    {
        
    }
    private int[,] GenerateFullMap(int[,] baseMap)
{
    int rows = baseMap.GetLength(0);
    int cols = baseMap.GetLength(1);

    int fullRows = rows * 2;
    int fullCols = cols * 2;

    int[,] fullMap = new int[fullRows, fullCols];

    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            int value = baseMap[y, x];
      
            fullMap[y, x] = value;
        
            fullMap[y, fullCols - 1 - x] = value;
  
            fullMap[fullRows - 1 - y, x] = value;

            fullMap[fullRows - 1 - y, fullCols - 1 - x] = value;
        }
    }

    return fullMap;
}

    private void GenerateLevel(){
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);
        

        for (int y = 0; y < rows; y++){
            for (int x = 0; x < cols; x++){
                int tileIndex = levelMap[y,x];
                Vector3Int pos = new Vector3Int(x,-y,0);
                PlaceTile(pos, tileIndex, DecideRotation(pos,tileIndex));
            }
        }
    }

private float DecideRotation(Vector3Int pos, int tileIndex)
{
    int x = pos.x;
    int y = -pos.y;
    int rows = levelMap.GetLength(0);
    int cols = levelMap.GetLength(1);
    List<int> relevantTiles;

    if (tileIndex == 1 || tileIndex == 3)
    {
        relevantTiles = new List<int> { 1, 2, 3, 4,7,8};
        if (x + 1 < cols && y + 1 < rows && relevantTiles.Contains(levelMap[y, x + 1]) && relevantTiles.Contains(levelMap[y + 1, x])) return 0f; // top left
        if (x - 1 >= 0 && y + 1 < rows && relevantTiles.Contains(levelMap[y, x - 1]) && relevantTiles.Contains(levelMap[y + 1, x])) return -90f; // top right
        if (x - 1 >= 0 && y - 1 >= 0 && relevantTiles.Contains(levelMap[y, x - 1]) && relevantTiles.Contains(levelMap[y - 1, x])) return 180f; // bot right
        if (x + 1 < cols && y - 1 >= 0 && relevantTiles.Contains(levelMap[y, x + 1]) && relevantTiles.Contains(levelMap[y - 1, x])) return 90f; // bot left
        return 90f;
    }

        if (tileIndex == 2 || tileIndex == 4 || tileIndex == 8)
    {
        relevantTiles = new List<int> { 1, 2, 3, 4,7,8 };

        bool left  = x - 1 >= 0 && relevantTiles.Contains(levelMap[y, x - 1]);
        bool right = x + 1 < cols && relevantTiles.Contains(levelMap[y, x + 1]);
        bool top   = y + 1 < rows && relevantTiles.Contains(levelMap[y + 1, x]);
        bool bottom= y - 1 >= 0 && relevantTiles.Contains(levelMap[y - 1, x]);

        if (left && right && !top && !bottom) return 90f;

        if (!left && !right && top && bottom) return 0f;

        if(left && right && top && !bottom) return 90f;
        if(left && right && !top && bottom) return 90f;
        if(left && !right && top && bottom) return 0f;
        if(!left && right && top && bottom) return 0f;

        if(left && !right && !top && !bottom) return 90f;
        if(!left && right && !top && !bottom) return 90f;
        if(!left && !right && top && !bottom) return 0f;
        if(!left && !right && !top && bottom) return 0f;

        if(left) return 90f;
    }

    return -90f;
}







    private void PlaceTile(Vector3Int position, int tileIndex, float rotationZ){
        if (tileIndex < 0 || tileIndex >= tiles.Length || tiles[tileIndex] == null)
            return;

        map.SetTile(position, tiles[tileIndex]);

        Quaternion rotation = Quaternion.Euler(0f, 0f, rotationZ);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        map.SetTransformMatrix(position, matrix);
        
    }
}
