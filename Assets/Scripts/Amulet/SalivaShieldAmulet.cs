using UnityEngine;

[CreateAssetMenu(fileName = "SalivaShieldAmulet", menuName = "Roguelike/Amulets/SalivaShieldAmulet")]

public class SalivaShieldAmulet: AmuletSO
{
    public float durabilityModificator;
    public float MoreDurability(float durability)//исправить когда рисование доделается
    {
        return durability * (1 + durabilityModificator);
    }

    public override string DescriptionCreate()
    {
        return $"{amuletName}\nУрон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nПолучаемый урон: x{1 - resistBuff}\n{amuletDescription}\nПрочность оружия: х{1 + durabilityModificator}";
    }
}
