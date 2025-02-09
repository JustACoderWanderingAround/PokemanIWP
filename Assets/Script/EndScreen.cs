using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    GameObject m_screen;
    [SerializeField]
    TMP_Text m_EndText;
    private void Start()
    {
        m_screen.SetActive(false);
    }
    public void EnableScreen(bool isWin)
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if (isWin)
        {
            m_EndText.text = "YOU WIN!";
        }
        else
        {
            m_EndText.text = "You LOSE!";
        }
    }
}
