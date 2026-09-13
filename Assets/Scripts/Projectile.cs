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
    public Vector3 direction;
    public BulletType type;
    private void Awake()
    {
        //direction = transform.forward;
    }
    public virtual void HitEntity(Entity target)
    {
        Destroy(this);
    }

    public virtual void HitObject()
    {
        Destroy(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target)
        {
            HitEntity(target);
        } else HitObject();
    }
}
