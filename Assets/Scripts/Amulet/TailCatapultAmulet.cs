using UnityEngine;

[CreateAssetMenu(fileName = "TailCatapultAmulet", menuName = "Roguelike/Amulets/TailCatapultAmulet")]

public class TailCatapultAmulet:AmuletSO
{
    public float grenadeFlightTimeModificator;
    private float baseTime;

    public override void OnEquip()
    {
        baseTime = Player.player.BasicFlightTime;

        Player.player.BasicFlightTime -= baseTime * (1 + grenadeFlightTimeModificator);
    }
    public override void OnUnequip()
    {
        Player.player.BasicFlightTime += baseTime * (1 + grenadeFlightTimeModificator);
    }

    public override string DescriptionCreate()
    {
        return $"{amuletName}\nУрон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nПолучаемый урон: x{1 - resistBuff}\n{amuletDescription}\nВремя полёта гранаты: х{1 + grenadeFlightTimeModificator}";
    }
}
