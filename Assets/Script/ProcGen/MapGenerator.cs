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

    List<List<MapTileSO>> tileSOGrid;
    List<List<MapTile>> tileGrid;
    List<List<bool>> dfsVisited;
    // Start is called before the first frame update
    void Start()
    {
        // init vars
        tileSOGrid = new List<List<MapTileSO>>();
        tileGrid = new List<List<MapTile>>();
        dfsVisited = new List<List<bool>>();
        for (int x = 0; x < mapSizeX; x++)
        {
            dfsVisited.Add(new List<bool>());
            tileSOGrid.Add(new List<MapTileSO>());
            for (int z = 0; z < mapSizeZ; z++)
            {
                dfsVisited[x].Add(false);
                tileSOGrid[x].Add(new MapTileSO());
                if (x == 0)
                    tileSOGrid[x][z].socketIDs[2] = 0;
                if (x == mapSizeX)
                    tileSOGrid[x][z].socketIDs[0] = 0;
                if (z == 0)
                    tileSOGrid[x][z].socketIDs[3] = 0;
                if (z == mapSizeZ)
                    tileSOGrid[x][z].socketIDs[1] = 0;
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

            tileSOGrid[startingLocX][startingLocZ] = spawnTile;

            // Find random area to place ending tile 

            endingLocX = startingLocX;
            endingLocZ = startingLocZ;

           

            do
            {
                endingLocX = Random.Range(0, mapSizeX);
                endingLocZ = Random.Range(0, mapSizeZ);

            } while (Vector2.Distance(new Vector2(endingLocX, endingLocZ), new Vector2(startingLocX, startingLocZ)) < mapSpawnExitMinGridDistance);
            tileSOGrid[endingLocX][endingLocZ] = exitTile;
            Debug.Log("Start: " + startingLocX.ToString() + " " + startingLocZ.ToString());
            Debug.Log("End: " + endingLocX.ToString() + " " + endingLocZ.ToString());

        }
        {
            LinearPickTiles();
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
                    tileSOGrid[x][z] = validTiles[Random.Range(0, validTiles.Count)];
                UpdateTileSockets();
            }
        }
    }
    void DFSPickTiles(int tileX, int tileZ)
    {
        // Right
        if (tileX + 1 < mapSizeX)
        {
            List<int> rightSockets = tileSOGrid[tileX + 1][tileZ].socketIDs;
        }
        // Up
        if (tileZ + 1 < mapSizeZ)
        {
            List<int> upSockets = tileSOGrid[tileX][tileZ + 1].socketIDs;
        }
        // Down
        if (tileX - 1 > 0)
        {
            List<int> leftSockets = tileSOGrid[tileX - 1][tileZ].socketIDs;
        }
        // Left
        if (tileZ - 1 > 0)
        {
            List<int> downSockets = tileSOGrid[tileX][tileZ - 1].socketIDs;
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
                    tileSOGrid[x][z].socketIDs[2] = 0;
                if (z == 0)
                    tileSOGrid[x][z].socketIDs[3] = 0;
                tileSOGrid[x + 1][z].socketIDs[2] = tileSOGrid[x][z].socketIDs[0];
                tileSOGrid[x][z + 1].socketIDs[3] = tileSOGrid[x][z].socketIDs[1];
            }
        }
    }
    // todo: fix this. stuff is spawning but not in the right way.
    bool CanFitInGridSlot(MapTileSO so, int tileX, int tileZ)
    {
        bool fitsRight, fitsUp, fitsLeft, fitsDown;

        // get reference to neighbouring indices
        List<Vector2> neighbours = new List<Vector2>() { new Vector2(tileX + 1, tileZ), new Vector2(tileX, tileZ + 1), new Vector2(tileX - 1, tileZ), new Vector2(tileX, tileZ - 1) };

        // check if neighbouring indices match corresponding sockets or if they're -1 (unpicked)
        if (tileX + 1 < mapSizeX)
            fitsRight = so.socketIDs[0] == tileSOGrid[tileX + 1][tileZ].socketIDs[2] /*&& so.CanConnect(tileSOGrid[tileX + 1][tileZ], 0)*/
                || tileSOGrid[tileX + 1][tileZ].socketIDs[2] == -1;
        else
            fitsRight = so.socketIDs[0] == 0;
        if (tileZ + 1 < mapSizeZ)
            fitsUp = so.socketIDs[1] == tileSOGrid[tileX][tileZ + 1].socketIDs[3] /*&& so.CanConnect(tileSOGrid[tileX][tileZ + 1], 1)*/
                || tileSOGrid[tileX][tileZ + 1].socketIDs[3] == -1;
        else
            fitsUp = so.socketIDs[1] == 0;
        if (tileX - 1 > 0)
            fitsLeft = so.socketIDs[2] == tileSOGrid[tileX - 1][tileZ].socketIDs[0] /*&& so.CanConnect(tileSOGrid[tileX - 1][tileZ], 2)*/
                || tileSOGrid[tileX - 1][tileZ].socketIDs[0] == -1;
        else
            fitsLeft = so.socketIDs[2] == 0;
        if (tileZ - 1 > 0)
            fitsDown = so.socketIDs[3] == tileSOGrid[tileX][tileZ - 1].socketIDs[1] /*&& so.CanConnect(tileSOGrid[tileX][tileZ - 1], 3)*/
                || tileSOGrid[tileX][tileZ - 1].socketIDs[1] == -1;
        else
            fitsDown = so.socketIDs[3] == 0;

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
                if (tileSOGrid[x][y].tilePrefab != null)
                {
                    Vector3 currentPosition = new Vector3(startX + (mapSizeX * x), 0, startZ + (mapSizeZ * y));
                    Instantiate(tileSOGrid[x][y].tilePrefab, currentPosition, Quaternion.identity);
                }
            }
        }
    }
}
