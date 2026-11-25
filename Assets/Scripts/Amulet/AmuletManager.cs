using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmuletManager : MonoBehaviour
{
    public List<AmuletSO> equippedAmulets = new List<AmuletSO>();
    public int maxSlots = 3;
    [SerializeField] public AmuletSO[] AllAmulets;

    public static AmuletManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool TryEquip()
    {
        if (equippedAmulets.Count < maxSlots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EquipAmulet(AmuletSO newAmulet)
    {
        equippedAmulets.Add(newAmulet);
        newAmulet.OnEquip();
    }

    public void ChangeAmulet(AmuletSO oldAmulet, AmuletSO newAmulet)
    {
        RemoveAmulet(oldAmulet);
        equippedAmulets.Add(newAmulet);
        newAmulet.OnEquip();
    }

    public void RemoveAmulet(AmuletSO Amulet)
    {
        equippedAmulets.Remove(Amulet);
        Amulet.OnUnequip();
    }

    public bool IsAmuletOnBobr(AmuletSO amuletdata)
    {
        foreach (AmuletSO amulet in equippedAmulets)
        {
            if (amuletdata.amuletName == amulet.amuletName)
            {
                return true;
            }
        }
        return false;
    }

    public AmuletSO GetAmuletById(int targetId)
    {
        AmuletSO foundamulet = Array.Find(AllAmulets, item => item != null && item.ID == targetId);
        return foundamulet;
    }

    public float GetModifiedMeleeDamage(float baseMeleeDamage)
    {
        foreach (AmuletSO amulet in equippedAmulets)
        {
            baseMeleeDamage = amulet.ModifyMeleeDamage(baseMeleeDamage);
        }

        return baseMeleeDamage;
    }

    public float GetModifiedRangedDamage(float baseRangedDamage)
    {
        foreach (AmuletSO amulet in equippedAmulets)
        {
            baseRangedDamage = amulet.ModifyMeleeDamage(baseRangedDamage);
        }

        return baseRangedDamage;
    }

    public float GetDamageDecrease(float baseincomingDamage)
    {
        foreach (AmuletSO amulet in equippedAmulets)
        {
            baseincomingDamage = amulet.ResistDamage(baseincomingDamage);
        }

        return baseincomingDamage;
    }

    public int GetPriceWithDiscount(int basePrice)
    {
        if (IsAmuletOnBobr(GetAmuletById(0)))
        {
            foreach (AggressiveTradingAmulet amulet in equippedAmulets.OfType<AggressiveTradingAmulet>())
            {
                basePrice = amulet.CalcPriceWithDiscount(basePrice);
            }
        }
        return basePrice;
    }

    public float GetDurabilityWithAmulet(float baseDurebility)
    {
        if (IsAmuletOnBobr(GetAmuletById(6)))
        {
            foreach (SalivaShieldAmulet amulet in equippedAmulets.OfType<SalivaShieldAmulet>())
            {
                baseDurebility = amulet.MoreDurability(baseDurebility);
            }
        }
        return baseDurebility;
    }

    public void AttackSplinter()
    {
        if (IsAmuletOnBobr(GetAmuletById(7)))
        {
            foreach (SplinterHurricaneAmulet amulet in equippedAmulets.OfType<SplinterHurricaneAmulet>())
            {
                amulet.ShootSplinter();
            }
        }
    }
    public float GetMoreTime(float time)
    {
        if (IsAmuletOnBobr(GetAmuletById(10)))
        {
            foreach (MesmerizingCrunchAmulet amulet in equippedAmulets.OfType<MesmerizingCrunchAmulet>())
            {
                time = amulet.MoreTime(time);
            }
        }
        return time;
    }

    public float GetDamageForHitpointBuff(float baseDamage)
    {
        if (IsAmuletOnBobr(GetAmuletById(3)))
        {
            foreach (BeavRage amulet in equippedAmulets.OfType<BeavRage>())
            {
                baseDamage = amulet.CalcDamageForHP(baseDamage);
            }
        }
        return baseDamage;
    }

    public void CreateNuclearCrunch(float Damage)
    {
        if (IsAmuletOnBobr(GetAmuletById(5)))
        {
            foreach (NuclearCrunchAmulet amulet in equippedAmulets.OfType<NuclearCrunchAmulet>())
            {
                amulet.Explosion(Damage);

            }
        }
    }

    public void CreateExplosionWithDodge()
    {
        if (IsAmuletOnBobr(GetAmuletById(11)))
        {
            foreach (TailDropper amulet in equippedAmulets.OfType<TailDropper>())
            {
                amulet.ExplosionWithDodge();

            }
        }
    }
}