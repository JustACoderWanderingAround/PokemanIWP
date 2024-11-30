using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTeleporter : MonoBehaviour
{
    [SerializeField]
    bool activated;
    private void OnTriggerEnter(Collider other)
    {
        if (activated)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Final teleporter activated!");
            }
        }
    }
}
