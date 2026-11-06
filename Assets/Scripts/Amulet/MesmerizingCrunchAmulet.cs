using UnityEngine;

[CreateAssetMenu(fileName = "WoodFlavoredProteinAmulet", menuName = "Roguelike/Amulets/WoodFlavoredProteinAmulet")]

public class MesmerizingCrunchAmulet: AmuletSO
{
    private float timeModificator;
    public float MoreTime(float time)
    {
        return time * (1 + timeModificator);
    }
}
