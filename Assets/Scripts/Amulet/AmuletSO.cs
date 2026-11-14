using System;
using Unity.VisualScripting;
using UnityEngine;


public class AmuletSO: ScriptableObject
{
    public string amuletName;
    public string amuletDescription;
    public Sprite amuletImage;
    public float resistBuff;
    public float damageMeleeBuff;
    public float damageRangedBuff;

    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }

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

    public string DescriptionCreate()
    {
        return $"Урон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nЗащита от урона: x{1 + resistBuff}\n{amuletDescription}";
    }
}
