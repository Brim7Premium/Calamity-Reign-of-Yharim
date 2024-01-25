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
    [HideInInspector] public bool InHand;
    public InventoryManager inventoryManager;
    public InvSlot slot;
    //Defaults to hand
    public static InvItem InitItem(ItemData _item, InventoryManager _inventoryManager, int _count = 1, int _slot = -1)
    {
        GameObject prefab = Resources.Load<GameObject>("InvItem");
        InvItem script;
        if(_slot != -1){
            script = Instantiate(prefab, _inventoryManager.slots[_slot]).GetComponent<InvItem>();
            script.slot = _inventoryManager.slots[_slot].gameObject.GetComponent<InvSlot>();
        }
        else
        {
            script = Instantiate(prefab, _inventoryManager.slots[0].root).GetComponent<InvItem>();
            script.slot = null;
        }
        
        script.item = _item;
        script.count = _count;
        script.image = script.gameObject.GetComponent<Image>();
        script.image.sprite = _item.sprite;
        script.inventoryManager = _inventoryManager;
        script.ReCount();
        return script;
    }
    public void ReCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
    void Update()
    {
        if(InHand)
        {
            Debug.Log("Dragging");
            Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }
    public void PutInHand()
    {
        Debug.Log("Begin Drag");
        slot = null;
        //Item won't be covered up by other UI elements
        transform.SetParent(transform.root); 
        transform.SetAsLastSibling(); 
        InHand = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!InHand)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                inventoryManager.TakeItem(transform.parent.gameObject.GetComponent<InvSlot>().number);
                PutInHand();
            }

            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                inventoryManager.TakeItem(transform.parent.gameObject.GetComponent<InvSlot>().number, 1).PutInHand();
            }
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

            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if(newSlot && newSlot.inventoryManager.AddItem(this, newSlot.number)) 
                {
                    slot = newSlot;
                    inventoryManager = newSlot.inventoryManager;
                    InHand = false;
                }
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                newSlot.inventoryManager.AddItem(item, 1, newSlot.number);
                count -= 1;
            }
        }
    }
}

