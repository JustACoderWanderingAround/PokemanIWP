using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProcGenMapTile")]
public class MapTileSO : ScriptableObject
{
    [Header("Blocks that can connect")]
    public List<MapTileSO> allowedConnectionsXPos;
    public bool allowAllXPos;
    public List<MapTileSO> allowedConnectionsZPos; 
    public bool allowAllZPos;
    public List<MapTileSO> allowedConnectionsXNeg;
    public bool allowAllXNeg;
    public List<MapTileSO> allowedConnectionsZNeg;
    public bool allowAllZNeg;
    public GameObject tilePrefab;
}
