using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CollisionSystem : MonoBehaviour
{
    private Color color;
    private string npcName;

    [SerializeField] private PlayerAI playerAI;
    [SerializeField] private EventReference hitSound;

    private void Start()
    {
        playerAI = gameObject.transform.parent.parent.GetComponent<PlayerAI>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPCs") && collision.gameObject.tag == "Hitbox")
        {
            if (playerAI.immune == false)
            {
                npcName = collision.gameObject.transform.parent.parent.name;
                Debug.Log(npcName + "is hitting " + gameObject.name);
                color = new Color(1f, 0f, 0f, 0.1764706f);
                gameObject.GetComponent<SpriteRenderer>().color = color;
                gameObject.transform.parent.parent.SendMessage("TakeDamage", collision.gameObject.transform.parent.parent.GetComponentInChildren<NPC>().damage);
                AudioManager.instance.PlayOneShot(hitSound, transform.position);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPCs") && collision.gameObject.tag == "Hitbox")
        {
            color = new Color(1f, 1f, 1f, 0.1764706f);
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }

    }
}
