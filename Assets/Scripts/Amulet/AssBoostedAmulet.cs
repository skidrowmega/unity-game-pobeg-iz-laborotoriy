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
}
