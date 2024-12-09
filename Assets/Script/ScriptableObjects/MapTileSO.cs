using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileSO : ScriptableObject
{
    [SerializeField]
    List<MapTileSO> allowedNeighboursXPos;
    [SerializeField]
    List<MapTileSO> allowedNeighboursXNeg;
    [SerializeField]
    List<MapTileSO> allowedNeighboursYPos;
    [SerializeField]
    List<MapTileSO> allowedNeighboursYNeg;

}
