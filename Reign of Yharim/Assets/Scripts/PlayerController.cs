using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded = false;

    public Tilemap tiles;
    public Vector3Int tileAtPlayer;
    public Sprite tileSprite;
    private string tileSpriteName;
    public string currentTileName;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal"); //sets horizontal to -1 or 1 based on the player's input

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);//sets the speed of the player along the x coordinate to 1 * speed or -1 * speed, allowing the player to move horizontally based on input

        GetTile();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    private bool IsGrounded()
    {
        return isGrounded;
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
                    GetComponent<AudioSource>().Play(); //plays the audiosource
                }
                else
                {
                    GetComponent<AudioSource>().Stop(); //stops the audiosource if the tile's name is not astral
                }
            }
        }
    }
}
