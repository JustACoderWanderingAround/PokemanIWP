using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    Camera mainCam;
    [SerializeField]
    float maxHitDistance = 10000f;
    string currHoverString;
    string savedStringState;
    public System.Action<string> OnHoverStringUpdate;
    private void Start()
    {
        mainCam = Camera.main;
        currHoverString = "";
        savedStringState = "";
        if (OnHoverStringUpdate!= null)
            OnHoverStringUpdate.Invoke("");
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
                    if (currHoverString != interactable.OnHover())
                    {
                        currHoverString = interactable.OnHover();
                    }
                    Debug.Log("Interactable works");
                }
                else
                {
                    currHoverString = "";
                }
            }
            else
            {
                currHoverString = "";
            }
        }
        if (currHoverString != savedStringState)
        {
            savedStringState = currHoverString;
            OnHoverStringUpdate.Invoke(savedStringState);
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