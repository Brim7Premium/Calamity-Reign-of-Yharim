using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void EnterWorld()
    {
        SceneManager.LoadScene("Surface");
    }
    public void LeaveGame()
    {
        Debug.Log("The game has quit");
        Application.Quit();
    }
}
