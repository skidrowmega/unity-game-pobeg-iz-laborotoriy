using UnityEngine;

public abstract class WeaponRanged: MonoBehaviour
{
    public string WeaponName;
    public int Damage;
    public int PushStrength;
    public int ReloadingTime;
    public int EnergyDecrease;
    public int RicochetCount;
    public Entity WeaponHolder;
    public GameObject projectileprefab;
    public virtual void Shot(Vector3 Direction)
    {
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.direction = Direction;
        bullet.Shooter = WeaponHolder;
        bullet.damage = Damage;
        bullet.PushStrength = PushStrength;
        bullet = Instantiate<Projectile>(bullet, WeaponHolder.transform.position + WeaponHolder.transform.right * 2, Quaternion.identity);
    }
    private void Awake()
    {
        WeaponHolder = transform.parent.GetComponent<Entity>();
    }

}
