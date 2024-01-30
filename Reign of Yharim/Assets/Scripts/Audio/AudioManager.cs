using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
	private List<EventInstance> eventInstances;
	public static AudioManager instance { get; private set; }

	[Header("Volume")]
	[Range(0, 1)]
	public float musicVolume = 1; // the volume of music
	[Range(0, 1)]
	public float SFXVolume = 1; // the volume of ambience and sfx

	private Bus musicBus; // the music group set in the mixer
	private Bus SFXBus; // the sfx group set in the mixer

	private GameObject settingsobj;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Found more than one Audio Manager in the scene");
		}
		instance = this;

		settingsobj = GameObject.Find("[Settings]");
		if (settingsobj == null)
		{
			settingsobj = new GameObject { name = "[Settings]" };
			settingsobj.AddComponent<bracketSettingsbracket>();
			DontDestroyOnLoad(settingsobj);
		}

		eventInstances = new List<EventInstance>();

		musicBus = RuntimeManager.GetBus("bus:/Music");
		SFXBus = RuntimeManager.GetBus("bus:/SFX");

		musicVolume = bracketSettingsbracket.instance.musicVolume;
		SFXVolume = bracketSettingsbracket.instance.SFXVolume;
	}

	public GameObject musSlider, sfxSlider;
	public void SliderValueChange(bool sfx)
	{
		if (musSlider == null)
		{
			musSlider = GameObject.Find("MusicSlider");
			sfxSlider = GameObject.Find("SFXSlider");
		}

		else if (!sfx)
		{
			var volumeSlider = sfxSlider.GetComponent<Slider>();
			bracketSettingsbracket.instance.musicVolume = volumeSlider.value;
			musicVolume = volumeSlider.value;
			var indicator = volumeSlider.GetComponent<TextMeshProUGUI>();
			indicator.text = ((int)(volumeSlider.value * 100)).ToString();
		}

		else
		{
			var volumeSlider = musSlider.GetComponent<Slider>();
			bracketSettingsbracket.instance.SFXVolume = volumeSlider.value;
			SFXVolume = volumeSlider.value;
			var indicator = volumeSlider.GetComponent<TextMeshProUGUI>();
			indicator.text = ((int)(volumeSlider.value * 100)).ToString();
		}
	}

	private void Update()
	{
		musicVolume = bracketSettingsbracket.instance.musicVolume;
		SFXVolume = bracketSettingsbracket.instance.SFXVolume;
		musicBus.setVolume(musicVolume);
		SFXBus.setVolume(SFXVolume);
	}

	public void PlayOneShot(EventReference sound, Vector3 worldPos = new Vector3())
	{
		RuntimeManager.PlayOneShot(sound, worldPos);
	}

	public EventInstance CreateEventInstance(EventReference eventReference)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		eventInstances.Add(eventInstance);

		return eventInstance;
	}

	private void CleanUp()
	{
		foreach (EventInstance eventInstance in eventInstances)
		{
			eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			eventInstance.release();
		}
	}

	private void OnDestroy()
	{
		CleanUp();
	}
}
