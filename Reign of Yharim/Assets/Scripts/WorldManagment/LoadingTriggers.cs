using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class LoadingTriggers : MonoBehaviour
{
	public List<string> scenesToLoad = new List<string>(), scenesToUnload = new List<string>();
	public string bossCondition;
	public Vector2 playerPosition;
	public bool loadsEvent = false;
	public GameObject worldManager;
	public GameObject player;
	public bool usesY;
	public float leftX, rightX;

	void Update()
	{
		if (worldManager == null)
		{
			worldManager = GameObject.Find("WorldManager");
			player = GameObject.Find("Player");
		}
		else if (!usesY && player.transform.position.x > gameObject.transform.position.x-10f && player.transform.position.x < gameObject.transform.position.x+10f)
		{
			UnloadScene("TempCam");
			if (string.IsNullOrEmpty(bossCondition) || player.GetComponent<PlayerAI>().Plundered.Contains(bossCondition))
			{
				if (!loadsEvent)
				{
					if (playerPosition != new Vector2(0f, 0f) && player != null)
					{
						player.transform.position = new Vector3(playerPosition.x, playerPosition.y, 0.35f);
					}
					LoadScenes();
					UnloadScenes();
				}
				else if (worldManager != null)
				{
					worldManager.GetComponent<Invasions>().StartEvent(scenesToLoad[0]);
				}
			}
		}
		else if (usesY && player.transform.position.x > leftX && player.transform.position.x < rightX && player.transform.position.y > gameObject.transform.position.y-10f && player.transform.position.y < gameObject.transform.position.y+10f)
		{
			UnloadScene("TempCam");
			if (string.IsNullOrEmpty(bossCondition) || player.GetComponent<PlayerAI>().Plundered.Contains(bossCondition))
			{
				if (!loadsEvent)
				{
					if (playerPosition != new Vector2(0f, 0f) && player != null)
					{
						player.transform.position = new Vector3(playerPosition.x, playerPosition.y, 0.35f);
					}
					LoadScenes();
					UnloadScenes();
				}
				else if (worldManager != null)
				{
					worldManager.GetComponent<Invasions>().StartEvent(scenesToLoad[0]);
				}
			}
		}
	}

	public void LoadScenes()
	{
		for (int i = 0; i < scenesToLoad.Count; i++)
		{
			LoadScene(scenesToLoad[i]);
		}
	}

	public void LoadScene(string scenetoload)
	{
		Debug.Log($"Loading {scenetoload}");
		bool sceneloaded = false;
		for (int j = 0; j < SceneManager.sceneCount; j++)
		{
			Scene loadedScene = SceneManager.GetSceneAt(j);
			if (loadedScene.name == scenetoload)
			{
				sceneloaded = true;
				break;
			}
		}
		if (!sceneloaded)
		{
			SceneManager.LoadSceneAsync(scenetoload, LoadSceneMode.Additive);
		}
	}

	public void UnloadScenes()
	{
		for (int i = 0; i < scenesToUnload.Count; i++)
		{
			UnloadScene(scenesToUnload[i]);
		}
	}

	public void UnloadScene(string scenetounload)
	{
		Debug.Log($"Unloading {scenetounload}");
		for (int j = 0; j < SceneManager.sceneCount; j++)
		{
			Scene loadedScene = SceneManager.GetSceneAt(j);
			if (loadedScene.name == scenetounload)
			{
				SceneManager.UnloadSceneAsync(scenetounload);
				break;
			}
		}
	}
}
