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
    public InvSlot[] slots;
    public GameObject itemPrefab;

    public Item[] itemsToPickup;

    const string HeartNormal = "Heart_normal";
    const string HeartDeath = "Heart_death";
    const string HeartFull = "Heart_full";

    private void Start()
    {
        inventoryOpened = false;
    }

    void Update()
    {
        timeText.text = GameTime.displayTime;

        if (playerAI.Life >= 0)
            healthText.text = "Health: " + playerAI.Life + "/" + playerAI.LifeMax;
        else
            healthText.text = "Health: 0/" + playerAI.LifeMax;

        if (playerAI.Life == playerAI.LifeMax)
        {
            ChangeAnimationState(HeartFull);
        }
        if (playerAI.Life != playerAI.LifeMax && playerAI.Life > 0f)
        {
            ChangeAnimationState(HeartNormal);
        }
        if (playerAI.Life <= 0f)
        {
            ChangeAnimationState(HeartDeath);
        }

        if (Input.GetKeyDown(KeyCode.Return) && inventoryOpened == false)
        {
            inventoryOpened = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && inventoryOpened == true)
        {
            inventoryOpened = false;
        }

        if (inventoryOpened == true)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }

    public void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return; //if currentAnimationState equals newAnimationState, stop the method (prevents animations from interupting themselves)

        guiHeartAnimator.Play(newAnimationState); //play the newState animation

        currentAnimationState = newAnimationState; //set currentAnimationState to newAnimationState
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InvSlot slot = slots[i];
            InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }
    public void SpawnNewItem(Item item, InvSlot slot)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, slot.transform);
        InvItem invItem = itemGameObject.GetComponent<InvItem>();
        invItem.InitItem(item);
    }
    public void PickUpItem()
    {
        AddItem(itemsToPickup[Random.Range(0, itemsToPickup.Length)]);
    }
}
