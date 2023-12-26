using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindBlade : ItemUse
{
    public GameObject WindBladeProj;
    public override void SetDefaults()
    {
        //sets basic variables, such as texture
        base.SetDefaults();

        //idk should be this included in ItemData
        int AtkSpeed = 5;
        bool DestroyOnEnd = true;
        transform.localPosition = Vector2.one;
        StartCoroutine(Swing(AtkSpeed, DestroyOnEnd));

        
    }

    public override void Use()
    {
        
    }
    
}
