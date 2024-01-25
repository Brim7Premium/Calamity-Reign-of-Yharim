using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class TextWall : MonoBehaviour
{
	public string credits = @"
	THE REIGN OF YHARIM DEVELOPMENT TEAM

	Brimston Premium
	Director and Creator of Reign of Yharim, Main musician, Game Designer, Programmer, Spriter, and Lore Writer.

	Jello Beanz
	Assistant Director and Lead concept artist, Spriter, Game Designer, and Programmer

	Axolote
	Lead spriter and concept artist

	CiggyPlant
	Concept Artist

	AstraGel
	Musician and Former spriter

	Lemi Saavedra
	Lore Writer

	115101120
	Programmer

	Destructogabe8
	Editor

	Acsazf Play
	Programmer

	Guard Boi
	Programmer

	Abendrot
	Concept artist

	Polusary
	Concept artist and Game Designer

	Salandit
	Concept Artist

	Trester
	Musician

	MAIN CONTRIBUTORS

	The Calamity Mod Development team
	All Calamity Mod assets and concepts

	DM DOKURO
	Music

	Heart Plus Up!
	Music

	OTHER CONTRIBUTORS

	Fazin
	Former Programmer

	Dragoncraft9
	Former Concept Artist
	";

	public float bottomY; // when it'll stop scrolling
	public GameObject mainmenu; // the component must be got every time you want to use it, else the music breaks
	void OnEnable()
	{
		gameObject.GetComponent<TextMeshPro>().text = credits;
		mainmenu.GetComponent<MainMenu>().TitleTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // stop toacw
		mainmenu.GetComponent<MainMenu>().TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Credits); // change it to moacw
		mainmenu.GetComponent<MainMenu>().TitleTheme.start(); // start moacw
	}

	void OnDisable()
	{
		gameObject.transform.position = new Vector3(0f, 0f, 0f); // reset position
		mainmenu.GetComponent<MainMenu>().TitleTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // stop moacw
		mainmenu.GetComponent<MainMenu>().TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Title); // change it to toacw
		mainmenu.GetComponent<MainMenu>().TitleTheme.start(); // start toacw
	}

	void FixedUpdate()
	{
		if (gameObject.transform.position.y < bottomY) // if its not at the bottom of the credits...
		{
			gameObject.transform.position += new Vector3(0f, .005f, 0f); // ...scroll down by moving the object upwards
		}
		else
		{
			GameObject.Find("/Canvas/CreditsMenu/BackButton").GetComponent<Button>().onClick.Invoke(); // go back to the main menu using the back button (jumpscare warning)
		}
	}
}
