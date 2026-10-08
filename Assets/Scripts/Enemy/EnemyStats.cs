using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Roguelike/Enemy Stats")]
public class EnemyStats: ScriptableObject
{
    public float baseHP = 100f;
    public float baseDamage = 100f;
    public float baseSpeed = 1f;
    public float baseReload = 1f;
    public float baseAttackDistance = 1f;
    public float baseStopDistance = 1f;
    public float basePushForce = 1f;
    public float baseAttackCooldown = 1f;
}
