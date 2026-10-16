using UnityEngine;

[CreateAssetMenu(fileName = "ChemistStats", menuName = "Roguelike/Enemy Stats/Chemist Stats")]
public class ChemistStats: EnemyStats
{
    public float baseChemistMolotovReload = 4f;
    public float baseChemistMolotovDamage = 3f;
}
