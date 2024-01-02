using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GUIController : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text healthText;
    public PlayerAI playerAI;
    public GameObject inventory;

    public string currentAnimationState;

    private bool inventoryOpened;
    public int maxStackedItems = 999;
    public GameObject itemPrefab;
    int selectedSlot = -1;
    public GameObject worldItem;
    public GameObject player;
    public GameObject heldItemObject;
    public TMP_Text itemText;
    public InventoryManager inventoryManager;

    public ItemData[] itemsToPickup;

    private IEnumerator Start()
    {
        //Invetory instantiated in runtime
        //At least one frame should pass before any actions
        yield return new WaitForFixedUpdate();
        inventoryOpened = false;
        ChangeSelectedSlot(0);
    }

    void Update()
    {
        float numberOfHotSlots = 5;
        timeText.text = GameTime.displayTime;

        if (playerAI.Life >= 0)
            healthText.text = "Health: " + playerAI.Life + "/" + playerAI.LifeMax;
        else
            healthText.text = "Health: 0/" + playerAI.LifeMax;

        if (Input.GetKeyDown(KeyCode.Return) && inventoryOpened == false)
        {
            // AudioManager.instance.
            inventoryOpened = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && inventoryOpened == true)
        {
            inventoryOpened = false;
        }

        inventory.SetActive(inventoryOpened);
        
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= numberOfHotSlots)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        InvItem heldItem = GetSelectedItem(false);

        if (GetSelectedItem(false))
            itemText.text = heldItem.item.displayName;
        else
            itemText.text = ("Empty Slot");

        if (Input.GetKeyDown(KeyCode.Backspace))
        {

            if (GetSelectedItem(false))
            {
                GameObject worldClone = Instantiate(worldItem, player.transform.position, Quaternion.identity);
                worldClone.GetComponent<WorldItem>().SpawnCooldown(2f);
                SpriteRenderer spriteRenderer = worldClone.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = heldItem.item.sprite;
                worldClone.GetComponent<WorldItem>().myDroppedItem = heldItem.item;
                worldClone.GetComponent<WorldItem>().Amount = heldItem.count;
                GetSelectedItem(true);
                worldClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f * playerAI.isFacing, 200f));
            }
        }
    }

    void ChangeSelectedSlot(int value)
    {
        if (selectedSlot >= 0)
            inventoryManager.slots[selectedSlot].gameObject.GetComponent<InvSlot>().Deselect();
        inventoryManager.slots[value].gameObject.GetComponent<InvSlot>().Select();
        selectedSlot = value;
    }

    public InvItem GetSelectedItem(bool use) //without bool use, method will get the selected item, with bool use, method will get and remove one of selected item
    {
        InvSlot slot = inventoryManager.slots[selectedSlot].gameObject.GetComponent<InvSlot>();
        InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();
        if (itemInSlot != null)
        {
            if (use == true)
            {
                Destroy(inventoryManager.TakeItem(slot.number).gameObject);
            }
            return itemInSlot;
        }
        else
            return null;
    }
    //demo script
    public void PickUpItem()
    {
        bool result = inventoryManager.AddItem(itemsToPickup[Random.Range(0, itemsToPickup.Length)]);
        if (result == true)
            Debug.Log("Item added");
        else
            Debug.LogWarning("Item could not be added");
    }
}