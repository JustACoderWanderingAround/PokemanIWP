using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTeleporter : MonoBehaviour
{
    [SerializeField]
    bool activated;
    [SerializeField]
    GameObject endScreen;
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            if (activated)
            {
                Debug.Log("Final teleporter activated!");
                endScreen.SetActive(true);
            }
            else
            {
                Debug.Log("Teleporter not active!");
            }
        }
    }
    public void ActivateTeleporter()
    {
        activated = true;
    }
}
