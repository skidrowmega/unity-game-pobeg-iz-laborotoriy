using UnityEngine;

public abstract class Projectile: MonoBehaviour
{
    public int Speed;
    public const int LifeTime = 10;
    public virtual void HitEntity()
    {
        
    }

    public virtual void HitObject()
    {

    }
}
