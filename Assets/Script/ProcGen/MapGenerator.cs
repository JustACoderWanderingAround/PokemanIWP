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
            //LinearPickTiles();
            DFSPickTiles(startingLocX, startingLocZ);
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
        if (tileGrid[tileX][tileZ].TileData == null) {
            tileGrid[tileX][tileZ].TileData = tilesSOs[Random.Range(0, tilesSOs.Count)];
        }
        else 
        {
            Debug.Log("2");
        }

        foreach ((int dx, int dz) in directions)
        {
            Debug.Log("Next tile:" + (tileX + dx).ToString() + " " + (tileZ + dz).ToString());
            DFSPickTiles(tileX + dx, tileZ + dz);
        }

    }
    void UpdateTileSockets()
    {
        //for (int x = 0; x < mapSizeX - 1; ++x)
        //{
        //    for (int z = 0; z < mapSizeZ - 1; ++z)
        //    {
        //        // Update edge cases
        //        if (x == 0)
        //            tileGrid[x][z].socketIDs[2] = 0;
        //        if (z == 0)
        //            tileGrid[x][z].socketIDs[3] = 0;
        //        tileGrid[x + 1][z].socketIDs[2] = tileGrid[x][z].socketIDs[0];
        //        tileGrid[x][z + 1].socketIDs[3] = tileGrid[x][z].socketIDs[1];
        //    }
        //}
    }
    // todo: fix this. stuff is spawning but not in the right way.
    bool CanFitInGridSlot(MapTileSO so, int tileX, int tileZ)
    {
        string outputStr = "";
        bool fitsRight = false, fitsUp = false, fitsLeft = false, fitsDown = false;



        // check if neighbouring indices match corresponding sockets or if they're -1 (unpicked)
        // Right
        if (tileX + 1 < mapSizeX)
        {
            if (tileGrid[tileX + 1][tileZ].TileData != null)
            {
                if (!tileGrid[tileX + 1][tileZ].TileData.allowAllXPos)
                {
                    MapTileSO currNeighbour = tileGrid[tileX + 1][tileZ].TileData;
                    fitsRight = CanConnect(so, currNeighbour, directions[0]);
                }
                else
                    fitsRight = true;
            }
            else
                fitsRight = true;
        }
        else
        {
            fitsRight = true;
        }
        // Up
        if (tileZ + 1 < mapSizeZ)
        {
            if (tileGrid[tileX][tileZ + 1].TileData != null)
            {
                if (!tileGrid[tileX][tileZ + 1].TileData.allowAllXPos)
                {
                    MapTileSO currNeighbour = tileGrid[tileX][tileZ + 1].TileData;
                    fitsUp = CanConnect(so, currNeighbour, directions[1]);
                }
                else
                    fitsUp = true;
            }
            else
                fitsUp = true;
        }
        else
        {
            fitsUp = true;
        }
        // Left
        if (tileX - 1 > 0)
        {
            if (tileGrid[tileX - 1][tileZ].TileData != null)
            {
                if (!tileGrid[tileX - 1][tileZ].TileData.allowAllXPos)
                {
                    MapTileSO currNeighbour = tileGrid[tileX - 1][tileZ].TileData;
                    fitsLeft = CanConnect(so, currNeighbour, directions[2]);
                }
                else
                    fitsLeft = true;
            }
            else
                fitsLeft = true;
        }
        else
        {
            fitsLeft = true;
        }
        // Down
        if (tileZ - 1 > 0)
        {
            if (tileGrid[tileX][tileZ - 1].TileData != null)
            {
                if (!tileGrid[tileX][tileZ - 1].TileData.allowAllXPos)
                {
                    MapTileSO currNeighbour = tileGrid[tileX][tileZ - 1].TileData;
                    fitsDown = CanConnect(so, currNeighbour, directions[2]);
                }
                else
                    fitsDown = true;
            }
            else
                fitsDown = true;
        }
        else
        {
            fitsDown = true;
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
            for (int z = 0; z < mapSizeZ; z++)
            {
                if (tileGrid[x][z].TileData != null) 
                {
                    if (tileGrid[x][z].TileData.tilePrefab != null)
                    {
                        Vector3 currentPosition = new Vector3(startX + (tileSizeX * x), 0, startZ + (tileSizeZ * z));
                        Instantiate(tileGrid[x][z].TileData.tilePrefab, currentPosition, Quaternion.identity);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Checks if a can connect to b. directions is relative to a.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="directions"></param>
    /// <returns></returns>
    bool CanConnect(MapTileSO a, MapTileSO b, (int, int) directions)
    {
        bool returnBool = false;
        switch (directions)
        {
            case (1, 0):
                return (SearchList(a, b.allowedConnectionsXNeg) || a.allowedConnectionsXPos == b.allowedConnectionsXNeg) ;
            case (0, 1):
                return (SearchList(a, b.allowedConnectionsZNeg) || a.allowedConnectionsZPos == b.allowedConnectionsZNeg);
            case (-1, 0):
                return (SearchList(a, b.allowedConnectionsXPos) || a.allowedConnectionsXNeg == b.allowedConnectionsXPos);
            case (0, -1):
                return (SearchList(a, b.allowedConnectionsZPos) || a.allowedConnectionsZNeg == b.allowedConnectionsZPos);
        }
        return returnBool;
    }
    /// <summary>
    /// Checks if a can connect to b. directions is relative to a.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="directions"></param>
    /// <returns></returns>
    bool SearchList(MapTileSO toFind, List<MapTileSO> toSearch)
    {
        foreach (MapTileSO item in toSearch)
        {
            if (toFind == item)
            {
                return true;
            }
        }
        return false;
    }
}
