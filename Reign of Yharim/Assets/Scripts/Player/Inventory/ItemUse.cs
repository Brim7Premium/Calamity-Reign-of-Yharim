using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Jobs;

public abstract class ItemUse : MonoBehaviour
{
    public ItemData item;
    public virtual IEnumerator Start()
    {   
        //Until item set
        while(item == null) yield return new WaitForFixedUpdate();
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
    public abstract void Use();
    public void Update() => Use();

    public IEnumerator Swing(float speed)
    {
        Vector3 pos = transform.position;
        do{
            transform.RotateAround(transform.parent.position, Vector3.forward * transform.parent.localScale.x, -speed * Time.deltaTime * 60);
            yield return new WaitForFixedUpdate();
            print(transform.eulerAngles.z);
        }while(transform.eulerAngles.z>270 || transform.eulerAngles.z<90);    
        Destroy(gameObject);
    }
}
