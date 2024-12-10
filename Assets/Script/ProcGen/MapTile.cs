using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public List<int> socketIDs = new List<int>() { -1, -1, -1, -1 };
    private MapTileSO tileData;
    public MapTileSO TileData { get { return tileData; } set { tileData = value; socketIDs = tileData.socketIDs; } }
}
