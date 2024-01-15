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

    }
    public void Deselect()
    {
        image.sprite = notSelectedSprite;
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InvItem draggableItem = dropped.GetComponent<InvItem>();
        if(inventoryManager.AddItem(draggableItem, number)) draggableItem.parentAfterDrag = transform;
    }

    public GameObject playerobj;
    public SpriteRenderer playeritem;

    void Update()
    {
        if (playerobj.GetComponent<PlayerAI>().IsAttacking)
        {
            playeritem.transform.GetComponent<SpriteRenderer>().sprite = null;
        }

        else if (this.gameObject.transform.childCount > 0)
        {
            playeritem.transform.GetComponent<SpriteRenderer>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        }
    }
}
