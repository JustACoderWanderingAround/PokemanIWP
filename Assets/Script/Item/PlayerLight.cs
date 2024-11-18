using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : UsableItem
{
    [SerializeField]
    GameObject lightSource;
    public override bool PrimaryUse()
    {
        lightSource.SetActive(true);
        return true;
    }

    public override bool SecondaryUse()
    {
        lightSource.SetActive(false);
        return true;
    }
}
