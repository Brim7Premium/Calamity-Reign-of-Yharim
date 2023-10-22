using System;
using System.Collections;
using UnityEngine;

public abstract class Projectile : Entity //Must be inherited, cannot be instanced 
{
    public string projName;
    public GameObject target;
    
    public float[] ai = new float[4];
    private int _damage;
    public int damage
    {
        get { return _damage;}
        set { _damage = value < 0 ? _damage : value;} //I think it's very unlikely that we gonna use negative values here. 
    }

    private float _knockback;
    public float knockback
    {
        get { return _knockback;}
        set { _knockback = value < 0 ? _knockback : value;} 
    }

    private float _timeLeft = 2;
    
    public float timeLeft
    {
        get { return _timeLeft;}
        set { _timeLeft = value < 0 ? _timeLeft : value;}
    }
    public float velocity = -1;
    public Rigidbody2D rb;

    IEnumerator Death()
    {
        //Just in case if we ever need to extend projectile livespan
        while(timeLeft > 1)
        {
            yield return new WaitForSeconds(1); 
            timeLeft -= 1;
        }

        yield return new WaitForSeconds(timeLeft);
        Destroy(gameObject);
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        StartCoroutine(Death());
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        AI();
        objectRenderer.enabled = IsVisibleFromCamera();
    }
    //-1 will set parametres to their default value in class. 
    public static Projectile NewProjectile(GameObject _projectile, Vector2 _position, Quaternion _rotation, float _velocity = -1, int _damage = -1, float _knockback = -1, float _timeLeft = -1, float _ai0 = 0, float _ai1 = 0, float _ai2 = 0, float _ai3 = 0, string _parent = "")
    {   
        GameObject projGameObject;

        if(_parent != ""){projGameObject = Instantiate(_projectile, _position, _rotation, GameObject.Find(_parent).transform);}
        else{projGameObject = Instantiate(_projectile, _position, _rotation);}

        Projectile proj = projGameObject.GetComponent<Projectile>();

        proj.SetDefaults();

        proj.damage = _damage;
        proj.knockback = _knockback;
        proj.velocity = _velocity;
        proj.ai = new float [] {_ai0, _ai1, _ai2, _ai3};
        proj.timeLeft = _timeLeft;

        return proj;
    }

    public static Projectile NewProjectile(GameObject _projectile, Vector2 _position, Quaternion _rotation, Vector2 _velocity, int _damage = -1, float _knockback = -1, float _timeLeft = -1, float _ai0 = 0, float _ai1 = 0, float _ai2 = 0, float _ai3 = 0, string _parent = "")
    {
        //Making sure all changes to the previous overload apply to that one as well
        Projectile proj = NewProjectile(_projectile, _position, _rotation, -1, _damage, _knockback, _timeLeft, _ai0 , _ai1, _ai2, _ai3, _parent);

        proj.rb.velocity = _velocity;

        return proj;
    }
    public static Projectile GetProjectile(GameObject gameObject)
    {
        return gameObject.GetComponent<Projectile>();
    }
    public virtual void AI(){}
    public virtual void OnHit(Collision2D col) => Destroy(gameObject);
    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f));
    public void DrawDistanceToPlayer(Color color) => Debug.DrawLine(gameObject.transform.position, target.transform.position, color);
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
