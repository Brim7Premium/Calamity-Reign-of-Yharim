// called this not too confuse it with other types of events
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

public class Invasions : MonoBehaviour
{
	public LoadingTriggers loadingTrigger;
	
	public EventInstance eventTheme;

	public void StartEvent(string EventName)
	{
		if (!BiomeManager.instance.eventActive && !BiomeManager.instance.bossAlive)
		{
			loadingTrigger.LoadScene("_" + EventName);
		}
	}

	void Update()
	{
		if (BiomeManager.instance.bossAlive)
		{
			StopEvent();
		}
	}

	public void StopEvent()
	{
		if (BiomeManager.instance.eventActive)
		{
			GameObject invasionobj = GameObject.Find("Invasion");
			if (invasionobj != null)
			{
				invasionobj.GetComponent<InvasionScene>().StopThisEvent();
			}
		}
	}


}