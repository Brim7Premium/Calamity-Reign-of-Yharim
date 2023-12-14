using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bracketSettingsbracket : MonoBehaviour
{
	[Header("General")]
	[Range(5, 45)]
	public int respawnTime = 5;

	[Header("Video")]
	public bool militaryTime = false;

	[Header("Audio")]
	[Range(0, 1)]
	public float musicVolume = 1;
	[Range(0, 1)]
	public float SFXVolume = 1;
}
