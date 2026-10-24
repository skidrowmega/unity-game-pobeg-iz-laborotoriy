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
    public float maxDurability;
    public float durability;
    public float durabilityDecrease;
    public int RicochetCount;
    public bool IsUsable;

    public WeaponRangedStats(WeaponRangedStats stats)
    {
        WeaponName = stats.WeaponName;
        Damage = stats.Damage;
        PushStrength = stats.PushStrength;
        maxDurability = stats.maxDurability;
        durability = stats.durability;
        durabilityDecrease = stats.durabilityDecrease;
        RicochetCount = stats.RicochetCount;
        IsUsable = stats.IsUsable;

    }

    public WeaponRangedStats ApplyMultiplier(float multiplier)
    {
        WeaponRangedStats newstats = new WeaponRangedStats(this);
        newstats.Damage *= multiplier;
        newstats.PushStrength *= multiplier;
        newstats.maxDurability = (int)(maxDurability * multiplier);
        newstats.durability = newstats.maxDurability;
        newstats.RicochetCount= (int)(newstats.RicochetCount * multiplier);
        return newstats;
    }

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
    public void BreakWeapon()
    {
        Destroy(weaponReference);
        weaponReference = null;
        OnBreak();
    }
    public GameObject PlaceWeapon(GameObject weapontoPlace, WeaponRangedStats newstats)
    {
        if (weaponReference != null) return null;
        weaponReference = Instantiate(weapontoPlace, HandReference.transform);
        weaponReference.transform.localPosition = Vector3.zero;
        weaponReference.transform.localRotation = Quaternion.Euler(0, 0, 180);
        SwapStats(newstats);
        return weaponReference;
    }
}
