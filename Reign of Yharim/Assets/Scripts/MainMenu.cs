using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text textToSet;
    public TMP_InputField textToUse;
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
        if (textToUse != null)
        {
            textToSet.text = textToUse.text;
        }
        Debug.Log(menuScreens);
    }
    public void ChangeMenuScreen(float menuID)
    {
        if (menuID == 1)
        {
            menuScreens = MainMenuScreens.Main;
        }
        if (menuID == 2)
        {
            menuScreens = MainMenuScreens.SinglePlay;
        }
        if (menuID == 3)
        {
            menuScreens = MainMenuScreens.SinglePlaySave;
        }
        if (menuID == 4)
        {
            menuScreens = MainMenuScreens.Options;
        }
        if (menuID == 5)
        {
            menuScreens = MainMenuScreens.OptionsAudio;
        }
    }

    public enum MainMenuScreens
    {
        Main,
        SinglePlay,
        SinglePlaySave,
        Options,
        OptionsAudio
    }
}
