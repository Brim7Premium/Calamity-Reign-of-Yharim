using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
	private List<EventInstance> eventInstances;
	public static AudioManager instance { get; private set; }

	[Header("Volume")]
	[Range(0, 1)]
	public float musicVolume = 1;
	[Range(0, 1)]
	public float SFXVolume = 1;

	private Bus musicBus;
	private Bus SFXBus;

	private GameObject settingsobj;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Found more than one Audio Manager in the scene");
		}
		instance = this;

		eventInstances = new List<EventInstance>();

		musicBus = RuntimeManager.GetBus("bus:/Music");
		SFXBus = RuntimeManager.GetBus("bus:/SFX");

		settingsobj = GameObject.Find("[Settings]");
		if (settingsobj != null) 
		{
			musicVolume = settingsobj.GetComponent<bracketSettingsbracket>().musicVolume;
			SFXVolume = settingsobj.GetComponent<bracketSettingsbracket>().SFXVolume;
		}
	}

	private void Update()
	{
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
