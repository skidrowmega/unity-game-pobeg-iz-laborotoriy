using UnityEngine;

public enum BulletType
{
    Normal,
    Ricochet
}

public abstract class Projectile: MonoBehaviour
{
    public int Speed = 1;
    public const int LifeTime = 10;
    public int damage = 1;
    public int PushTime = 0;
    public Vector3 direction;
    public BulletType type;
    public Entity Shooter;
    private float timestart;

    private void Awake()
    {
        timestart=Time.time;
    }
    public virtual void HitEntity(Entity target)
    {
        if (target != Shooter)
        {
            target.TakeDamage(damage, transform.position, Shooter, PushTime);
            Destroy(gameObject);
        }
    }

    public virtual void HitObject(Collider target)
    {
        //Destroy(target);
    }


    protected virtual void CheckForCollision()
    {
        foreach (Collider hit in Physics.OverlapSphere(transform.position, 0.05f))
        {
            Entity target = hit.gameObject.GetComponent<Entity>();
            if (target)
            {
                HitEntity(target);
            }
            else HitObject(hit);
        }
    }
    protected virtual void MoveBullet()
    {
        transform.position += direction.normalized * Speed * Time.deltaTime;
    }

    private void Update()
    {
        CheckForCollision();
        MoveBullet();
        if (Time.time - timestart > LifeTime) Destroy(gameObject);
    }
}
