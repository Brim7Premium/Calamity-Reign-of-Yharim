using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class InvSlot : MonoBehaviour
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
        //shows this item on the back
        if (gameObject.transform.childCount > 0 && GameObject.Find("Player") != null)
        {
            GameObject.Find("/Player/Item").transform.GetComponent<SpriteRenderer>().sprite = gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        }

        else if (gameObject.transform.childCount == 0 && GameObject.Find("Player") != null)
        {
            GameObject.Find("/Player/Item").transform.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    public void Deselect()
    {
        image.sprite = notSelectedSprite;
    }
}
