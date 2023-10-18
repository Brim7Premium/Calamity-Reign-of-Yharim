using System;
using System.Collections;
using UnityEngine;

public abstract class Projectile : Entity //Must be inherited, cannot be instanced 
{
    public string projName;
    public GameObject target;
    public float timeLeft = 10;
    public float[] ai = new float[4];
    public int damage;
    public float knockback;
    public float velocity;
    public Rigidbody2D rb;
    IEnumerator death(float liveExpectancy)
    {
        yield return new WaitForSeconds(liveExpectancy);
        Destructor();
    }
    void Awake()
    {
        StartCouroutine(death(timeLeft));
    }
    public void Update()
    {
        AI();
        objectRenderer.enabled = IsVisibleFromCamera();
    }
    public static Projectile NewProjectile(GameObject _projectile, Vector2 _position, Quaternion _rotation, float _velocity, int _damage, float _knockback, string _parent = "", float _ai0 = 0, float _ai1 = 0, float _ai2 = 0, float _ai3 = 0)
    {   
        GameObject projGameObject = Instantiate(projectile, position, rotation, GameObject.Find(_parent).transform); 

        Projectile proj = projGameObject.GetComponent<Projectile>();
        proj.SetDefaults();
        proj.damage = _damage;
        proj.knockback = _knockback;
        proj.velocity = _velocity;
        proj.ai = new float[] {_ai0, _ai1, _ai2, _ai3};

        return proj;
    }

    public static Projectile NewProjectile(GameObject _projectile, Vector2 _position, Quaternion _rotation, Vector2 _velocity, int _damage, float _knockback, string _parent = "", float _ai0 = 0, float _ai1 = 0, float _ai2 = 0, float _ai3 = 0)
    {
        GameObject projGameObject = Instantiate(projectile, position, rotation, GameObject.Find(_parent).transform); 

        Projectile proj = projGameObject.GetComponent<Projectile>();
        proj.SetDefaults();
        proj.damage = _damage;
        proj.knockback = _knockback;
        proj.rb.velocity = _velocity;
        proj.ai = new float[] {_ai0, _ai1, _ai2, _ai3};

        return proj;
    }
    public static Projectile GetProjectile(GameObject gameObject)
    {
        return gameObject.GetComponent<Projectile>();
    }
    public virtual void AI(){}
    public virtual void OnHit(Collision2D col) => Destroy(gameObject);
    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f));
    void OnCollisiontStay(Collision2D col)
    {
        OnHit(col);
    }

     public int GetTargetDirectionX() => transform.position.x < target.transform.position.x ? 1 : -1; 

    public float GetDistanceToTarget()
    {
        return Vector2.Distance(gameObject.transform.position, target.transform.position);
    }
}
