using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;

public class CollisionSystem : MonoBehaviour
{
    private Color color;
    private string entityName;

    [SerializeField] private PlayerAI playerAI;

    private void Start()
    {
        playerAI = gameObject.transform.parent.parent.GetComponent<PlayerAI>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPCs"))
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                if (playerAI.immune == false)
                {
                    entityName = collision.gameObject.transform.parent.parent.name;
                    Debug.Log(gameObject.name + " is hitting " + entityName);
                    color = new Color(1f, 0f, 0f, 0.1764706f);
                    gameObject.GetComponent<SpriteRenderer>().color = color;
                    gameObject.transform.parent.parent.SendMessage("TakeDamage", collision.gameObject.transform.parent.parent.GetComponentInChildren<NPC>().Damage);
                    FMODUnity.RuntimeManager.PlayOneShot(SFX.PlayerHit, transform.position);
                }
            }
            if (collision.gameObject.tag == "ProjectileHitbox")
            {
                if (playerAI.immune == false)
                {
                    entityName = collision.gameObject.transform.parent.parent.name;
                    Debug.Log(gameObject.name + " is hitting " + entityName);
                    color = new Color(1f, 0f, 0f, 0.1764706f);
                    gameObject.GetComponent<SpriteRenderer>().color = color;

                    gameObject.transform.parent.parent.SendMessage("TakeDamage", collision.gameObject.transform.parent.parent.gameObject.GetComponent<Projectile>().damage);
                    FMODUnity.RuntimeManager.PlayOneShot(SFX.PlayerHit, transform.position);
                }
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
