using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDecorationGenerator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> availableDecoPrefabs;
    [SerializeField]
    List<GameObject> availableDecoSpawnLocs;
    private void Start()
    {
        foreach (GameObject availTileLoc in availableDecoSpawnLocs)
        {
            int randomIndex = Random.Range((int)0, (int)availableDecoPrefabs.Count);
            Instantiate(availableDecoPrefabs[randomIndex], availTileLoc.transform);
        }
    }
}
