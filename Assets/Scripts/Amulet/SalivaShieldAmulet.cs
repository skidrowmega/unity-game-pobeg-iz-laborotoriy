using UnityEngine;

[CreateAssetMenu(fileName = "SalivaShieldAmulet", menuName = "Roguelike/Amulets/SalivaShieldAmulet")]

public class SalivaShieldAmulet: AmuletSO
{
    public float durabilityModificator;
    public float MoreDurability(float durability)//исправить когда рисование доделается
    {
        return durability * (1 + durabilityModificator);
    }
}
