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
    public AudioSource audioSource;
    public AudioClip Desert;
    public AudioClip Astral;

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
                    //GetComponent<AudioSource>().Play(); //plays the audiosource
                    audioSource.clip = Astral;
                    audioSource.Play();
                }
                if(tileSpriteName == "Desert")
                {
                    audioSource.clip = Desert;
                    audioSource.Play();
                }
            }
        }
    }
}

