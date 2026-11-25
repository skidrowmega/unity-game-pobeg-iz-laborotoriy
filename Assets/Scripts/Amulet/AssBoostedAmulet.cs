using UnityEngine;

[CreateAssetMenu(fileName = "AssBoostedAmulet", menuName = "Roguelike/Amulets/AssBoostedAmulet")]
public class AssBoostedAmulet: AmuletSO
{
    public float speedBuff;
    private float basespeed;

    public override void OnEquip()
    {
        basespeed = Player.player.baseSpeed;

        Player.player.baseSpeed += speedBuff * basespeed;
    }

    public override void OnUnequip()
    {
        Player.player.baseSpeed -= speedBuff * basespeed;
    }

    public override string DescriptionCreate()
    {
        return $"{amuletName}\nУрон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nПолучаемый урон: x{1 - resistBuff}\n{amuletDescription}\nСкорость бобра: х{1 + speedBuff}";
    }
}
