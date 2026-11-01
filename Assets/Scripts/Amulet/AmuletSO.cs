using System;
using UnityEngine;


public class AmuletSO: ScriptableObject
{
    public string amuletName;
    public string amuletDescription;
    public Sprite amuletImage;
    public float resistBuff;
    public float damageMeleeBuff;
    public float damageRangedBuff;

    public virtual void OnEquip(GameObject player) { }
    public virtual void OnUnequip(GameObject player) { }


    public float ModifyMeleeDamage(float Damage)
    {
        return Damage * (1 + damageMeleeBuff);
    }

    public float ModifyRangedDamage(float Damage)
    {
        return Damage * (1 + damageRangedBuff);
    }

    public float ResistDamage(float incomingDamage)
    {
        return incomingDamage * (1 - resistBuff);
    }

}
