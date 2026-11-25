using UnityEngine;

[CreateAssetMenu(fileName = "MesmerizingCrunchAmulet", menuName = "Roguelike/Amulets/MesmerizingCrunchAmulet")]

public class MesmerizingCrunchAmulet: AmuletSO
{
    public float timeModificator;
    public float MoreTime(float time)//исправить когда рисование доделается
    {
        return time * (1 + timeModificator);
    }

    public override string DescriptionCreate()
    {
        return $"{amuletName}\nУрон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nПолучаемый урон: x{1 - resistBuff}\n{amuletDescription}\nВремя для выгрызания оружия: х{1 + timeModificator}";
    }
}
