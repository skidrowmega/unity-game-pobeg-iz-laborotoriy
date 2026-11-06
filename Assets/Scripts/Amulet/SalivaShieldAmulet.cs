using UnityEngine;

[CreateAssetMenu(fileName = "SalivaShieldAmulet", menuName = "Roguelike/Amulets/SalivaShieldAmulet")]

public class SalivaShieldAmulet: AmuletSO
{
    private float durabilityModificator;
    public float MoreDurability(float durability)
    {
        return durability * (1 + durabilityModificator);
    }
}
