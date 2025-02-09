using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamagableEntity
{
    public System.Action OnDeathEvent;
    public System.Action OnPlayerObservedEvent;
    bool isAlive;
    private void Start()
    {
        isAlive = true;
    }
    protected void OnPlayerObserved()
    {
        OnPlayerObservedEvent.Invoke();
    }
    protected void OnDeath()
    {
        isAlive = false;
        if (OnDeathEvent != null)
        {
            OnDeathEvent.Invoke();
        }
    }
}
