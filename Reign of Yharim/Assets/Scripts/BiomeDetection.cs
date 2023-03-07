using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeDetection : MonoBehaviour
{
    public Tilemap tiles;
    public Vector3Int tileAtPlayer;
    public Sprite tileSprite;
    private string tileSpriteName;
    public string currentTileName;

    public Camera mainCam;

    public AudioSource audioSource;
    public AudioClip Desert;
    public AudioClip Astral;
    public AudioClip Blight;
    public AudioClip Tundra;
    public AudioClip Bloody;
    public AudioClip Ocean;
    public AudioClip Sulfur;

    void Update()
    {
        GetTile();
    }

    void GetTile()
    {
        Vector3 mp = transform.position; //creates a vector3 named mp that is the player's coordinates 
        tileAtPlayer = tiles.WorldToCell(mp); //sets the vector3int location to the tile at the player's coordinates
        tileSprite = tiles.GetSprite(tileAtPlayer); //gets the sprite of the tile at the player's location and assigns it to the tilesprite variable
        if (tileSprite != null) //if the sprite exists (if the player is behind a background tile)
        {
            tileSpriteName = tileSprite.name; //set the variable tilespritename to the name of the tilesprite
        }


        if (tiles.GetTile(tileAtPlayer)) //if there is a tile behind the player
        {
            if (tileSpriteName != currentTileName) //if the name of the sprite is not equal to the current tile name
            {
                currentTileName = tileSpriteName; //set the current tile name to the name of the sprite
                if (tileSpriteName == "Astral") //if the tile's name is astral
                {
                    mainCam.backgroundColor = new Color(0.06666667f, 0.003921569f, 0.07450981f);
                    //audioSource.clip = Astral;
                    //audioSource.Play();
                }
                if (tileSpriteName == "Desert")
                {
                    mainCam.backgroundColor = new Color(1f, 0.9850028f, 0.8264151f);
                    //audioSource.clip = Desert;
                    //audioSource.Play();
                }
                if (tileSpriteName == "Blight")
                {
                    mainCam.backgroundColor = new Color(0.09783139f, 0.1509434f, 0.06778213f);
                    //audioSource.clip = Blight;
                    //audioSource.Play();
                }
                if (tileSpriteName == "Bloody")
                {
                    mainCam.backgroundColor = new Color(0.09433961f, 0.005603133f, 0f);
                    //audioSource.clip = Bloody;
                    //audioSource.Play();
                }
                if (tileSpriteName == "Ocean")
                {
                    mainCam.backgroundColor = new Color(0.345283f, 0.8541663f, 1f);
                    //audioSource.clip = Ocean;
                    //audioSource.Play();
                }
                if (tileSpriteName == "Sulfur")
                {
                    mainCam.backgroundColor = new Color(0.5457409f, 0.8962264f, 0.3179068f);
                    //audioSource.clip = Sulfur;
                    //audioSource.Play();
                }
                if (tileSpriteName == "Tundra")
                {
                    mainCam.backgroundColor = new Color(0.7415094f, 1f, 0.95700063f);
                    //audioSource.clip = Tundra;
                    //audioSource.Play();
                }
                if (tileSpriteName == "Forest")
                {
                    mainCam.backgroundColor = new Color(0.5622642f, 0.9691256f, 1f);
                }
                if (tileSpriteName == "Light")
                {
                    mainCam.backgroundColor = new Color(1f, 1f, 1f);
                }
                if (tileSpriteName == "Jungle")
                {
                    mainCam.backgroundColor = new Color(0.5254902f, 1f, 0.8364275f);
                }
            }
        }
    }
}

