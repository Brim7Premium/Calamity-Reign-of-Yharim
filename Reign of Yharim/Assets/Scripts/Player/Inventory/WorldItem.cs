using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData myDroppedItem;
    public int Amount;
    public GUIController gUIController;
    public InventoryManager inventoryManager;
    bool isWaiting = false;
    int stackDistance = 3;
    [SerializeField] private LayerMask itemLayer;

    // Update is called once per frame

    private void Start()
    {
        if (GameObject.Find("GUIManager") != null)
            gUIController = GameObject.Find("GUIManager").GetComponent<GUIController>();
        else
            Debug.LogError("No GUI Manager");
        itemLayer = 1 << LayerMask.NameToLayer("Item");
        gameObject.GetComponent<SpriteRenderer>().sprite = myDroppedItem.sprite;
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
        Collider2D[] nearbyItems = Physics2D.OverlapBoxAll(transform.position, Vector2.one * stackDistance, 0, itemLayer);
        
        foreach(Collider2D item in nearbyItems)
        {
            WorldItem script = item.gameObject.GetComponent<WorldItem>();

            if(script.myDroppedItem == myDroppedItem && !(item.gameObject == gameObject) && myDroppedItem.stackable)
            {
                Amount += script.Amount;
                Destroy(item.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                if (!isWaiting)
                {
                    inventoryManager = gUIController.inventoryManager;
                    if(inventoryManager.AddItem(myDroppedItem, Amount))
                        Destroy(gameObject);
                    else
                    {
                        inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
                        if(inventoryManager.AddItem(myDroppedItem, Amount))
                            Destroy(gameObject);
                    }
                    StartCoroutine(Wait(0.5f));
                }
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
