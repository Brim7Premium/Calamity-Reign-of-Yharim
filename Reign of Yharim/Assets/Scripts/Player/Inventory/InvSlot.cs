using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvSlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Sprite selectedSprite, notSelectedSprite;

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
        if (transform.childCount == 0) //if the slot is not already occupied
        {
            GameObject dropped = eventData.pointerDrag;
            InvItem draggableItem = dropped.GetComponent<InvItem>();
            draggableItem.parentAfterDrag = transform;
        }
    }
}
