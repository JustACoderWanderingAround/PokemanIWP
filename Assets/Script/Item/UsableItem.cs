using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : MonoBehaviour
{
    public abstract void PrimaryUse();
    public abstract void SecondaryUse();
}
