using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : MonoBehaviour
{
    public abstract bool PrimaryUse();
    public abstract bool SecondaryUse();
}
