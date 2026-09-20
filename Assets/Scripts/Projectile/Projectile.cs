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
    public int PushStrength = 0;
    public Vector3 direction;
    public BulletType type;
    public DamageType damageType=DamageType.Normal;
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
            target.TakeDamage(damage, target.transform.position-direction, Shooter, PushStrength,damageType);
            Destroy(gameObject);
        }
    }

    public virtual void HitObject(Collider target)
    {
        //Destroy(target);
    }


    protected virtual void CheckForCollision()
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, Time.deltaTime * Speed + .5f)) return;
        Entity target = hit.transform.GetComponent<Entity>();
        if (target)
        HitEntity(target);
        else
        {

            HitObject(hit.collider);
        }
        if (type == BulletType.Ricochet)
        {
            direction = Vector3.Reflect(direction, hit.normal);
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
