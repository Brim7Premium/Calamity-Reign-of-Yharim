using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptible Object/Item")] 
public class ItemData : ScriptableObject
{
    [Header("Wepon stats")]
    public ItemType type;
    public ActionType actionType;
    public bool consumable = false;
    public float damage = 1;
    public float knockback = 0;
    public float AtkSpeed = 2;
    public int AtkType = 0;

    [Header("Other")]
    public Vector2 size = Vector2.one*0.5f;
    public GameObject Projectile = null;
    public string Script = "Item";

    [Header("UI Data")]
    public bool stackable = true;
    public string displayName;

    [Header("Both")]
    public Sprite sprite;
    public float ID;

    public enum ItemType
    {
        Tool, 
        MeleeWeapon, 
        RangedWeapon,
        MagicWeapon,
        SummonWeapon,
        RogueWeapon,
        Generic
    }
    public enum ActionType
    {
        Custom,
        Swing,
        Throw
    }
}


