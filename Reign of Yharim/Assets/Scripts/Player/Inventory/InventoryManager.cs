using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InvItem[] inventory;
    public InventoryManager[] pages;
    public Transform[] slots;
    public ItemData.InventoryType StoringType;
    public int invSize;
    public Transform GUI;
    [SerializeField] private GameObject InvItem; 
    [SerializeField] private GameObject InvSlot;
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
            InvSlot slot = Instantiate(InvSlot, GUI).GetComponent<InvSlot>();
            slot.number = i;
            slot.inventoryManager = this;
            slots[i] = slot.transform;
        }
    }
    //Adds in the first availablt slot or specified slot
    //First tries all universal inventories, then specific
    public bool AddItem(ItemData item, int count = 1)
    {
        bool AddItem()
        {
            for(int i = 0; i<invSize; i++)
            {
                if(inventory[i] == null)//if no item in slot
                {
                    inventory[i] = Instantiate(InvItem, slots[i]).GetComponent<InvItem>();
                    inventory[i].InitItem(item, this, count);
                    return true;
                }
                else if(inventory[i].item == item && item.stackable)//if the same item in slot
                {
                    inventory[i].count += count;
                    return true;
                }
            }

            return false;
        }



        if(StoringType == ItemData.InventoryType.All)
        {   

            if(AddItem()) return true;
        }

        foreach(InventoryManager page in pages)
        {   
            if(page.StoringType == ItemData.InventoryType.All) 
            {
                if(page.AddItem(item, count)) return true;
            }
        }



        if(StoringType == item.inventoryType)
        {
            if(AddItem()) return true;
        }

        foreach(InventoryManager page in pages)
        {
            if(page.StoringType == item.inventoryType) 
            {
                if(page.AddItem(item, count)) return true;
            }
        }




        return false;
    }
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
            InvItem newItem = Instantiate(InvItem, slots[slot]).GetComponent<InvItem>();
            newItem.InitItem(item.item, this);
            return newItem;
        }
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
}
