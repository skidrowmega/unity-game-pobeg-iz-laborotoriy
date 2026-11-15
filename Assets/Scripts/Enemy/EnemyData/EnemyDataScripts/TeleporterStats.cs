using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyStats", menuName = "Roguelike/Enemy Stats/Teleporter Stats")]
public class TeleporterStats: ScriptableObject
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



    [Header("Телепортация")]
    public float baseTeleportRadius = 100f;
    public float baseTeleportDistance = 5f;
    public float baseTeleportCooldown = 15f;



}
