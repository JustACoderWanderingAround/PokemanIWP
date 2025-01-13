using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public List<MapTileSO> allowedConnectionsXPos;
    public List<MapTileSO> allowedConnectionsZPos;
    public List<MapTileSO> allowedConnectionsXNeg;
    public List<MapTileSO> allowedConnectionsZNeg;
    public List<List<MapTileSO>> allowedConnections;
    private MapTileSO tileData;
    public MapTileSO TileData { 
        get { return tileData; } 
        set {
            tileData = value;
            Init(value.allowedConnectionsXPos,
                value.allowedConnectionsZPos,
                value.allowedConnectionsXNeg,
                value.allowedConnectionsZNeg);
                } 
    }
    public void Init(List<MapTileSO> aXP, List<MapTileSO> aYP, List<MapTileSO> aXN, List<MapTileSO> aYN)
    {
        allowedConnectionsXPos = aXP;
        allowedConnectionsZPos = aYP;
        allowedConnectionsXNeg = aXN;
        allowedConnectionsZNeg = aYN;
    }
    public MapTile()
    {
    }
    public MapTile(MapTileSO newData)
    {
        TileData = newData;
    }
}
