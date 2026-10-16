using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyStats", menuName = "Roguelike/Enemy Stats/Base Enemy Stats")]
public class EnemyStats: ScriptableObject
{
    [Header("Базовые статы")]
    public float baseHP = 10f;
    public float baseDamage = 10f;
    public float baseSpeed = 5f;
    public float baseAttackDuration = 1f;
    public float baseAttackDistance = 1f;
    public float baseStopDistance = 1f;
    public float basePushForce = 0f;
    public float baseAttackCooldown = 1.5f;
}
