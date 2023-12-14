using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public class BiomeDetection : MonoBehaviour
{
	public Tilemap tiles;
	public Vector3Int tileAtPlayer;
	public Sprite tileSprite;
	private string tileSpriteName;
	public string currentTileName;

	public Camera mainCam;

	private EventInstance biometheme;
	private bool day = true;
	private bool wasday = false;
	private int daythemenum = 0;

	private int count;

	private Color daybg = Color.black;
	private Color nightbg = new Color(0.11f, 0.17f, 0.28f);

	void Update()
	{
		GetTile();
	}

	void GetTile()
	{
		var stopit = true;
		count = GameObject.Find("WorldManager").GetComponent<GameTime>().count;
		day = (count >= 4.5*60 && count < 19.5*60);
		Vector3 mp = transform.position; //creates a vector3 named mp that is the player's coordinates 
		tileAtPlayer = tiles.WorldToCell(mp); //sets the vector3int location to the tile at the player's coordinates
		tileSprite = tiles.GetSprite(tileAtPlayer); //gets the sprite of the tile at the player's location and assigns it to the tilesprite variable
		if (tileSprite != null) //if the sprite exists (if the player is behind a background tile)
		{
			tileSpriteName = tileSprite.name; //set the variable tilespritename to the name of the tilesprite
		}

		if (tileSpriteName == "Forest" && day) // dedicated forest day time system
		{
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
				biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				biometheme = AudioManager.instance.CreateEventInstance(eventref);
				biometheme.start();
				stopit = false;
			}
		}

		if (tiles.GetTile(tileAtPlayer)) //if there is a tile behind the player
		{
			if (tileSpriteName != currentTileName||wasday != day) //if the name of the sprite is not equal to the current tile name
			{
				
				daybg = Color.black;
				nightbg = new Color(0.11f, 0.17f, 0.28f);
				wasday = day; // if it becomes night, check the biome again
				if (stopit)
				{
					biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				}
				currentTileName = tileSpriteName; //set the current tile name to the name of the sprite
				if (tileSpriteName == "Astral")
				{
					// Astral Infection
					daybg = new Color(0.06666667f, 0.003921569f, 0.07450981f);
					nightbg = daybg;
				}
				if (tileSpriteName == "Desert")
				{
					// Desert
					daybg = new Color(1f, 0.9850028f, 0.8264151f);
				}
				if (tileSpriteName == "Blight")
				{
					// Blight
					daybg = new Color(0.09783139f, 0.1509434f, 0.06778213f);
				}
				if (tileSpriteName == "Bloody")
				{
					// Bloody Meteor
					daybg = new Color(0.09433961f, 0.005603133f, 0f);
				}
				if (tileSpriteName == "Ocean")
				{
					// Ocean
					daybg = new Color(0.345283f, 0.8541663f, 1f);
				}
				if (tileSpriteName == "Sulfur")
				{
					// Sulphurous Sea
					daybg = new Color(0.5457409f, 0.8962264f, 0.3179068f);
				}
				if (tileSpriteName == "Tundra")
				{
					// Tundra
					daybg = new Color(0.7415094f, 1f, 0.95700063f);
				}
				if (tileSpriteName == "Forest")
				{
					// Spawn plains/Forest
					daybg = new Color(0.701f, 0.9691256f, 1f);
					if (!day)
					{
						biometheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Night);
						biometheme.start();
					}
				}
				if (tileSpriteName == "Feral")
				{
					// Feral Swamplands
					daybg = new Color(1f, 1f, 1f); // Change this later
				}
				if (tileSpriteName == "Jungle")
				{
					// Jungle
					daybg = new Color(0.5254902f, 1f, 0.8364275f);
				}
				if (tileSpriteName == "Planetoids")
				{
					// Planetoids
					daybg = new Color(0.701f, 0.9691256f, 1f);
				}
				if (tileSpriteName == "Underworld")
				{
					// Underworld
					daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
					nightbg = daybg;
				}
				if (tileSpriteName == "Space")
				{
					// Space
				}
				if (tileSpriteName == "Crags")
				{
					// Brimstone Crags/Azafure
					daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
					nightbg = daybg;
				}
				if (tileSpriteName == "Abyss1")
				{
					// Sulphuric depths
				}
				if (tileSpriteName == "SunkenSea")
				{
					// Sunken Sea
					daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
					nightbg = daybg;
				}
				if (tileSpriteName == "Obsidian")
				{
					// Obsidian Cliffs
					daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
				}
				if (tileSpriteName == "Garden")
				{
					// Profaned Garden
					daybg = new Color(0.5254902f, 1f, 0.8364275f); // Change this later
					nightbg = daybg;
				}
			}
		}

		if (count >= 472 && count < 1000 && day)
		{
			mainCam.backgroundColor = daybg;
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
	}
}

