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
	private bool wasday = true;
	private int daythemenum = 0;

	private int count;

	void Start()
	{
	}

	void Update()
	{
		GetTile();
	}

	void GetTile()
	{
		var stopit = true;
		count = GameObject.Find("WorldManager").GetComponent<GameTime>().count;
		day = (count >= 7.5*60 && count < 19.5*60);
		Vector3 mp = transform.position; //creates a vector3 named mp that is the player's coordinates 
		tileAtPlayer = tiles.WorldToCell(mp); //sets the vector3int location to the tile at the player's coordinates
		tileSprite = tiles.GetSprite(tileAtPlayer); //gets the sprite of the tile at the player's location and assigns it to the tilesprite variable
		if (tileSprite != null) //if the sprite exists (if the player is behind a background tile)
		{
			tileSpriteName = tileSprite.name; //set the variable tilespritename to the name of the tilesprite
		}

		if (tileSpriteName == "Forest" && (count >= 4.5*60 && count < 19.5*60))
		{
			var newdaythemenum = daythemenum;
			if (count >= 4.5*60 && count < 7.5*60)
			{
				daythemenum = 1;
			}
			else if (count >= 7.5*60 && count < 12*60)
			{
				daythemenum = 2;
			}
			else if (count >= 12*60 && count < 16.5*60)
			{
				daythemenum = 3;
			}
			else
			{
				daythemenum = 4;
			}
			if (daythemenum != newdaythemenum)
			{
				biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				var eventreference = FMODUnity.EventReference.Find("event:/Music/Biomes/LegendStrife" + daythemenum.ToString());
				biometheme = AudioManager.instance.CreateEventInstance(eventreference);
				biometheme.start();
				stopit = false;
			}
		}

		if (tiles.GetTile(tileAtPlayer)) //if there is a tile behind the player
		{
			if (tileSpriteName != currentTileName||wasday != day) //if the name of the sprite is not equal to the current tile name
			{
				wasday = day; // if it becomes night, check the biome again
				if (stopit)
				{
					biometheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				}
				currentTileName = tileSpriteName; //set the current tile name to the name of the sprite
				if (tileSpriteName == "Astral") //if the tile's name is astral
				{
					/*
					Astral Infection
					mainCam.backgroundColor = new Color(0.06666667f, 0.003921569f, 0.07450981f);
					audioSource.clip = Astral;
					audioSource.Play(); 
					*/
				}
				if (tileSpriteName == "Desert")
				{
					/*
					Desert
					mainCam.backgroundColor = new Color(1f, 0.9850028f, 0.8264151f);
					audioSource.clip = Desert;
					audioSource.Play(); 
					*/
				}
				if (tileSpriteName == "Blight")
				{
					/*
					Blight
					mainCam.backgroundColor = new Color(0.09783139f, 0.1509434f, 0.06778213f);
					audioSource.clip = Blight;
					audioSource.Play();
					*/
				}
				if (tileSpriteName == "Bloody")
				{
					/*
					Bloody Meteor
					mainCam.backgroundColor = new Color(0.09433961f, 0.005603133f, 0f);
					audioSource.clip = Bloody;
					audioSource.Play();
					*/
				}
				if (tileSpriteName == "Ocean")
				{
					/*
					Ocean
					mainCam.backgroundColor = new Color(0.345283f, 0.8541663f, 1f);
					audioSource.clip = Ocean;
					audioSource.Play();
					*/
				}
				if (tileSpriteName == "Sulfur")
				{
					/*
					Sulphurous Sea
					mainCam.backgroundColor = new Color(0.5457409f, 0.8962264f, 0.3179068f);
					audioSource.clip = Sulfur;
					audioSource.Play();
					*/
				}
				if (tileSpriteName == "Tundra")
				{
					/*
					Tundra
					mainCam.backgroundColor = new Color(0.7415094f, 1f, 0.95700063f);
					audioSource.clip = Tundra;
					audioSource.Play();
					*/
				}
				if (tileSpriteName == "Forest")
				{
					//Spawn plains/Forest
					if (count >= 4.5*60 && count < 19.5*60)
					{
						mainCam.backgroundColor = new Color(0.701f, 0.9691256f, 1f);
					}
					else
					{
						mainCam.backgroundColor = new Color(0.11f, 0.17f, 0.28f);
						biometheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Night);
						biometheme.start();
					}
				}
				if (tileSpriteName == "Feral")
				{
					//Feral Swamplands
					//mainCam.backgroundColor = new Color(1f, 1f, 1f); Change this later
				}
				if (tileSpriteName == "Jungle")
				{
					//Jungle
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f);
				}
				if (tileSpriteName == "Sky")
				{
					//Sky
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
				if (tileSpriteName == "Underworld")
				{
					//Underworld
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
				if (tileSpriteName == "Space")
				{
					//Space
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
				if (tileSpriteName == "Crags")
				{
					//Brimstone Crags/Azafure
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
				if (tileSpriteName == "Abyss2")
				{
					//Abyss tier 1
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
				if (tileSpriteName == "SunkenSea")
				{
					//Sunken Sea
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
				if (tileSpriteName == "Obsidian")
				{
					//Obsidian Cliffs
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
				if (tileSpriteName == "Garden")
				{
					//Profaned Garden
					//mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f); Change this later
				}
			}
		}
	}
}

