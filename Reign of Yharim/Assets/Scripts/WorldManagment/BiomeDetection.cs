using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class BiomeDetection : MonoBehaviour
{
	[SerializeField] private GameObject player;
	public string biomeName = "Forest";
	public string prevBiomeName;

	public Camera mainCam;

	public bool bossAlive;
	private bool bossWasAlive = false;

	private EventInstance biometheme;
	private EventInstance foresttheme;
	private bool day = true;
	private bool wasday = false;
	private int daythemenum = 0;

	private int count;

	private Color daybg = Color.black;
	private Color nightbg = new Color(0.11f, 0.17f, 0.28f);

	void Update()
	{
		GetTile();
		
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

	void GetTile()
	{	
		var truebiomename = Regex.Replace(biomeName, @"[\d]", string.Empty);
		var trueprevbiomename = Regex.Replace(prevBiomeName, @"[\d]", string.Empty);
		var forvol = 1f;
		//var nosunset = false;
		count = this.GetComponent<GameTime>().count;
		day = (count >= 4.5*60 && count < 19.5*60);

		if (truebiomename == "Forest" && day && !bossAlive) // dedicated forest day time system
		{	
			foresttheme.getVolume(out forvol);
			if (forvol < 1f)
			{
				foresttheme.setVolume(forvol + .01f);
			}
			biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			var newdaythemenum = daythemenum;
			var eventref = FMODEvents.instance.FullDay;
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
			if (daythemenum != newdaythemenum)
			{
				foresttheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				foresttheme = AudioManager.instance.CreateEventInstance(eventref);
				foresttheme.start();
			}
		}

		if (truebiomename != "Forest" && day && !bossAlive) // makes the forest themes not stop, and instead only mute
		{
			foresttheme.getVolume(out forvol);
			if (forvol > .01f)
			{
				foresttheme.setVolume(forvol - .01f);
			}

			else
			{
				foresttheme.setVolume(0f);
			}
		}

		if ((truebiomename != trueprevbiomename || wasday != day) && !bossAlive) //if the name of the sprite is not equal to the current tile name or it changes day
		{
			daybg = Color.black;
			nightbg = new Color(0.11f, 0.17f, 0.28f);
			wasday = day; // if it becomes night, check the biome again
			biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			prevBiomeName = biomeName; //set the current tile name to the name of the sprite
			if (truebiomename == "Astral")
			{
				// Astral Infection
				daybg = new Color(0.06666667f, 0.003921569f, 0.07450981f);
				nightbg = daybg;
//				nosunset = true;
			}
			if (truebiomename == "Desert")
			{
				// Desert
				daybg = new Color(1f, 0.9850028f, 0.8264151f);
//				nosunset = false;
			}
			if (truebiomename == "Blight")
			{
				// Blight
				daybg = new Color(0.09783139f, 0.1509434f, 0.06778213f);
//				nosunset = true;
			}
			if (truebiomename == "Bloody")
			{
				// Bloody Meteor
				daybg = new Color(0.09433961f, 0.005603133f, 0f);
//				nosunset = true;
			}
			if (truebiomename == "Ocean")
			{
				// Ocean
				daybg = new Color(0.345283f, 0.8541663f, 1f);
//				nosunset = false;
			}
			if (truebiomename == "Sulfur")
			{
				// Sulphurous Sea
				daybg = new Color(0.5457409f, 0.8962264f, 0.3179068f);
//				nosunset = true;
			}
			if (truebiomename == "Tundra")
			{
				// Tundra
				daybg = new Color(0.7415094f, 1f, 0.95700063f);
				biometheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Tundra);
				biometheme.start();
//				nosunset = false;
			}
			if (truebiomename == "Forest")
			{
				// Spawn plains/Forest
				daybg = new Color(0.701f, 0.9691256f, 1f);
				if (!day)
				{
					biometheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Night);
					biometheme.start();
				}
//				nosunset = false;
			}
			if (truebiomename == "Feral")
			{
				// Feral Swamplands
				daybg = new Color(1f, 1f, 1f); // Change this later
//				nosunset = true;
			}
			if (truebiomename == "Jungle")
			{
				// Jungle
				daybg = new Color(0.5254902f, 1f, 0.8364275f);
//				nosunset = false;
			}
			if (truebiomename == "Planetoids")
			{
				// Planetoids
				daybg = new Color(0.701f, 0.9691256f, 1f);
//				nosunset = true;
			}
			if (truebiomename == "Underworld")
			{
				// Underworld
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
//				nosunset = true;
			}
			if (truebiomename == "Space")
			{
				// Space
//				nosunset = true;
			}
			if (truebiomename == "Crags")
			{
				// Brimstone Crags/Azafure
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
//				nosunset = true;
			}
			if (truebiomename == "Abyss1")
			{
				// Sulphuric depths
//				nosunset = true;
			}
			if (truebiomename == "SunkenSea")
			{
				// Sunken Sea
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
//				nosunset = true;
			}
			if (truebiomename == "Obsidian")
			{
				// Obsidian Cliffs
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
//				nosunset = true;
			}
			if (truebiomename == "Garden")
			{
				// Profaned Garden
				daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				nightbg = daybg;
//				nosunset = true;
			}
		}
		
		if (!day)
		{
			mainCam.backgroundColor = nightbg;
		}
		

		if (count > 1000 && day)
		{
			mainCam.backgroundColor = Color.Lerp(daybg, nightbg, (float)((count-1000)/(19.5*60-1000)));
		}

		if (count < 472 && day)
		{
			mainCam.backgroundColor = Color.Lerp(nightbg, daybg, (float)((count-4.5*60)/(472-4.5*60)));
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