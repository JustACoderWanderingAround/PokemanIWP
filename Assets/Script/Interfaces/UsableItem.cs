using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UsableItem
{
    public bool PrimaryUse();
    public bool SecondaryUse();
    public bool IsRightHanded();
}
