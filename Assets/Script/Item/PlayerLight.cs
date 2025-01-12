using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour, UsableItem
{
    [SerializeField]
    GameObject lightSource;

    public bool IsPrimary()
    {
        return false;
    }

    public bool PrimaryUse()
    {
        lightSource.SetActive(true);
        return true;
    }

    public bool SecondaryUse()
    {
        lightSource.SetActive(false);
        return true;
    }
    
}
