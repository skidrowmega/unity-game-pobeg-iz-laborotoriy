using System.Collections.Generic;
using UnityEngine;

public class AmuletManager : MonoBehaviour
{
    public List<AmuletSO> equippedAmulets = new List<AmuletSO>();
    public int maxSlots = 4;

    public static AmuletManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void EquipAmulet(AmuletSO newAmulet)
    {
        if (equippedAmulets.Count < maxSlots)
        {
            equippedAmulets.Add(newAmulet);
            newAmulet.OnEquip(gameObject);
        }
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