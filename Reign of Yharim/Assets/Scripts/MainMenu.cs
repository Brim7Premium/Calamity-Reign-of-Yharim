using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
	public MainMenuScreens menuScreens;
	public string biomeScene;

	private GameObject settingsobj;

	public EventInstance TitleTheme;
	public void Awake()
	{
		settingsobj = GameObject.Find("[Settings]");
		if (settingsobj == null)
		{
			settingsobj = new GameObject { name = "[Settings]" };
			settingsobj.AddComponent<bracketSettingsbracket>();
			DontDestroyOnLoad(settingsobj);
		}
		TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Title);
		TitleTheme.start();
		Debug.Log("Main");
	}

	public void HoverSound()
	{
		AudioManager.instance.PlayOneShot(FMODEvents.instance.TitleHover);
	}

	void EnterWorld()
	{
		TitleTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		SceneManager.LoadScene("TempCam"); // So there isnt a missing camera for a second, also acts as a loading screen
		SceneManager.LoadSceneAsync(biomeScene, LoadSceneMode.Additive); // biome MUST be loaded before the systems
		SceneManager.LoadSceneAsync("_Systems", LoadSceneMode.Additive);
		AudioManager.instance.PlayOneShot(FMODEvents.instance._055Roar);
	}

	public void LeaveGame()
	{
		Debug.Log("The game has been quit");
		Application.Quit();
	}

	public void ClickSound()
	{
		AudioManager.instance.PlayOneShot(FMODEvents.instance.TitleClick);
	}

	[SerializeField] private Slider respawntimeSliderObj;
	public void setrespawntime()
	{
		Slider timeSlider = respawntimeSliderObj.GetComponent<Slider>();
		TextMeshProUGUI indicator = respawntimeSliderObj.GetComponent<TextMeshProUGUI>();
		indicator.text = ((int)timeSlider.value).ToString();
		bracketSettingsbracket.instance.respawnTime = (int)timeSlider.value;
	}

	[SerializeField] private Toggle militaryTimeToggle;
	public void setmilitarytime()
	{
		bracketSettingsbracket.instance.militaryTime = militaryTimeToggle.isOn;
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
		if (menuID == 6)
		{
			menuScreens = MainMenuScreens.OptionsGeneral;
		}
		if (menuID == 7)
		{
			menuScreens = MainMenuScreens.OptionsVideo;
		}
		if (menuID == 8)
		{
			menuScreens = MainMenuScreens.Credits;
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
		OptionsAudio,
		OptionsGeneral,
		OptionsVideo,
		Credits,
	}
}
