using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphAI : Projectile
{

    public override void SetDefaults()
    {
        projName = "TelegraphProjectile";
        damage = 0;
        //objectRenderer = GetComponent<Renderer>();
    }
    /*public override void AI()
    {
        if (IsVisibleFromCamera())
        {
            // Enable rendering if the object is visible
            objectRenderer.enabled = true;
        }
        else
        {
            objectRenderer.enabled = false;
        }
    }*/
}
