using UnityEngine;

[CreateAssetMenu(fileName = "AssBoostedAmulet", menuName = "Roguelike/Amulets/AssBoostedAmulet")]
public class AssBoostedAmulet: AmuletSO
{
    public float speedBuff;
    private float basespeed;

    public override void OnEquip()
    {
        basespeed = Player.player.Speed;

        Player.player.Speed += speedBuff * basespeed;
    }

    public override void OnUnequip()
    {
        Player.player.Speed -= speedBuff * basespeed;
    }
}
