using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSceneSwitcher : MonoBehaviour, IInteractable
{
    public bool isInteractable;
    public string interactableText;
    public string nonInteractableText;
    public string nextSceneName;
    public string OnHover()
    {
        return isInteractable ? interactableText : nonInteractableText;
    }

    public void OnInteract()
    {
        if (isInteractable)
            SceneSwitcher.SwapScene(nextSceneName);
    }
}
