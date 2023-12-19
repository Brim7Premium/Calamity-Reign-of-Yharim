using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InvItem[] inventory;
    public Transform[] slots;
    public int invSize;
    public Transform GUI;
    [SerializeField] private GameObject InvItem; 
    [SerializeField] private GameObject InvSlot;
    public void Start()
    {
        inventory = new InvItem[invSize];
        slots = new Transform[invSize];
        for(int i = 0; i<invSize; i++)
        {
            GameObject slot = Instantiate(InvSlot, GUI);
            slot.GetComponent<InvSlot>().number = i;
            slot.GetComponent<InvSlot>().inventoryManager = this;
            slots[i] = slot.GetComponent<Transform>();
        }
    }
    //Adds in the first availablt slot
    public bool AddItem(ItemData item, int _count = 1)
    {
        for(int i = 0; i<invSize; i++)
        {
            if(inventory[i] == null)//if no item in slot
            {
                inventory[i] = Instantiate(InvItem, slots[i]).GetComponent<InvItem>();
                inventory[i].InitItem(item, this, _count);
                return true;
            }
            else if(inventory[i].item == item)//if the same item in slot
            {
                inventory[i].count += _count;
                return true;
            }
            
        }
        return false;
    }
    public bool AddItem(InvItem item, int slot)
    {
        if(inventory[slot] == null)
        {
            inventory[slot] = item;
            item.transform.SetParent(slots[slot]);
            return true;
        }
        if(inventory[slot].item == item.item)
        {
            inventory[slot].count += item.count;
            Destroy(item);
            return true;
        }
        return false;
    }
    public InvItem TakeItem(int slot, int _count = -1)
    {
        if(slot>invSize || slot<0) 
        {
            Debug.LogError(nameof(slot) + " doesn't have slot number" + slot);
            return null;
        }
        if(_count<-1 || _count == 0)
        {
            Debug.LogError(nameof(_count) + " : " + _count + " is a wrong count");
            return null;
        }
        if(inventory[slot] == null) return null;

        InvItem item = inventory[slot];

        if(_count == -1)
        {
            inventory[slot] = null;
            return item;
        }
        else
        {
            item.count -= _count;
            InvItem newItem = Instantiate(InvItem, slots[slot]).GetComponent<InvItem>();
            newItem.InitItem(item.item, this);
            return newItem;
        }
    }
    public InvItem TakeItem(ItemData item, int _count = -1)
    {
        for(int i = 0; i<invSize; i++)
        {
            InvItem itemScript = inventory[i].GetComponent<InvItem>();
            if(itemScript.item == item)
            {
                return TakeItem(i, _count);
            }
        }
        return null;
    }
}
