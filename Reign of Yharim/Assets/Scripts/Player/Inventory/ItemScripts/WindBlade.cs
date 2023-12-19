using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindBlade : ItemUse
{
    // Start is called before the first frame update
    public override IEnumerator Start()
    {
        StartCoroutine(base.Start());
        gameObject.transform.localScale = Vector2.one * 0.5f;
        gameObject.transform.localPosition = Vector2.one;
        StartCoroutine(Swing(5));
        yield return new WaitForFixedUpdate();
    }

    // Update is called once per frame
    public override void Use()
    {
        
    }
    public void OnDestroy()
    {
        transform.parent.GetComponent<PlayerAI>().IsAttacking = false;
    }
}
