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
	[SerializeField] private List<string> scenesToLoad = new List<string>();
	[SerializeField] private List<string> scenesToUnload = new List<string>();
	[SerializeField] private List<string> bossConditions = new List<string>(); // impliment later

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player")
		{
			scenesToUnload.Add("TempCam");
			LoadScenes();
			UnloadScenes();
		}
	}

	private void LoadScenes()
	{
		for (int i = 0; i < scenesToLoad.Count; i++)
		{
			bool sceneloaded = false;
			for (int j = 0; j < SceneManager.sceneCount; j++)
			{
				Scene loadedScene = SceneManager.GetSceneAt(j);
				Debug.Log($"Loading {loadedScene.name}");
				if (loadedScene.name == scenesToLoad[i])
				{
					sceneloaded = true;
					break;
				}
			}
			if (!sceneloaded)
			{
				SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
			}
		}
	}

	private void UnloadScenes()
	{
		for (int i = 0; i < scenesToUnload.Count; i++)
		{
			for (int j = 0; j < SceneManager.sceneCount; j++)
			{
				Scene loadedScene = SceneManager.GetSceneAt(j);
				if (loadedScene.name == scenesToUnload[i])
				{
					SceneManager.UnloadSceneAsync(scenesToUnload[i]);
					break;
				}
			}
		}
	}
}
