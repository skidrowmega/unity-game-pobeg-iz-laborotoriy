using UnityEngine;

[CreateAssetMenu(fileName = "MesmerizingCrunchAmulet", menuName = "Roguelike/Amulets/MesmerizingCrunchAmulet")]

public class MesmerizingCrunchAmulet: AmuletSO
{
    public float timeModificator;
    public float MoreTime(float time)//исправить когда рисование доделается
    {
        return time * (1 + timeModificator);
    }
}
