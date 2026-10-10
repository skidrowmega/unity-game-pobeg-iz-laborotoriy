using System;
using UnityEngine;


[Serializable]
public class WeaponRangedStats
{
    public WeaponRangedStats(WeaponRanged stampweapon)
    {
        WeaponName = stampweapon.WeaponName;
        Damage = stampweapon.Damage;
        PushStrength = stampweapon.PushStrength;
        maxDurability = stampweapon.MaxDurability;
        durability = stampweapon.Durability;
        durabilityDecrease = stampweapon.DurabilityDecrease;
        RicochetCount = stampweapon.RicochetCount;
        IsUsable = stampweapon.IsUsable;
        
    }
    public string WeaponName;
    public float Damage;
    public float PushStrength;
    public int maxDurability;
    public int durability;
    public int durabilityDecrease;
    public int RicochetCount;
    public bool IsUsable;
}

public class WeaponRanged: Weapon
{

    protected WeaponRanged()
    {
        Damage = 10;
        WeaponName = "Gun";
        PushStrength = 1;
    }
    public int ReloadingTime;
    public int RicochetCount=0;
    public GameObject projectileprefab;
    public bool IsUsable = true;
    public virtual void Shot(Vector3 Direction)
    {
        if (!IsUsable) return;
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.direction = Direction;
        bullet.Shooter = WeaponHolder;
        bullet.damage = Damage;
        bullet.PushStrength = PushStrength;
        bullet = Instantiate<Projectile>(bullet, WeaponHolder.transform.position + WeaponHolder.transform.right * 2, Quaternion.identity);
    }

    public void SwapStats(WeaponRangedStats stats)
    {
        WeaponName = stats.WeaponName;
        Damage = stats.Damage;
        PushStrength = stats.PushStrength;
        MaxDurability = stats.maxDurability;
        Durability = stats.durability;
        DurabilityDecrease = stats.durabilityDecrease;
        IsUsable = stats.IsUsable;
    }

}
