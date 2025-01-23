using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTeleporter : MonoBehaviour
{
    GameObject endScreen;
    InteractableSceneSwitcher interactableSceneSwitcher;

    private void Start()
    {
        interactableSceneSwitcher = GetComponent<InteractableSceneSwitcher>();
    }
    public void ActivateTeleporter()
    {
        interactableSceneSwitcher.isInteractable = true;
    }
}
