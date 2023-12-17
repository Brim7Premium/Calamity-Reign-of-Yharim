using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptible Object/Item")] 
public class ItemData : ScriptableObject
{
    [Header("In-Game Data")]
    public ItemType2 type;
    public ActionType2 actionType;
    public bool consumable;
    public Vector2 size;

    [Header("UI Data")]
    public bool stackable = true;
    public string displayName;

    [Header("Both")]
    public Sprite sprite;
    public float ID;
}

public enum ItemType2
{
    Tool, 
    MeleeWeapon, 
    RangedWeapon,
    MagicWeapon,
    SummonWeapon,
    RogueWeapon,
    Generic
}
public enum ActionType2
{
    Attack,
    Use,
    Throw,
    Shoot
}
