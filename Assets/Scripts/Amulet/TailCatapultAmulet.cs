using UnityEngine;

[CreateAssetMenu(fileName = "TailCatapultAmulet", menuName = "Roguelike/Amulets/TailCatapultAmulet")]

public class TailCatapultAmulet:AmuletSO
{
    public float grenadeFlightTimeModificator;
    private float baseTime;
    public void Awake()
    {
        baseTime = Player.player.BasicFlightTime;
    }
    public override void OnEquip()
    {
        Player.player.BasicFlightTime -= baseTime * (1 + grenadeFlightTimeModificator);
    }
    public override void OnUnequip()
    {
        Player.player.BasicFlightTime += baseTime * (1 + grenadeFlightTimeModificator);
    }
}
