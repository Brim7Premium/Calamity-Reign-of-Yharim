using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class InvasionScene : MonoBehaviour
{
	public GameObject worldManager;
	public GameObject player;
	public LoadingTriggers loadingTrigger;
	public Color daybg = new Color(0.701f, 0.9691256f, 1f), nightbg = new Color(0.11f, 0.17f, 0.28f);
	public bool showSun = true;
	public bool timed;
	public float totalProgress = 100f;
	public float currentProgress = 0f;
	public GameObject eventBoss; // leave blank to not spawn anything
	public EnemySpawner enemySpawner;
	public bool useBiomeTheme = false;
	public EventReference eventTheme;
	private EventInstance eventMusic;

	void Update()
	{
		if (worldManager == null)
		{
			worldManager = GameObject.Find("WorldManager");
			enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
			player = GameObject.Find("Player");
			if (!useBiomeTheme)
			{
				eventMusic = AudioManager.instance.CreateEventInstance(eventTheme);
				eventMusic.start();
			}
		}

		else
		{
			worldManager.GetComponent<BiomeManager>().eventUsesBiome = useBiomeTheme;
			worldManager.GetComponent<BiomeManager>().daybg = daybg;
			worldManager.GetComponent<BiomeManager>().nightbg = nightbg;
			worldManager.GetComponent<BiomeManager>().nosunlight = showSun;
			loadingTrigger = worldManager.GetComponent<Invasions>().loadingTrigger;

			if (timed)
			{
				StartCoroutine(EventTimer());
			}

			if (currentProgress >= totalProgress)
			{
				StartCoroutine(enemySpawner.SpawnEnemy(eventBoss));

				if (!player.GetComponent<PlayerAI>().Plundered.Contains(gameObject.scene.name.Trim('_')))
				{
					player.GetComponent<PlayerAI>().Plundered.Add(gameObject.scene.name.Trim('_'));
					StopThisEvent();
				}
			}
		}
	}
	
	IEnumerator EventTimer()
	{
		yield return new WaitForSeconds(1);
		currentProgress += 1f;
	}

	public void StopThisEvent()
	{
		GameObject.Destroy(gameObject);
	}

	void OnDestroy()
	{
		if (!useBiomeTheme)
		{
			eventMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		worldManager.GetComponent<BiomeManager>().eventActive = false;
		loadingTrigger.UnloadScene(gameObject.scene.name);
	}
}
