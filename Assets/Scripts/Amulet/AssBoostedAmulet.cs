using UnityEngine;

[CreateAssetMenu(fileName = "AssBoostedAmulet", menuName = "Roguelike/Amulets/AssBoostedAmulet")]
public class AssBoostedAmulet: AmuletSO
{
    public float speedBuff;
    public override void OnEquip()
    {
        Player.player.Speed += speedBuff * Player.player.Speed;
    }

    public override void OnUnequip()
    {
        Player.player.Speed -= speedBuff * Player.player.Speed;
    }
}
