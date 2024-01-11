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
	public GameObject worldManager;
	public MainMenu mainmenuscr;
	void Start()
	{
		if (gameObject.scene.name != "MainMenu")
		{
			// add later ig
		}
		else
		{
			mainmenuscr = GameObject.Find("MenuManager").GetComponent<MainMenu>();
			mainmenuscr.TitleTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			mainmenuscr.TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Memory);
			mainmenuscr.TitleTheme.start();
		}
	}

	void OnDisable()
	{
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		mainmenuscr.TitleTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		mainmenuscr.TitleTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Title);
		mainmenuscr.TitleTheme.start();
	}

	void FixedUpdate()
	{
		if (gameObject.transform.position.y < bottomY)
		{
			gameObject.transform.position += new Vector3(0f, .01f, 0f);
		}
	}
}
