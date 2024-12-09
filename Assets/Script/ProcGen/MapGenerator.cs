using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Data")]
    [SerializeField]
    float startX = -22.5f;
    [SerializeField]
    float startY = -22.5f;
    [SerializeField]
    float tileSizeX = 5;
    [SerializeField]
    float tileSizeY = 5;
    [SerializeField]
    int mapSeed = 0;
    [SerializeField]
    int mapSizeX = 10;
    [SerializeField]
    int mapSizeY = 10;
    [SerializeField]
    float mapSpawnExitMinGridDistance = 3;

    [Header("Map prefabs")]
    [SerializeField]
    List<MapTileSO> tilesSOs;

    [SerializeField]
    MapTileSO spawnTile;
    [SerializeField]
    MapTileSO exitTile;

    List<List<MapTileSO>> tileGrid;
    
    // Start is called before the first frame update
    void Start()
    {
        // init vars
        tileGrid = new List<List<MapTileSO>>();
        for (int x = 0; x < mapSizeX; x++)
        {
            tileGrid.Add(new List<MapTileSO>());
            for (int y = 0; y < mapSizeY; y++)
            {
                tileGrid[x].Add(new MapTileSO());
            }
        }
        Random.InitState(mapSeed);

        GenerateMap();

        SpawnTiles();
    }
    void GenerateMap()
    {
        // ProcGen Init
        {
            // Find random area to place starting tile
            int startingLocX = Random.Range(0, mapSizeX / 2 );
            int startingLocY = Random.Range(0, mapSizeY / 2);

            tileGrid[startingLocX][startingLocY] = spawnTile;

            // Find random area to place ending tile 

            int endingLocX = startingLocX;
            int endingLocY = startingLocY;

           

            do
            {
                endingLocX = Random.Range(0, mapSizeX);
                endingLocY = Random.Range(0, mapSizeY);

            } while (Vector2.Distance(new Vector2(endingLocX, endingLocY), new Vector2(startingLocX, startingLocY)) < mapSpawnExitMinGridDistance);
            tileGrid[endingLocX][endingLocY] = exitTile;
            Debug.Log("Start: " + startingLocX.ToString() + " " + startingLocY.ToString());
            Debug.Log("End: " + endingLocX.ToString() + " " + endingLocY.ToString());
        }
        // ProcGen Start
        {
            
        }
    }
    void SpawnTiles()
    {
       for(int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (tileGrid[x][y].tilePrefab != null)
                {
                    Vector3 currentPosition = new Vector3(startX + (mapSizeX * x), 0, startY + (mapSizeY * y));
                    Instantiate(tileGrid[x][y].tilePrefab, currentPosition, Quaternion.identity);
                }
            }
        }
    }
}
