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

public class BiomeManager : MonoBehaviour
{
	[SerializeField] private GameObject player;
	public string biomeName = "Forest";
	public string prevBiomeName;

	public Camera mainCam;

	public bool bossAlive;
	private bool bossWasAlive = false;

	public EventInstance biometheme;
	public EventInstance foresttheme;
	private bool day = true;
	private bool wasday = false;
	private int daythemenum = 0;
	private bool nosunlight;

	public Light2D SunLight;

	private int count;

	private Color daybg = Color.black;
	private Color nightbg = new Color(0.11f, 0.17f, 0.28f);

	void Update()
	{
		int index = biomeName.IndexOf("_");
		if (index >= 0)
		{
  			biomeName = biomeName.Substring(0, index);
		}
		
		index = prevBiomeName.IndexOf("_");
		if (index >= 0)
		{
  			prevBiomeName = prevBiomeName.Substring(0, index);
		}

		ManageBiome();
		
		if (bossAlive)
		{
			biometheme.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			foresttheme.setVolume(0f);
		}

		if (bossAlive != bossWasAlive)
		{
			bossWasAlive = bossAlive;

			if (!bossAlive)
			{
				wasday = !day;
			}
		}
	}

	void ManageBiome()
	{
		var forvol = 1f; // this is the volume of the forest theme
		count = gameObject.GetComponent<GameTime>().count; // get the time of the world
		day = (count >= 4.5*60 && count < 19.5*60); // calculates if it's day or night

		if (biomeName == "Forest" && day && !bossAlive) // dedicated forest day time system
		{	
			foresttheme.getVolume(out forvol); // get forest volume
			if (forvol < 1f) // if its not at full volume...
			{
				foresttheme.setVolume(forvol + .01f); // ...increase it slowly
			}
			biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // stop the normal biome music so the forest music can play
			var newdaythemenum = daythemenum;
			var eventref = FMODEvents.instance.FullDay; // the eventref to use
			if (count >= 4.5*60 && count < 7.5*60)
			{
				daythemenum = 1;
				eventref = FMODEvents.instance.Day1;
			}
			else if (count >= 7.5*60 && count < 12*60)
			{
				daythemenum = 2;
				eventref = FMODEvents.instance.Day2;
			}
			else if (count >= 12*60 && count < 16.5*60)
			{
				daythemenum = 3;
				eventref = FMODEvents.instance.Day3;
			}
			else
			{
				daythemenum = 4;
				eventref = FMODEvents.instance.Day4;
			}
			if (daythemenum != newdaythemenum) // if the previous day theme isnt the same
			{
				foresttheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // stop the day theme
				foresttheme = AudioManager.instance.CreateEventInstance(eventref); // change to the next one
				foresttheme.start(); // start the new one
			}
		}

		if ((biomeName != "Forest" || !day) && !bossAlive) // makes the forest themes not stop, and instead only mute
		{
			foresttheme.getVolume(out forvol); // get the volume
			if (forvol >= .01f) // if its not almost muted...
			{
				foresttheme.setVolume(forvol - .01f); // ...decrease the volume slowly
			}

			else
			{
				foresttheme.setVolume(0f); // if it is almost muted, then just set it to 0; this avoids a previous error caused by it going below zero volume
			}
		}

		if ((biomeName != prevBiomeName || wasday != day) && !bossAlive) // if it changes day or you leave the biome
		{
			daybg = Color.black; // if the day sky colour isnt set, its just black
			nightbg = new Color(0.11f, 0.17f, 0.28f); // if the day sky colour isnt set, its just a really dark blue
			wasday = day; // if it becomes night, check the biome again
			biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // stop the biome theme, the forest theme is handled seperately
			prevBiomeName = biomeName;
			if (biomeName == "Astral") // only going to comment on three of these
			{
				// Astral Infection
				daybg = new Color(0.06666667f, 0.003921569f, 0.07450981f); // sets the day background to almost black
				nightbg = daybg; // the sky is always dark here
				nosunlight = false; // the sun should be visible here, even if it doesnt do much to the sky
			}
			if (biomeName == "Desert")
			{
				// Desert
				daybg = new Color(1f, 0.9850028f, 0.8264151f);
				nosunlight = false;
			}
			if (biomeName == "Blight")
			{
				// Blight
				daybg = new Color(0.09783139f, 0.1509434f, 0.06778213f);
				nosunlight = false;
			}
			if (biomeName == "Bloody")
			{
				// Bloody Meteor
				daybg = new Color(0.09433961f, 0.005603133f, 0f);
				nosunlight = false;
			}
			if (biomeName == "Ocean")
			{
				// Ocean
				daybg = new Color(0.345283f, 0.8541663f, 1f);
				nosunlight = false;
			}
			if (biomeName == "Sulfur")
			{
				// Sulphurous Sea
				daybg = new Color(0.5457409f, 0.8962264f, 0.3179068f);
				nosunlight = false;
			}
			if (biomeName == "Tundra")
			{
				// Tundra
				daybg = new Color(0.7415094f, 1f, 0.95700063f); // day colour is the same as the forest
				biometheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Tundra); // sets the biome theme to the tundra theme
				biometheme.start(); // start the tundra theme
				nosunlight = false; // the sun is visible here
			}
			if (biomeName == "Forest")
			{
				// Spawn plains/Forest
				daybg = new Color(0.701f, 0.9691256f, 1f);
				if (!day)
				{
					biometheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Night);
					biometheme.start();
				}
				nosunlight = false;
			}
			if (biomeName == "Feral")
			{
				// Feral Swamplands
				daybg = new Color(1f, 1f, 1f); // Change this later
				nosunlight = false;
			}
			if (biomeName == "Jungle")
			{
				// Jungle
				daybg = new Color(0.5254902f, 1f, 0.8364275f);
				nosunlight = false;
			}
			if (biomeName == "Planetoids")
			{
				// Planetoids/Sky (different from space)
				daybg = new Color(0.701f, 0.9691256f, 1f);
				nosunlight = false;
			}
			if (biomeName == "Underworld")
			{
				// Underworld
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
				nosunlight = true;
				SunLight.intensity = 0.5f;
			}
			if (biomeName == "Space")
			{
				// Space
				nightbg = daybg; // makes both night and day black
				nosunlight = true; // theres no sun once youre in space, but there is in the planetoids layer
				SunLight.intensity = 0.5f; // since theres no sun, the lighting must be changed manually
			}
			if (biomeName == "Crags")
			{
				// Brimstone Crags/Azafure
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
				nosunlight = true;
				SunLight.intensity = 0.5f;
			}
			if (biomeName == "Abyss1")
			{
				// Sulphuric depths
				nosunlight = true;
				SunLight.intensity = 0.5f;
			}
			if (biomeName == "SunkenSea")
			{
				// Sunken Sea
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
				nosunlight = true;
				SunLight.intensity = 0.5f;
			}
			if (biomeName == "Obsidian")
			{
				// Obsidian Cliffs
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nosunlight = true;
				SunLight.intensity = 0.5f;
			}
			if (biomeName == "Garden")
			{
				// Profaned Garden
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
				nosunlight = true;
				SunLight.intensity = 0.5f;
			}
		}

		gameObject.GetComponent<GameTime>().orbitPoint.gameObject.SetActive(!nosunlight); // makes the sun (and moon) disappear if nosunlight is true 
		
		if (!day && !nosunlight)
		{
			mainCam.backgroundColor = nightbg;
			SunLight.intensity = 0.1f;
		}

		if (count >= 1000 && day && !nosunlight)
		{
			mainCam.backgroundColor = Color.Lerp(daybg, nightbg, ((count-1000f)/(19.5f*60f-1000f)));
			SunLight.intensity = 1.1f - ((count-1000f)/(19.5f*60f-1000f));
		}

		if (count < 472 && day && !nosunlight)
		{
			mainCam.backgroundColor = Color.Lerp(nightbg, daybg, ((count-4.5f*60f)/(472f-4.5f*60f)));
			SunLight.intensity = ((count-4.5f*60f)/(472f-4.5f*60f)) + 0.1f;
		}

		if (count < 1000 && count > 472 && !nosunlight)
		{
			SunLight.intensity = 1f + 0.05f;
		}

		/*
		if (!nosunset && day)
		{
			var sunsetcolour = new Color(0.84f, 0.3f, 0.36f);

			// formula is (count-start)/(end-start)

			if (count >= 270 && count < 472)
			{
				mainCam.backgroundColor = Color.Lerp(nightbg, sunsetcolour, (float)((count-270)/(472-270)));
			}

			else if (count >= 472 && count < 522)
			{
				mainCam.backgroundColor = sunsetcolour;
			}

			else if (count >= 522 && count < 600)
			{
				mainCam.backgroundColor = Color.Lerp(sunsetcolour, daybg, (float)((count-522)/(600-522)));
			}

			else if (count >= 848 && count < 888)
			{
				mainCam.backgroundColor = Color.Lerp(daybg, sunsetcolour, (float)((count-848)/(888-848)));
			}
			
			else if (count >= 888 && count < 976)
			{
				mainCam.backgroundColor = sunsetcolour;
			}
			
			else if (count >= 976 && count < 1170)
			{
				mainCam.backgroundColor = Color.Lerp(sunsetcolour, nightbg, (float)((count-976)/(1170-976)));
			}

			else
			{
				mainCam.backgroundColor = daybg;
			}
		}

		if (nosunset && day)
		{
			if (count < 472 && day)
			{
				mainCam.backgroundColor = Color.Lerp(nightbg, daybg, (float)((count-270)/(472-270)));
			}

			else if (count >= 1000 && day)
			{
				mainCam.backgroundColor = Color.Lerp(daybg, nightbg, (float)((count-1000)/(1170-1000)));
			}

			else
			{
				mainCam.backgroundColor = daybg;
			}
		}
		
		/*
		Notes

		Fade into sunrise at 270
		Start sunrise at 472, end at 552
		Fade into day (600)

		Fade into sunset at 848
		Start sunset at 888, end at 976
		Fade into night (1170)

		If nosunset

		Fade into "sunrise" (472) at day (270)
		Fade into night (1170) at "sunset" (1000)
		*/
	}
}