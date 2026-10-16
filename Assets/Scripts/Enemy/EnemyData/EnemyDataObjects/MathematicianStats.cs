using UnityEngine;

[CreateAssetMenu(fileName = "MathematicianStats", menuName = "Roguelike/Enemy Stats/Mathematician Stats")]
public class MathematicianStats: EnemyStats
{
    public float baseMathematicianArithmeticDamage = 2f;
    public float baseMathematicianDivisionDamage = 20f;
    public float baseMathematicianArithmeticReload = 3f;
    public float baseMathematicianDivisionReload = 10f;
    public float baseMathematicianArithmeticDamageIncrease = 3f;
    public float baseMathematicianDivisionDelay = 1.5f;
}
