using UnityEngine;

[CreateAssetMenu(fileName = "WoodFlavoredProteinAmulet", menuName = "Roguelike/Amulets/WoodFlavoredProteinAmulet")]

public class WoodFlavoredProteinAmulet: AmuletSO
{
    public float regenModificator;
    public void Regen()
    {
        Player.player.HealthPoints += Player.player.MaxHealthpoints * regenModificator;
    }
}
