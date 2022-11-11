using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Canvas StartCanvas;
    public Canvas RestartCanvas;
    public PlayGrid state;
    public void RestartMenu()
    {
        SceneManager.LoadScene("SampleScene");
        state.gameover = false;
        DisableRestart();
    }

    public void StartMenu()
    {
        state.gameover = false;
        DisableStart();
    }
    public void DisableRestart()
    {
        RestartCanvas.GetComponent<Canvas>().enabled = false;
    }
    public void DisableStart()
    {
        StartCanvas.GetComponent<Canvas>().enabled = false;
    }
    public void EnableRestart()
    {
        RestartCanvas.GetComponent<Canvas>().enabled = true;
    }
    public void EnableStart()
    {
        StartCanvas.GetComponent<Canvas>().enabled = true;
    }
}
