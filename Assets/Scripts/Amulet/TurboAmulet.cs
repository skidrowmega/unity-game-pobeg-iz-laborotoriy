using UnityEngine;

[CreateAssetMenu(fileName = "TurboAmulet", menuName = "Roguelike/Amulets/TurboAmulet")]
public class TurboAmulet: AmuletSO
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
