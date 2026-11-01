using System.Collections.Generic;
using UnityEngine;

public class AmuletManager : MonoBehaviour
{
    public List<AmuletSO> equippedAmulets = new List<AmuletSO>();

    public int maxSlots = 4;

    public void EquipAmulet(AmuletSO newAmulet)
    {
        if (equippedAmulets.Count < maxSlots)
        {
            equippedAmulets.Add(newAmulet);
            newAmulet.OnEquip(gameObject);
        }
    }


    public float GetModifiedDamage(float baseDamage)
    {
        float finalDamage = baseDamage;

        foreach (AmuletSO amulet in equippedAmulets)
        {
            finalDamage = amulet.ModifyDamage(finalDamage);
        }

        return finalDamage;
    }

    public float GetDamageDecrease(float baseincomingDamage)
    {
        float finalincomingDamage = baseincomingDamage;

        foreach (AmuletSO amulet in equippedAmulets)
        {
            finalincomingDamage = amulet.ModifyDamage(finalincomingDamage);
        }

        return finalincomingDamage;
    }
}