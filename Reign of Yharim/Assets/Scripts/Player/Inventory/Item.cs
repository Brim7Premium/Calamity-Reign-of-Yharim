using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

//anybody please rename this
public class Item : MonoBehaviour
{
    public GameObject DefaultProjectilePrefab;
    private ItemData _item;
    public ItemData item
    {
        get => _item;
        set{ _item = value; SetDefaults();}
    }
    public void SetDefaults()
    {
        //DefaultProjectilePrefab = Resources.Load<GameObject>("");
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
        transform.localScale = item.size;
        transform.localPosition = item.size*2;
        if(item.Projectile != null)
        {
            Projectile.NewProjectile(item.Projectile, transform.position, Quaternion.identity, 0);
        }
        switch (item.actionType)
        {
            case ItemData.ActionType.Custom: break;
            case ItemData.ActionType.Swing: StartCoroutine(Swing(item.AtkSpeed, true)); break;
        }
    }
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
