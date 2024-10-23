using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static void SwapScene(string nextScene) 
    {
        SceneManager.LoadScene(nextScene);
    }
}
