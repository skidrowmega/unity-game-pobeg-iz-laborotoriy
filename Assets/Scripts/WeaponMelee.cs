using System;
using UnityEngine;

[Serializable]
public class WeaponMeleeStats
{
    public WeaponMeleeStats(WeaponMelee stampweapon)
    {
        WeaponName = stampweapon.WeaponName;
        Damage = stampweapon.Damage;
        PushStrength = stampweapon.PushStrength;
        maxDurability = stampweapon.MaxDurability;
        durability = stampweapon.Durability;
        durabilityDecrease = stampweapon.DurabilityDecrease;
        Maxstamina = stampweapon.Maxstamina;
        stamina = stampweapon.Stamina;
        staminaRecoveryRate = stampweapon.StaminaRecoveryRate;
        staminaDecreaseRate = stampweapon.StaminaDecreaseRate;
    }

    public WeaponMeleeStats(WeaponMeleeStats weaponmeleestats)
    {
        WeaponName = weaponmeleestats.WeaponName;
        Damage = weaponmeleestats.Damage;
        PushStrength = weaponmeleestats.PushStrength;
        maxDurability = weaponmeleestats.maxDurability;
        durability = weaponmeleestats.durability;
        durabilityDecrease = weaponmeleestats.durabilityDecrease;
    }

    public string WeaponName;
    public float Damage;
    public float PushStrength;
    public int maxDurability;
    public int durability;
    public int durabilityDecrease;
    public int Maxstamina;
    public int stamina;
    public int staminaRecoveryRate;
    public int staminaDecreaseRate;

    public WeaponMeleeStats ApplyMultiplier(float multiplier)
    {
        WeaponMeleeStats newstats = new WeaponMeleeStats(this);
        newstats.Damage *= multiplier;
        newstats.PushStrength *= multiplier;
        newstats.maxDurability = (int)(maxDurability * multiplier);
        newstats.durability = newstats.maxDurability;
        return newstats;
    }

}

public class WeaponMelee: Weapon
{
    public DamageType Type;
    [SerializeField] protected GameObject tempNewWeapon;
    [SerializeField] protected WeaponMeleeStats tempstats;
    [SerializeField] protected WeaponMeleeStats statsonbreak;
    protected WeaponMelee()
    {
        WeaponName = "Oar";
        Damage = 10;
        PushStrength = 5;
        Type = DamageType.Normal;
    }

    protected WeaponMelee(DamageType type, int maxDurability, int durability, int durabilityDecrease,int maxstamina, int stamina, int staminaRecoveryRate, int staminaDecreaseRate)
    {
        Type = type;
        MaxDurability = maxDurability;
        Durability = durability;
        DurabilityDecrease = durabilityDecrease;
        Maxstamina = maxstamina;
        Stamina = stamina;
        StaminaRecoveryRate = staminaRecoveryRate;
        StaminaDecreaseRate = staminaDecreaseRate;
    }
    public void BreakWeapon()
    {
        tempstats = new WeaponMeleeStats(this);
        SwapStats(statsonbreak);
        OnBreak();
    }
    protected override void OnBreak()
    {
        Destroy(weaponReference);
        weaponReference = null;
        base.OnBreak();
    }
    public GameObject PlaceWeapon(GameObject weapontoPlace, WeaponMeleeStats newstats)
    {
        if (weaponReference != null) return null;
        weaponReference = Instantiate(weapontoPlace, HandReference.transform);
        weaponReference.transform.localPosition = Vector3.zero;
        weaponReference.transform.localRotation = Quaternion.Euler(0,0,180);
        SwapStats(newstats);
        return weaponReference;
    }

    public void SwapStats(WeaponMeleeStats stats)
    {
        WeaponName = stats.WeaponName;
        Damage = stats.Damage ;
        PushStrength = stats.PushStrength ;
        MaxDurability = stats.maxDurability ;
        Durability = stats.durability ;
        DurabilityDecrease = stats.durabilityDecrease;
        Maxstamina = stats.Maxstamina;
        Stamina = stats.stamina ;
        StaminaRecoveryRate = stats.staminaRecoveryRate;
        StaminaDecreaseRate = stats.staminaDecreaseRate;
    }

    [SerializeField] public int Maxstamina = 100;
    [SerializeField] public int Stamina=100;
    [SerializeField] public int StaminaRecoveryRate=20;
    [SerializeField] public int StaminaDecreaseRate=20;
    
    public virtual void OnHit(Entity entity, Vector3 source) {
        entity.TakeDamage(Damage, WeaponHolder.transform.position, WeaponHolder, PushStrength, Type);
    }
}
