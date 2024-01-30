using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class GameTime : MonoBehaviour
{
	public static int count = 270;
	public static string displayTime;

	public Transform orbitPoint;
	[SerializeField] private Vector3 rotation;
	public GameObject player;
	private GameObject detection;

	IEnumerator Start()
	{
		var miltime = bracketSettingsbracket.instance.militaryTime;
		while (true)
		{
			if (count >= 24*60)
			{
				count = 0;
			}

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

			yield return new WaitForSeconds(1);
			count++;
		}
	}

	void FixedUpdate()
	{
		if (orbitPoint.gameObject.activeSelf)
			orbitPoint.rotation = Quaternion.Euler(0, 0, count/-4f);
	}
}
