using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<Enemy> spawnableEnemies;
    [SerializeField]
    List<float> respectiveSpawnChances;
    [SerializeField]
    int numberToSpawn;
    public void SpawnEnemies(List<GameObject> potentialLocations)
    {
        // Validate inputs
        if (spawnableEnemies == null || spawnableEnemies.Count == 0 ||
            respectiveSpawnChances == null || respectiveSpawnChances.Count != spawnableEnemies.Count ||
            potentialLocations == null || potentialLocations.Count == 0)
        {
            Debug.LogError("Invalid inputs for enemy spawning.");
            return;
        }

        // Normalize spawn chances
        float totalChance = 0;
        foreach (float chance in respectiveSpawnChances)
        {
            totalChance += chance;
        }

        if (totalChance <= 0)
        {
            Debug.LogError("Total spawn chances must be greater than 0.");
            return;
        }

        List<float> normalizedChances = new List<float>();
        foreach (float chance in respectiveSpawnChances)
        {
            normalizedChances.Add(chance / totalChance);
        }

        // Spawn enemies
        for (int i = 0; i < numberToSpawn; i++)
        {
            // Choose a random location
            int randomLocationIndex = Random.Range(0, potentialLocations.Count);
            GameObject spawnLocation = potentialLocations[randomLocationIndex];

            // Choose an enemy based on weighted chances
            float randomValue = Random.value; // Value between 0 and 1
            float cumulativeChance = 0;
            int chosenEnemyIndex = 0;

            for (int j = 0; j < normalizedChances.Count; j++)
            {
                cumulativeChance += normalizedChances[j];
                if (randomValue <= cumulativeChance)
                {
                    chosenEnemyIndex = j;
                    break;
                }
            }

            // Instantiate the chosen enemy at the spawn location
            Enemy chosenEnemy = spawnableEnemies[chosenEnemyIndex];
            Instantiate(chosenEnemy.gameObject, spawnLocation.transform.position, Quaternion.identity);
        }
    }
}
