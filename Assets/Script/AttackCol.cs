using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {

            DamagableEntity dE = other.gameObject.GetComponent<DamagableEntity>();
            if (dE != null)
            {
                dE.Damage(1);
                Debug.Log("Player damaged!");
            }
        }
    }
}
