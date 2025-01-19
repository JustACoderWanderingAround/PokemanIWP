using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    Camera mainCam;
    [SerializeField]
    float maxHitDistance = 10000f;
    private void Start()
    {
        mainCam = Camera.main;
    }
    public void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxHitDistance))
        {
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
            if (objectHit != null)
            {
                IInteractable interactable = objectHit.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnHover();
                    Debug.Log("Interactable works");
                }
            }
        }
    }
    public void Interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxHitDistance))
        {
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
            if (objectHit != null)
            {
                IInteractable interactable = objectHit.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract();
                    Debug.Log("Interactable works");
                }
            }
        }
    }
}