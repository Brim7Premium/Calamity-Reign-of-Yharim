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

	IEnumerator Start()
	{
		while (true)
		{
			if(count == 24*60){count = 0;}

			displayTime = $"{count/60:d2} : {count%60:d2}";

			yield return new WaitForSeconds(1);
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
