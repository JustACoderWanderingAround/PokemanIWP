using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity data")]
    [SerializeField]
    string entityName;
    [SerializeField]
    string description;
    [SerializeField]
    int maxHealth;
    public int GetMaxHealth() { return maxHealth; }
    int currentHealth;
    public int GetCurrentHealth() { return currentHealth; }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
