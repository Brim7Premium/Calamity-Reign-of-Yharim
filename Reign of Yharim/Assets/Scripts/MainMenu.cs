using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public MainMenuScreens menuScreens;

    public void EnterWorld()
    {
        SceneManager.LoadScene("Surface");
    }
    public void LeaveGame()
    {
        Debug.Log("The game has quit");
        Application.Quit();
    }
    private void Update()
    {
    }
   
    public void ChangeMenuScreen(float menuID)
    {
        if (menuID == 1)
        {
            menuScreens = MainMenuScreens.Main;
        }
        if (menuID == 2)
        {
            menuScreens = MainMenuScreens.SinglePlayer;
        }
        if (menuID == 3)
        {
            menuScreens = MainMenuScreens.SinglePlayerSave;
        }
        if (menuID == 4)
        {
            menuScreens = MainMenuScreens.Options;
        }
        if (menuID == 5)
        {
            menuScreens = MainMenuScreens.OptionsAudio;
        }
        Debug.Log(menuScreens);
    }
    /* Set the variables using unity's built in button system
     * for each button, assign the id of the next screen/the screen that the button would send you to
     * for back buttons, assign the id of the previous screen
     * these variables are going to be how we control the mechanic of each screen
     */

    public enum MainMenuScreens
    {
        Main,
        SinglePlayer,
        SinglePlayerSave,
        Options,
        OptionsAudio
    }
}
