using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
    }
    public void Interact()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width, Screen.height));

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
            if (objectHit != null)
            {
                IInteractable interactable = objectHit.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract();
                }
            }
        }
    }
}