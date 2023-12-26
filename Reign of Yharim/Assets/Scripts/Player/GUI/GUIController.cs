using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GUIController : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text healthText;
    public Animator guiHeartAnimator;
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

    const string HeartNormal = "Heart_normal";
    const string HeartDeath = "Heart_death";
    const string HeartFull = "Heart_full";

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
        timeText.text = GameTime.displayTime;

        if (playerAI.Life >= 0)
            healthText.text = "Health: " + playerAI.Life + "/" + playerAI.LifeMax;
        else
            healthText.text = "Health: 0/" + playerAI.LifeMax;

        if (playerAI.Life == playerAI.LifeMax)
            ChangeAnimationState(HeartFull);
        if (playerAI.Life != playerAI.LifeMax && playerAI.Life > 0f)
            ChangeAnimationState(HeartNormal);
        if (playerAI.Life <= 0f)
            ChangeAnimationState(HeartDeath);

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
            if (isNumber && number > 0 && number < 10)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        ItemData heldItem = GetSelectedItem(false);

        if (GetSelectedItem(false))
            itemText.text = heldItem.displayName;
        else
            itemText.text = ("Empty Slot");

        if (Input.GetKeyDown(KeyCode.Backspace))
        {

            if (GetSelectedItem(false))
            {
                GameObject worldClone = Instantiate(worldItem, player.transform.position, Quaternion.identity);
                worldClone.GetComponent<WorldItem>().SpawnCooldown(2f);
                SpriteRenderer spriteRenderer = worldClone.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = heldItem.sprite;
                worldClone.GetComponent<WorldItem>().myDroppedItem = heldItem;
                GetSelectedItem(true);
                worldClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f * playerAI.isFacing, 200f));
            }
        }
    }

    public void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return; //if currentAnimationState equals newAnimationState, stop the method (prevents animations from interupting themselves)

        guiHeartAnimator.Play(newAnimationState); //play the newState animation

        currentAnimationState = newAnimationState; //set currentAnimationState to newAnimationState
    }

    void ChangeSelectedSlot(int value)
    {
        if (selectedSlot >= 0)
            inventoryManager.slots[selectedSlot].gameObject.GetComponent<InvSlot>().Deselect();
        inventoryManager.slots[value].gameObject.GetComponent<InvSlot>().Select();
        selectedSlot = value;
    }

    public ItemData GetSelectedItem(bool use) //without bool use, method will get the selected item, with bool use, method will get and remove one of selected item
    {
        InvSlot slot = inventoryManager.slots[selectedSlot].gameObject.GetComponent<InvSlot>();
        InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();
        if (itemInSlot != null)
        {
            ItemData item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                    itemInSlot.ReCount();
            }
            return item;
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