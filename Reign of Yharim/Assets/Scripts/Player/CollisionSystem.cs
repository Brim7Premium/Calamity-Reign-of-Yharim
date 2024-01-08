using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
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
            System.Random rnd = new System.Random();
            FMODUnity.EventReference[] hitevents = {FMODEvents.instance.PlayerHit1, FMODEvents.instance.PlayerHit2, FMODEvents.instance.PlayerHit3};
            int r = rnd.Next(hitevents.Length);
            if (collision.gameObject.tag == "Hitbox")
            {
                if (playerAI.immune == false)
                {
                    entityName = collision.gameObject.transform.parent.parent.name;
                    color = new Color(1f, 0f, 0f, 0.1764706f);
                    gameObject.GetComponent<SpriteRenderer>().color = color;
                    gameObject.transform.parent.parent.SendMessage("TakeDamage", collision.gameObject.transform.parent.parent.GetComponentInChildren<NPC>().Damage);
                    AudioManager.instance.PlayOneShot(hitevents[r]);
                    Debug.Log(gameObject.name + " is hitting " + entityName + $". PlayerHit{r+1} was played.");
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
                    AudioManager.instance.PlayOneShot(hitevents[r]);
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
