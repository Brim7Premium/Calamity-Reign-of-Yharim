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
	public float bottomY;
	public GameObject mainmenu;
	void OnEnable()
	{
		mainmenu.GetComponent<MainMenu>().TitleTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		mainmenu.GetComponent<MainMenu>().TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Memory);
		mainmenu.GetComponent<MainMenu>().TitleTheme.start();
	}

	void OnDisable()
	{
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		mainmenu.GetComponent<MainMenu>().TitleTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		mainmenu.GetComponent<MainMenu>().TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Title);
		mainmenu.GetComponent<MainMenu>().TitleTheme.start();
	}

	void FixedUpdate()
	{
		if (gameObject.transform.position.y < bottomY)
		{
			gameObject.transform.position += new Vector3(0f, .005f, 0f);
		}
		else
		{
			gameObject.transform.parent.gameObject.SetActive(false);
		}
	}
}
