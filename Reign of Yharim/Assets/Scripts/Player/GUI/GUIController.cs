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
    public InvSlot[] slots;
    public GameObject itemPrefab;
    int selectedSlot = -1;
    public GameObject worldItem;
    public GameObject player;

    public Item[] itemsToPickup;

    const string HeartNormal = "Heart_normal";
    const string HeartDeath = "Heart_death";
    const string HeartFull = "Heart_full";

    private void Start()
    {
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
            inventoryOpened = true;
        else if (Input.GetKeyDown(KeyCode.Return) && inventoryOpened == true)
            inventoryOpened = false;

        if (inventoryOpened == true)
            inventory.SetActive(true);
        else
            inventory.SetActive(false);
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 10)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Item droppedItem = GetSelectedItem(false);

            if (GetSelectedItem(false))
            {
                GameObject worldClone = Instantiate(worldItem, player.transform.position, Quaternion.identity);
                worldClone.GetComponent<WorldItem>().SpawnCooldown(2f);
                SpriteRenderer spriteRenderer = worldClone.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = droppedItem.image;
                worldClone.GetComponent<WorldItem>().myDroppedItem = droppedItem;
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
            slots[selectedSlot].Deselect();
        slots[value].Select();
        selectedSlot = value;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InvSlot slot = slots[i];
            InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.ReCount();
                return true;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            InvSlot slot = slots[i];
            InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }
    public void SpawnNewItem(Item item, InvSlot slot)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, slot.transform);
        InvItem invItem = itemGameObject.GetComponent<InvItem>();
        invItem.InitItem(item);
    }

    public Item GetSelectedItem(bool use) //without bool use, method will get the selected item, with bool use, method will get and remove one of selected item
    {
        InvSlot slot = slots[selectedSlot];
        InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
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
        bool result = AddItem(itemsToPickup[Random.Range(0, itemsToPickup.Length)]);
        if (result == true)
            Debug.Log("Item added");
        else
            Debug.LogWarning("Item could not be added");
    }
}