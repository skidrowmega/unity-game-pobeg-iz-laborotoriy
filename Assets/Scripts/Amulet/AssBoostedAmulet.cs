using UnityEngine;

[CreateAssetMenu(fileName = "AssBoostedAmulet", menuName = "Roguelike/Amulets/AssBoostedAmulet")]
public class AssBoostedAmulet: AmuletSO
{
    public float speedBuff;
    private float basespeed;
    private void Awake()
    {
        basespeed = Player.player.Speed;
    }
    public override void OnEquip()
    {
        Player.player.Speed += speedBuff * basespeed;
    }

    public override void OnUnequip()
    {
        Player.player.Speed -= speedBuff * basespeed;
    }
}
