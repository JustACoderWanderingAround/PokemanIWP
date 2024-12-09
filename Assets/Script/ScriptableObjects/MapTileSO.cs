using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProcGenMapTile")]
public class MapTileSO : ScriptableObject
{
    [Header("0 for blocked, 1 for clear. ")]
    public List<int> socketIDs = new List<int>() { 1, 1, 1, 1 };
    public GameObject tilePrefab;
    [Header("Blocks that cannot connect on a specific socket")]
    public List<GameObject> disallowedConnectionsXPos;
    public List<GameObject> disallowedConnectionsYPos; 
    public List<GameObject> disallowedConnectionsXNeg;
    public List<GameObject> disallowedConnectionsYNeg;
}
