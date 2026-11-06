using UnityEngine;

[CreateAssetMenu(fileName = "AssBoostedAmulet", menuName = "Roguelike/Amulets/AssBoostedAmulet")]
public class AssBoostedAmulet: AmuletSO
{
    private float speedBuff;
    public override void OnEquip(GameObject player)
    {
        Player.player.Speed += speedBuff * Player.player.Speed;
    }

    public override void OnUnequip(GameObject player)
    {
        Player.player.Speed -= speedBuff * Player.player.Speed;
    }
}
