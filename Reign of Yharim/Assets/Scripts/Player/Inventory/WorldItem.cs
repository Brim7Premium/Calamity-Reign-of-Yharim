using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item myDroppedItem;
    public GUIController gUIController;
    bool wait = true;

    // Update is called once per frame

    public void SpawnCooldown(float seconds)
    {
        StartCoroutine(Wait(seconds));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                if (wait)
                {
                    gUIController.AddItem(myDroppedItem);
                    Destroy(gameObject);
                    StartCoroutine(Wait(0.5f));
                }
            }
        }
    }
    public IEnumerator Wait(float seconds)
    {
        wait = false;
        yield return new WaitForSeconds(seconds);
        wait = true;
    }
}
