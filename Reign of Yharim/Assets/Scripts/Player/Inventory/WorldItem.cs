using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData myDroppedItem;
    public int Amount;
    public GUIController gUIController;
    public bool isWaiting = false;
    float stackDistance = 3f;
    public Rigidbody2D rb;
    public Transform target;
    private LayerMask layerMask;

    // Update is called once per frame

    private void Start()
    {
        if (GameObject.Find("GUIManager") != null)
            gUIController = GameObject.Find("GUIManager").GetComponent<GUIController>();
        else
            Debug.LogError("No GUI Manager");
        layerMask = (1 << LayerMask.NameToLayer("Item")) | (1 << LayerMask.NameToLayer("WalkThroughNPCPlayer"));

        gameObject.GetComponent<SpriteRenderer>().sprite = myDroppedItem.sprite;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    public void SpawnCooldown(float seconds)
    {
        StartCoroutine(Wait(seconds));
    }
    void FixedUpdate()
    {
        TryStack();
    }
    public void TryStack()
    {
        if (isWaiting) return;

        Collider2D[] nearbyItems = Physics2D.OverlapBoxAll(transform.position, Vector2.one * stackDistance, 0, layerMask);

        //if no items detected
        if (nearbyItems.Length == 0) return;

        //Items fill move towards the stack with max items in it
        Transform target = null;
        int maxAmount = Amount;

        foreach(Collider2D item in nearbyItems)
        {
            if(item.CompareTag("Player"))
            {
                target = item.transform;
                break;
            }
            WorldItem script = item.gameObject.GetComponent<WorldItem>();

            if((script.myDroppedItem == myDroppedItem) && !(item.gameObject == gameObject) && myDroppedItem.stackable && !script.isWaiting && script.Amount>=maxAmount)
            {
                target = script.transform;
                maxAmount = script.Amount;
            }
        }

        if(target)
        {
            float speed = 50/Amount;
            rb.velocity = Mathf.Clamp(speed, 3, 10) * (target.position - transform.position).normalized;
        }

        if(target && Vector3.Distance(transform.position, target.position)<0.5f)
        {
            if(target.CompareTag("WorldItem"))
            {
                Amount += target.gameObject.GetComponent<WorldItem>().Amount;
                transform.position = (transform.position+target.position)/2;
                rb.velocity = Vector2.zero;
                Destroy(target.gameObject);
            }
            
            else if(target.root.CompareTag("Player"))
            {
                gUIController.inventoryManager.AddItem(myDroppedItem, Amount);
                Destroy(gameObject);
            }
        }
    }
    public IEnumerator Wait(float seconds)
    {
        isWaiting = true;
        yield return new WaitForSeconds(seconds);
        isWaiting = false;
    }
}
