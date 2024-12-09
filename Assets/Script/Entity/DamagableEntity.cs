using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagableEntity : Entity
{
    [SerializeField]
    int maxHealth;
    public int GetMaxHealth() { return maxHealth; }
    int currentHealth;
    public int GetCurrentHealth() { return currentHealth; }
    public UnityEvent OnDamageEvent;

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
        OnDamageEvent.Invoke();
    }
    public bool IsAlive() => isAlive;
}
