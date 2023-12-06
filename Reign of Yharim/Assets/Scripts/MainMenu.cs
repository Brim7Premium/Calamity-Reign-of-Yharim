using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
	public MainMenuScreens menuScreens;

	private EventInstance TitleTheme;
	public void Awake()
	{
		TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Title);
		TitleTheme.start();
		Debug.Log("Main");
	}

	public void HoverSound()
	{
		AudioManager.instance.PlayOneShot(FMODEvents.instance.AureusStomp);
	}

	public void EnterWorld()
	{
		AudioManager.instance.PlayOneShot(FMODEvents.instance._055Roar);
		SceneManager.LoadScene("Surface");
	}
	public void LeaveGame()
	{
		Debug.Log("The game has been quit");
		Application.Quit();
	}

	public void ClickSound()
	{
		AudioManager.instance.PlayOneShot(FMODEvents.instance.ExoTwinsHoverIcon);
	}

	public void ChangeMenuScreen(float menuID)
	{
		ClickSound();
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
