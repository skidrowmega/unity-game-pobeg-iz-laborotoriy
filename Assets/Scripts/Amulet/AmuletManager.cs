using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

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
        float finalMeleeDamage = baseMeleeDamage;

        foreach (AmuletSO amulet in equippedAmulets)
        {
            finalMeleeDamage = amulet.ModifyMeleeDamage(finalMeleeDamage);
        }

        return finalMeleeDamage;
    }

    public float GetModifiedRangedDamage(float baseRangedDamage)
    {
        float finalRangedDamage = baseRangedDamage;

        foreach (AmuletSO amulet in equippedAmulets)
        {
            finalRangedDamage = amulet.ModifyMeleeDamage(finalRangedDamage);
        }

        return finalRangedDamage;
    }

    public float GetDamageDecrease(float baseincomingDamage)
    {
        float finalincomingDamage = baseincomingDamage;

        foreach (AmuletSO amulet in equippedAmulets)
        {
            finalincomingDamage = amulet.ResistDamage(finalincomingDamage);
        }

        return finalincomingDamage;
    }

    public int GetPriceWithDiscount(int basePrice)
    {
        int finalPrice = basePrice;
        if (GetAmuletById(0))
        {
            foreach (AggressiveTradingAmulet amulet in equippedAmulets)
            {
                finalPrice = amulet.CalcPriceWithDiscount(finalPrice);
                print(finalPrice);
            }
        }
        return finalPrice;
    }

    public float GetDurabilityWithAmulet(float baseDurebility)
    {
        float finalDurebility = baseDurebility;
        if (GetAmuletById(6))
        {
            foreach (SalivaShieldAmulet amulet in equippedAmulets)
            {
                finalDurebility = amulet.MoreDurability(finalDurebility);
            }
        }
        return finalDurebility;
    }

    public void AttackSplinter()
    {
        if (GetAmuletById(6))
        {
            foreach (SplinterHurricaneAmulet amulet in equippedAmulets)
            {
                amulet.ShootSplinter();
            }
        }
    }
}