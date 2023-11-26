using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptible Object/Item")] 
public class Item : ScriptableObject
{
    [Header("In-Game Data")]
    public ItemType type;
    public ActionType actionType;
    public bool consumable;

    [Header("UI Data")]
    public bool stackable = true;
    public string displayName;

    [Header("Both")]
    public Sprite image;
    public float ID;
}

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
    Attack,
    Use,
    Throw,
    Shoot
}
