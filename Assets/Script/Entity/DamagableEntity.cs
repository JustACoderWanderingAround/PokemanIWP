using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableEntity : Entity
{
    [SerializeField]
    int maxHealth;
    public int GetMaxHealth() { return maxHealth; }
    int currentHealth;
    public int GetCurrentHealth() { return currentHealth; }

    bool isAlive;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void Damage(int dmg)
    {
        currentHealth -= dmg;
        isAlive = currentHealth > 0;
    }
    public bool IsAlive() => isAlive;
}
