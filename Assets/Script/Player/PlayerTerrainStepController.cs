using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTerrainStepController : MonoBehaviour
{
    [SerializeField]
    SoundGenerator soundGenerator;

    enum TerrainType
    {
        Terrain_Default = 0,
        Terrain_Water = 1
    }
    TerrainType currTerrain;
    private void Start()
    {
        currTerrain = TerrainType.Terrain_Default;
    }
    public void CheckFootStep()
    {
        if (currTerrain > 0)
        {
            Debug.Log("Splash sound played");
            soundGenerator.PlaySoundOnce(((int)currTerrain) - 1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            Debug.Log("PTSC: Terrain enter");
            if (other.GetComponent<Terrain>() != null)
            currTerrain = (TerrainType)other.gameObject.GetComponent<Terrain>().terrainNum;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            Debug.Log("PTSC: Terrain exit");
            currTerrain = TerrainType.Terrain_Default;
        }
    }
}
