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
    float tileSize = 5;
    [SerializeField]
    int mapSeed = 0;
    [SerializeField]
    int mapSizeX = 10;
    [SerializeField]
    int mapSizeY = 10;

    // TODO: update with tile system
    [Header("Map Prefabs")]
    [SerializeField]
    List<GameObject> mapPrefabs;


    // TODO: tile system then uncomment
    //List<List<MapTile>> tiles;

    List<List<GameObject>> tileObjects;

    // Start is called before the first frame update
    void Start()
    {
        // init vars
        List<GameObject> list = new List<GameObject>();
        Random.InitState(mapSeed);

        // startProcGen
    }
}
