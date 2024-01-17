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

	void OnTriggerEnter2D(Collider2D collision)
	{
		UnloadScene("TempCam");
		if (collision.gameObject.name == "Player" && collision.gameObject.GetComponent<PlayerAI>().Plundered.Contains(bossCondition))
		{
			LoadScenes();
			UnloadScenes();
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
