using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class InternMelee : EnemyAll
{
    [Header("Увеличение статов по сравнению с базовыми")]
    [SerializeField] float MainScale = 0.1f;
    [SerializeField] float SpeedScale = 1.1f;
    [SerializeField] float DistanceScale = 1f;
    [SerializeField] EnemyStats stats;

    public override void Initialize()
    {
        base.Initialize();
        MaxHealthpoints = stats.baseHP * MainScale * DifficultyScale;
        HealthPoints = MaxHealthpoints;
        Damage = stats.baseDamage * MainScale * DifficultyScale;
        Speed = stats.baseSpeed * SpeedScale;
        AttackDuration = stats.baseAttackDuration * SpeedScale;
        attackCooldown = stats.baseAttackCooldown * SpeedScale;
        attackDistance = stats.baseAttackDistance * DistanceScale;
        PushForce = stats.basePushForce;
    }

    public override void Update()
    {
        base.Update();
        TryAttack();
    }
}
