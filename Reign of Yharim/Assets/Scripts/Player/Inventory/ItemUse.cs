using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Jobs;

//anybody please rename this
public abstract class ItemUse : MonoBehaviour
{
    private ItemData _item;
    public ItemData item
    {
        get => _item; 
        set { _item = value; SetDefaults();}
    }
    public virtual void SetDefaults()
    {   
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
        transform.localScale = item.size;
    }
    public abstract void Use();
    public void Update() => Use();

    public IEnumerator Swing(float speed, bool destroyOnEnd, float rotationAngle = 90)
    {
        float startAngle = transform.eulerAngles.z;
        do{
            transform.RotateAround(transform.parent.position, Vector3.forward * transform.parent.localScale.x, -speed * Time.deltaTime * 60);
            yield return new WaitForFixedUpdate();
        }while(Mathf.Abs(Mathf.DeltaAngle(startAngle, transform.eulerAngles.z))<rotationAngle);    
        if(destroyOnEnd) Destroy(gameObject);
    }
    public void OnDestroy()
    {
        transform.parent.GetComponent<PlayerAI>().IsAttacking = false;
    }
}
