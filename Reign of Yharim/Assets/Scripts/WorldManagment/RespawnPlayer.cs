using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class RespawnPlayer : MonoBehaviour
{
	[SerializeField] private GameObject player, timerobj;

	private PlayerAI playerAI;

	private void Start()
	{
		playerAI = player.GetComponent<PlayerAI>();
		timerobj.SetActive(false);
	}

	IEnumerator Respawn()
	{
		var settingsobj = GameObject.Find("[Settings]");
		if (settingsobj == null)
		{
			settingsobj = new GameObject { name = "[Settings]" };
			settingsobj.AddComponent<bracketSettingsbracket>();
			DontDestroyOnLoad(settingsobj);
		}
		var respawnSeconds = bracketSettingsbracket.instance.respawnTime;
		var seconds = respawnSeconds;
		timerobj.SetActive(true);
		while (0 < seconds)
		{
			timerobj.GetComponent<TextMeshProUGUI>().text = $"{seconds}";
			yield return new WaitForSeconds(1);
			seconds -= 1;
		}
		seconds = respawnSeconds;
		var loadingtriggerssobj = GameObject.Find("LoadingTrigger");
		timerobj.SetActive(false);
		if (loadingtriggerssobj != null)
		{
			var loadingtriggerss = loadingtriggerssobj.GetComponent<LoadingTriggers>();
			for (int j = 0; j < SceneManager.sceneCount; j++)
			{
				Scene loadedScene = SceneManager.GetSceneAt(j);
				if (loadedScene.name.StartsWith(BiomeManager.instance.biomeName))
				{
					loadingtriggerss.UnloadScene(BiomeManager.instance.biomeName);
				}
			}
			loadingtriggerss.LoadScene("Forest_Spawn");
			player.SetActive(true);
			playerAI.SetDefaults();
			playerAI.StartCoroutine(playerAI.Immunity());
			player.transform.position = new Vector3(0f, 0f, 0.35f);
		}
		else
		{
			player.SetActive(true);
			playerAI.SetDefaults();
			playerAI.StartCoroutine(playerAI.Immunity());
		}
	}
}
