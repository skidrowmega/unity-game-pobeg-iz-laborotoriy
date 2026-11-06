using UnityEngine;

[CreateAssetMenu(fileName = "TailCatapultAmulet", menuName = "Roguelike/Amulets/TailCatapultAmulet")]

public class TailCatapultAmulet:AmuletSO
{
    public float grenadeFlightTimeModificator;
    public override void OnEquip()
    {
        Player.player.BasicFlightTime += Player.player.BasicFlightTime * (1 + grenadeFlightTimeModificator);
    }
    public override void OnUnequip()
    {
        Player.player.BasicFlightTime -= Player.player.BasicFlightTime * (1 + grenadeFlightTimeModificator);
    }
}
