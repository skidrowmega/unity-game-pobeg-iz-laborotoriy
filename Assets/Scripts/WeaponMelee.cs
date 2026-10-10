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
}

public class WeaponMelee: Weapon
{
    public DamageType Type;
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
