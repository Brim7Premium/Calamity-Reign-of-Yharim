using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data;

public class InvItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TMP_Text countText;

    [HideInInspector] public ItemData item;
    [SerializeField] private int _count;
    public int count
    {
        set
        {
            if(item.stackable)
            {
                _count = value;
                ReCount();
                if(_count<=0) Destroy(gameObject);
            }
        }
        get => _count;
        
    }
    [HideInInspector] public Transform parentAfterDrag;
    private InventoryManager inventoryManager;

    public void InitItem(ItemData newItem, InventoryManager _inventoryManager, int _count = 1)
    {
        item = newItem;
        count = _count;
        image = gameObject.GetComponent<Image>();
        image.sprite = newItem.sprite;
        inventoryManager = _inventoryManager;
        ReCount();
    }

    public void ReCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inventoryManager.TakeItem(transform.parent.gameObject.GetComponent<InvSlot>().number);
        Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent; //format stuff
        transform.SetParent(transform.root); //format stuff
        transform.SetAsLastSibling(); //format stuff
        image.raycastTarget = false; //allows for us to check if there is a slot underneath
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        Vector3 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        transform.position = mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag); //format stuff
        image.raycastTarget = true;
        inventoryManager = transform.parent.gameObject.GetComponent<InvSlot>().inventoryManager;
        inventoryManager.AddItem(this, transform.parent.gameObject.GetComponent<InvSlot>().number);
    }
}

