using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data;
using System;

public class InvItem : MonoBehaviour, IPointerClickHandler
{
    public Image image;
    public TMP_Text countText;

    [HideInInspector] public ItemData item;
    [SerializeField] private int _count = 1;
    public int count
    {
        set
        {
            if(item.stackable || value<=1)
            {
                _count = value;
                ReCount();
                if(count<=0) Destroy(gameObject);
            }
        }
        get => _count;
        
    }
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool dragging;
    public InventoryManager inventoryManager;
    public InvSlot slot;
    public void InitItem(ItemData _item, InventoryManager _inventoryManager, int _count = 1)
    {
        item = _item;
        count = _count;
        image = gameObject.GetComponent<Image>();
        image.sprite = _item.sprite;
        inventoryManager = _inventoryManager;
        ReCount();
    }
    public void ReCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
    void Update()
    {
        if(dragging)
        {
            Debug.Log("Dragging");
            Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!dragging)
        {
            inventoryManager.TakeItem(transform.parent.gameObject.GetComponent<InvSlot>().number);
            Debug.Log("Begin Drag");
            
            //Item won't be covered up by other UI elements
            transform.SetParent(transform.root); 
            transform.SetAsLastSibling(); 
            dragging = true;
        }
        else
        {
            //gets slot underneath the cursor if such exists
            InvSlot newSlot = null;
            List<RaycastResult> posSlot = new();
            transform.root.GetComponent<GraphicRaycaster>().Raycast(eventData, posSlot);
            //posSlot.Reverse();
            foreach (var i in posSlot)
            {
                if (i.gameObject.TryGetComponent<InvSlot>(out newSlot)) break;
            }

            if(newSlot && newSlot.inventoryManager.AddItem(this, newSlot.number)) 
            {
                transform.SetParent(newSlot.transform);
                slot = newSlot;
                inventoryManager = newSlot.inventoryManager;
                dragging = false;
                image.raycastTarget = true;
            }
        }
    }
}

