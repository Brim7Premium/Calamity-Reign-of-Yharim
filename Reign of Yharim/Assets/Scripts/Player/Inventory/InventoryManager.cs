using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    //god forgive me for crimes I've commited
    [SerializeField] private InvItem[] inventory;
    public InvItem this[int i] 
    {
        get => inventory[i];
        private set => inventory[i] = value;
    }
    public InventoryManager[] pages;
    public Transform[] slots;
    public ItemData.InventoryType StoringType;
    public int invSize;
    public Transform GUI;
    [SerializeField] private GameObject InvItemPrefab; 
    [SerializeField] private GameObject InvSlotPrefab;
    public void Start()
    {
        //Only referred to this inventory excluding pages
        inventory = new InvItem[invSize];
        slots = new Transform[invSize];
        int i = 0;
        foreach(InvSlot script in GUI.GetComponentsInChildren<InvSlot>())
        {
            script.number = i;
            script.inventoryManager = this;
            slots[i] = script.transform;
            i += 1;
            if(i >= invSize) break;
        }
        for(;i<invSize; i++)
        {
            InvSlot slot = Instantiate(InvSlotPrefab, GUI).GetComponent<InvSlot>();
            slot.number = i;
            slot.inventoryManager = this;
            slots[i] = slot.transform;
        }
    }
    //Adds in the first availablt slot
    //Universal slots are checked first
    //Slot means anything only in this inventory

    public bool AddItem(ItemData item, int count, int slot)
    {
        if(inventory[slot] == null)//if no item in slot
        {
            inventory[slot] = InvItem.InitItem(item, this, count, slot);
            return true;
        }
        else if(inventory[slot].item == item && item.stackable)//if the same item in slot
        {
            inventory[slot].count += count;
            return true;
        }
        return false;
    }

    
    public bool AddItem(ItemData item, int count = 1)
    {
        if(StoringType == ItemData.InventoryType.All)
        {   

            for(int i = 0; i<invSize; i++)
            {
                if (AddItem(item, count, i)) return true;
            }
        }

        foreach(InventoryManager page in pages)
        {   
            if(page.StoringType == ItemData.InventoryType.All) 
            {
                for(int i = 0; i<invSize; i++)
                {
                    if (AddItem(item, count, i)) return true;
                }
            }
        }



        if(StoringType == item.inventoryType)
        {
            for(int i = 0; i<invSize; i++)
            {
                if (AddItem(item, count, i)) return true;
            }
        }

        foreach(InventoryManager page in pages)
        {
            for(int i = 0; i<invSize; i++)
            {
                if (AddItem(item, count, i)) return true;
            }
        }
        return false;
    }
    public InvItem TakeItem(ItemData item, int count = -1)
    {

        if(StoringType == ItemData.InventoryType.All)
        {   
            for(int i = 0; i<invSize; i++)
            {
                InvItem itemScript = inventory[i].GetComponent<InvItem>();
                if(itemScript.item == item)
                {
                    return TakeItem(i, count);
                }
            }
        }

        foreach(InventoryManager page in pages)
        {   
            if(page.StoringType == ItemData.InventoryType.All) 
            {
                InvItem itemFromAnotherInventory = page.TakeItem(item, count);
                if(itemFromAnotherInventory!=null) return itemFromAnotherInventory;
            }
        }



        if(StoringType == item.inventoryType)
        {
            for(int i = 0; i<invSize; i++)
            {
                InvItem itemScript = inventory[i].GetComponent<InvItem>();
                if(itemScript.item == item)
                {
                    return TakeItem(i, count);
                }
            }
        }

        foreach(InventoryManager page in pages)
        {
            if(page.StoringType == item.inventoryType) 
            {
                InvItem itemFromAnotherInventory = page.TakeItem(item, count);
                if(itemFromAnotherInventory!=null) return itemFromAnotherInventory;
            }
        }


        
        return null;
    }
    //Other overloads used mainly by drag handlers and operate only in this inventory
    public bool AddItem(InvItem item, int slot)
    {
        if(slot>invSize || slot<0) 
        {
            Debug.LogError(name + " doesn't have slot number " + slot);
            return false;
        }
        if(!((item.item.inventoryType == StoringType) || (StoringType == ItemData.InventoryType.All))) return false;

        if(inventory[slot] == null)
        {
            inventory[slot] = item;
            item.transform.SetParent(slots[slot]);
            return true;
        }
        
        if((inventory[slot].item == item.item) && item.item.stackable)
        {
            inventory[slot].count += item.count;
            Destroy(item.gameObject);
            return true;
        }
        
        return false;
    }
    public InvItem TakeItem(int slot, int count = -1)
    {
        if(slot>invSize || slot<0) 
        {
            Debug.LogError(name + " doesn't have slot number " + slot);
            return null;
        }
        if(count<-1 || count == 0)
        {
            Debug.LogError(name + " : " + count + " is a wrong count");
            return null;
        }
        if(inventory[slot] == null) return null;

        InvItem item = inventory[slot];

        if(count == -1 || (item.count - count)<=0)
        {
            inventory[slot] = null;
            return item;
        }
        else
        {
            item.count -= count;
            
            return InvItem.InitItem(item.item, this, count);
        }
    }
    
}
