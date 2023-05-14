using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    public Color color;
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("I'm hitting something!");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                Debug.Log("I'm hitting a player hitbox!");
                color = new Color(1f, 0f, 0f, 0.1764706f);
                collision.gameObject.GetComponent<SpriteRenderer>().color = color;
                collision.gameObject.transform.parent.parent.SendMessage("TakeDamage", 5f); //damage the player for 5 damage. the five damage is temporary and will be replaced to work with custom damage for each entity
            }
        }
        else
            Debug.Log("I'm not hitting a player hitbox, I'm hitting " + collision.gameObject.name);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                color = new Color(1f, 1f, 1f, 0.1764706f);
                collision.gameObject.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
