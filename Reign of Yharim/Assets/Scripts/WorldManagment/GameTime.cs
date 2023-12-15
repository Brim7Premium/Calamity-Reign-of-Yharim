using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class GameTime : MonoBehaviour
{
	public int count;
	public static string displayTime;

	[SerializeField] private Transform orbitPoint;
	[SerializeField] private Vector3 rotation;

	private GameObject settingsobj;

	IEnumerator Start()
	{
		
		settingsobj = GameObject.Find("[Settings]");
		if (settingsobj == null)
		{
			settingsobj = new GameObject { name = "[Settings]" };
			settingsobj.AddComponent<bracketSettingsbracket>();
			DontDestroyOnLoad(settingsobj);
		}
		var miltime = settingsobj.GetComponent<bracketSettingsbracket>().militaryTime;
		while (true)
		{
			if(count == 24*60){count = 0;}

			if (miltime)
			{
				displayTime = $"{count/60:d2} : {count%60:d2}";
			}
			else
			{
				var min = $"{count%60:d2}";
				var hour = count/60;
				var am = true;
				if (hour == 12)
				{
					am = false;
				}
				else if (hour > 12)
				{
					hour -= 12;
					am = false;
				}
				else if (hour == 0)
				{
					hour = 12;
				}
				var tim = $"{hour:d2} : {min} ";
				if (am)
				{
					tim += "AM";
				}
				else
				{
					tim += "PM";
				}
				displayTime = tim;
			}

			yield return new WaitForSeconds(0);
			count++;
		}
	}

	void Update()
	{ 
		orbitPoint.rotation = Quaternion.Euler(0, 0, count/-4f);
		try{orbitPoint.position = GameObject.Find("Player").transform.position;
		orbitPoint.position -= new Vector3(0, 20, 0);}catch{}
	}
}
