using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableSingletonStarter : MonoBehaviour
{
    private void Start()
    {
        var a = PlayerInventory.Instance;
        var b = ObjectiveManager.Instance;
        var c = EventPostOffice.Instance;
    }
}
