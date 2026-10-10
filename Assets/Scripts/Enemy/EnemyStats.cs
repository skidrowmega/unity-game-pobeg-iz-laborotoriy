using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Roguelike/Enemy Stats")]
public class EnemyStats: ScriptableObject
{
    [Header("Базовые статы")]
    public float baseHP = 10f;
    public float baseDamage = 10f;
    public float baseSpeed = 5f;
    public float baseReload = 1f;
    public float baseAttackDistance = 1f;
    public float baseStopDistance = 1f;
    public float basePushForce = 0f;
    [Header("Статы Физика")]
    public float basePhysicistPushDamage = 5f;
    public float basePhysicistStanDamage = 10f;
    public float basePhysicistPushReload = 5f;
    public float basePhysicistStanReload = 7f;
    public float basePhysicistPushPushForce = 1f;
    public float basePhysicistStanStanTime = 0.5f;
    [Header("Статы Химика")]
    public float MolotovReload = 4f;
    public float MolotovDamage = 3f;
    [Header("Статы Математика")]
    public float baseMathematicianArithmeticDamage = 2f;
    public float baseMathematicianDivisionDamage = 20f;
    public float baseMathematicianArithmeticReload = 3f;
    public float baseMathematicianDivisionReload = 10f;
    public float baseMathematicianArithmeticDamageIncrease = 3f;
    public float baseAttackCooldown = 1f;
}
