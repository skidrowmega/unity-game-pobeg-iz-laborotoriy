using System.Collections.Generic;
using UnityEngine;

public class AmuletManager : MonoBehaviour
{
    public List<AmuletSO> equippedAmulets = new List<AmuletSO>();
    public int maxSlots = 3;

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
}