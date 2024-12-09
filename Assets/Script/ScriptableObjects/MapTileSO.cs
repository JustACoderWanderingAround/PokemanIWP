using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProcGenMapTile")]
public class MapTileSO : ScriptableObject
{
    [Header("0 for blocked, 1 for clear. ")]
    public List<int> socketIDs = new List<int>() { -1, -1, -1, -1 };
    public GameObject tilePrefab;
    [Header("Blocks that cannot connect on a specific socket")]
    public List<MapTileSO> disallowedConnectionsXPos;
    public List<MapTileSO> disallowedConnectionsYPos; 
    public List<MapTileSO> disallowedConnectionsXNeg;
    public List<MapTileSO> disallowedConnectionsYNeg;

    public bool CanConnect(MapTileSO other, int dir)
    {
        List<List<MapTileSO>> dCon = new List<List<MapTileSO>>() { disallowedConnectionsXPos, disallowedConnectionsYPos, disallowedConnectionsXNeg, disallowedConnectionsYNeg };
        foreach (MapTileSO so in dCon[dir])
        {
            if (other == so)
                return false;
        }
        return true;
    }
}
