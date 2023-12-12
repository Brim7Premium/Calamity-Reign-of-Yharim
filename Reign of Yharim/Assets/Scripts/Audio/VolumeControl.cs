using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeControl : MonoBehaviour
{
	private enum VolumeType 
	{
		MUSIC,
		SFX
	}

	[Header("Type")]
	[SerializeField] private VolumeType volumeType;

	private Slider volumeSlider;
	private GameObject settingsobj;

	private void Awake()
	{
		settingsobj = GameObject.Find("[Settings]");
		if (settingsobj == null)
		{
			settingsobj = new GameObject { name = "[Settings]" };
			settingsobj.AddComponent<bracketSettingsbracket>();
			DontDestroyOnLoad(settingsobj);
		}
		volumeSlider = this.GetComponentInChildren<Slider>();
	}

	public void SliderValueChange()
	{
		switch (volumeType)
		{
			case VolumeType.MUSIC:
				AudioManager.instance.musicVolume = volumeSlider.value;
				settingsobj.GetComponent<bracketSettingsbracket>().musicVolume = volumeSlider.value;
				break;
			case VolumeType.SFX:
				AudioManager.instance.SFXVolume = volumeSlider.value;
				settingsobj.GetComponent<bracketSettingsbracket>().SFXVolume = volumeSlider.value;
				break;
		}
		var indicator = volumeSlider.GetComponent<TextMeshProUGUI>();
		indicator.text = ((int)(volumeSlider.value * 100)).ToString();
	}
}
