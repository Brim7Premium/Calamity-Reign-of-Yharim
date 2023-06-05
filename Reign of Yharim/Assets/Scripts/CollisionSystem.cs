using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    public Color color;
    public float damage = 5f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                color = new Color(1f, 0f, 0f, 0.1764706f);
                collision.gameObject.GetComponent<SpriteRenderer>().color = color;
                collision.gameObject.transform.parent.parent.SendMessage("TakeDamage", damage); //damage the player for 5 damage. the five damage is temporary and will be replaced to work with custom damage for each entity
            }
        }
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
