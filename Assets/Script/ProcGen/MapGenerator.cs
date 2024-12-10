using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Data")]
    [SerializeField]
    float startX = -22.5f;
    [SerializeField]
    float startZ= -22.5f;
    [SerializeField]
    float tileSizeX = 5;
    [SerializeField]
    float tileSizeZ = 5;
    [SerializeField]
    int mapSeed = 0;
    [SerializeField]
    bool randomSeed = false;
    [SerializeField]
    int mapSizeX = 10;
    [SerializeField]
    int mapSizeZ = 10;
    [SerializeField]
    float mapSpawnExitMinGridDistance = 3;

    [Header("Map prefabs")]
    [SerializeField]
    List<MapTileSO> tilesSOs;

    [SerializeField]
    MapTileSO spawnTile;
    [SerializeField]
    MapTileSO exitTile;

    List<List<MapTile>> tileGrid;
    List<List<bool>> dfsVisited;

    // Helper var

    List<(int dx, int dy)> directions = new List<(int, int)>
        {
            (1, 0),  // Right
            (0, 1),  // Up
            (-1, 0), // Left
            (0, -1)  // Down
        };
    // Start is called before the first frame update
    void Start()
    {
        // init vars
        tileGrid = new List<List<MapTile>>();
        dfsVisited = new List<List<bool>>();
        for (int x = 0; x < mapSizeX; x++)
        {
            dfsVisited.Add(new List<bool>());
            tileGrid.Add(new List<MapTile>());
            for (int z = 0; z < mapSizeZ; z++)
            {
                dfsVisited[x].Add(false);
                tileGrid[x].Add(new MapTile());
                if (x == 0)
                {
                    tileGrid[x][z].socketIDs[2] = 0;
                }

                if (x == mapSizeX)
                {
                    tileGrid[x][z].socketIDs[0] = 0;
                }
                if (z == 0)
                {
                    tileGrid[x][z].socketIDs[3] = 0;
                }
                if (z == mapSizeZ)
                {
                    tileGrid[x][z].socketIDs[1] = 0;
                }
            }
        }
        if (!randomSeed)
            Random.InitState(mapSeed);

        GenerateMap();

        SpawnTiles();
    }
    void GenerateMap()
    {
        int startingLocX;
        int startingLocZ;
        int endingLocX;
        int endingLocZ;

        // ProcGen Init
        {
            // Find random area to place starting tile
            startingLocX = Random.Range(0, mapSizeX / 2 );
            startingLocZ = Random.Range(0, mapSizeZ / 2);

            tileGrid[startingLocX][startingLocZ].TileData = spawnTile;

            // Find random area to place ending tile 

            endingLocX = startingLocX;
            endingLocZ = startingLocZ;

           

            do
            {
                endingLocX = Random.Range(0, mapSizeX);
                endingLocZ = Random.Range(0, mapSizeZ);

            } while (Vector2.Distance(new Vector2(endingLocX, endingLocZ), new Vector2(startingLocX, startingLocZ)) < mapSpawnExitMinGridDistance);
            tileGrid[endingLocX][endingLocZ].TileData = exitTile;
            Debug.Log("Start: " + startingLocX.ToString() + " " + startingLocZ.ToString());
            Debug.Log("End: " + endingLocX.ToString() + " " + endingLocZ.ToString());

        }
        {
            LinearPickTiles();
            //DFSPickTiles(startingLocX, startingLocZ);
        }
    }
    void LinearPickTiles()
    {
        for (int x = 0; x < mapSizeX; ++x)
        {
            for (int z = 0; z < mapSizeZ; ++z)
            {
                List<MapTileSO> validTiles = FindValidTiles(x, z);
                if (validTiles.Count > 0)
                    tileGrid[x][z].TileData = validTiles[Random.Range(0, validTiles.Count)];
                UpdateTileSockets();
            }
        }
    }
    void DFSPickTiles(int tileX, int tileZ)
    {
        // Check if out of bounds
        if (tileX < 0 || tileX > mapSizeX - 1|| tileZ < 0 || tileZ > mapSizeZ - 1 || dfsVisited[tileX][tileZ])
            return;
        // Set tile to visited
        dfsVisited[tileX][tileZ] = true;
        // Check if visited tile has tile data
        // TODO: FIX!!
        if (tileGrid[tileX][tileZ].TileData == null)
            if (tileGrid[tileX][tileZ].TileData.tilePrefab == null)
                tileGrid[tileX][tileZ].TileData = tilesSOs[Random.Range(0, tilesSOs.Count)];
            else
                Debug.Log("1");
        else
            Debug.Log("2");
       foreach ((int dx, int dz) in directions)
       {
            Debug.Log("Next tile:" + (tileX + dx).ToString() + " " + (tileZ + dz).ToString());
            DFSPickTiles(tileX + dx, tileZ + dz);
       }
    }
    void UpdateTileSockets()
    {
        for (int x = 0; x < mapSizeX - 1; ++x)
        {
            for (int z = 0; z < mapSizeZ - 1; ++z)
            {
                // Update edge cases
                if (x == 0)
                    tileGrid[x][z].socketIDs[2] = 0;
                if (z == 0)
                    tileGrid[x][z].socketIDs[3] = 0;
                tileGrid[x + 1][z].socketIDs[2] = tileGrid[x][z].socketIDs[0];
                tileGrid[x][z + 1].socketIDs[3] = tileGrid[x][z].socketIDs[1];
            }
        }
    }
    // todo: fix this. stuff is spawning but not in the right way.
    bool CanFitInGridSlot(MapTileSO so, int tileX, int tileZ)
    {
        Debug.Log("Input SO sockets: " + so.socketIDs.ToString());
        string outputStr = "";
        bool fitsRight, fitsUp, fitsLeft, fitsDown;

        

        // check if neighbouring indices match corresponding sockets or if they're -1 (unpicked)
        // Right
        if (tileX + 1 < mapSizeX)
        {
            outputStr += tileGrid[tileX + 1][tileZ].socketIDs[2] + "/";
            fitsRight = so.socketIDs[0] == tileGrid[tileX + 1][tileZ].socketIDs[2] /*&& so.CanConnect(tileGrid[tileX + 1][tileZ], 0)*/
                || tileGrid[tileX + 1][tileZ].socketIDs[2] == -1;
        }
        else
        {
            outputStr +=  "0/";
            fitsRight = so.socketIDs[0] == 0;
        }
        // Up
        if (tileZ + 1 < mapSizeZ)
        {
            outputStr += tileGrid[tileX][tileZ + 1].socketIDs[3] + "/";
            fitsUp = so.socketIDs[1] == tileGrid[tileX][tileZ + 1].socketIDs[3] /*&& so.CanConnect(tileGrid[tileX][tileZ + 1], 1)*/
                || tileGrid[tileX][tileZ + 1].socketIDs[3] == -1;
        }
        else
        {
            outputStr += "0/";
            fitsUp = so.socketIDs[1] == 0;
        }
        // Left
        if (tileX - 1 > 0)
        {
            outputStr += tileGrid[tileX - 1][tileZ].socketIDs[0] + "/";
            fitsLeft = so.socketIDs[2] == tileGrid[tileX - 1][tileZ].socketIDs[0] /*&& so.CanConnect(tileGrid[tileX - 1][tileZ], 2)*/
                || tileGrid[tileX - 1][tileZ].socketIDs[0] == -1;
        }
        else
        {
            outputStr += "0/";
            fitsLeft = so.socketIDs[2] == 0;
        }
        // Down
        if (tileZ - 1 > 0)
        {
            outputStr += tileGrid[tileX][tileZ - 1].socketIDs[1] + "/";
            fitsDown = so.socketIDs[3] == tileGrid[tileX][tileZ - 1].socketIDs[1] /*&& so.CanConnect(tileGrid[tileX][tileZ - 1], 3)*/
                || tileGrid[tileX][tileZ - 1].socketIDs[1] == -1;
        }
        else
        {
            outputStr += "0/";
            fitsDown = so.socketIDs[3] == 0;
        }
        Debug.Log(outputStr);
        Debug.Log(fitsRight && fitsUp && fitsDown && fitsLeft);
        return fitsRight && fitsUp && fitsDown && fitsLeft;
    }
    List<MapTileSO> FindValidTiles(int tileX, int tileZ)
    {
        List<MapTileSO> returnList = new List<MapTileSO> ();
       foreach (MapTileSO so in tilesSOs)
       {
            if (CanFitInGridSlot(so, tileX, tileZ))
            {
                returnList.Add(so);
            }
       }
       return returnList;
    }
    void SpawnTiles()
    {
       for(int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeZ; y++)
            {
                if (tileGrid[x][y].TileData != null) 
                {
                    if (tileGrid[x][y].TileData.tilePrefab != null)
                    {
                        Vector3 currentPosition = new Vector3(startX + (mapSizeX * x), 0, startZ + (mapSizeZ * y));
                        Instantiate(tileGrid[x][y].TileData.tilePrefab, currentPosition, Quaternion.identity);
                    }
                }
            }
        }
    }
}
