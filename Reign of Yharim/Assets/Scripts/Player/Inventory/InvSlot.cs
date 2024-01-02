using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvSlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Sprite selectedSprite, notSelectedSprite;
    public int number;
    public InventoryManager inventoryManager;

    private void Awake()
    {
        Deselect();
    }
    public void Select()
    {
        image.sprite = selectedSprite;
        if (this.gameObject.transform.childCount > 0 && GameObject.Find("Player") != null)
        {
            GameObject.Find("/Player/Item").transform.GetComponent<SpriteRenderer>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        }

        else if (this.gameObject.transform.childCount == 0 && GameObject.Find("Player") != null)
        {
            GameObject.Find("/Player/Item").transform.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    public void Deselect()
    {
        image.sprite = notSelectedSprite;
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InvItem draggableItem = dropped.GetComponent<InvItem>();
        InvItem itemInSlot = transform.GetComponentInChildren<InvItem>();
        //if the slot is not already occupied or the same item in the slot
        if (transform.childCount == 0 || (draggableItem.item.stackable && draggableItem.item == itemInSlot.item)) 
        {
            draggableItem.parentAfterDrag = transform;
            itemInSlot.count += draggableItem.count;
        }
    }
}
