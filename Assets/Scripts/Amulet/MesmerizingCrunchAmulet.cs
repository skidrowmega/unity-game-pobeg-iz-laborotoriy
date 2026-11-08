using UnityEngine;

[CreateAssetMenu(fileName = "WoodFlavoredProteinAmulet", menuName = "Roguelike/Amulets/WoodFlavoredProteinAmulet")]

public class MesmerizingCrunchAmulet: AmuletSO
{
    public float timeModificator;
    public float MoreTime(float time)//исправить когда рисование доделается
    {
        return time * (1 + timeModificator);
    }
}
