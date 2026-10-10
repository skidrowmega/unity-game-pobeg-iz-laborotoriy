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

    public override void Initialize(EnemyStats data)
    {
        base.Initialize(data);
        MaxHealthpoints = data.baseHP * MainScale * DifficultyScale;
        Damage = data.baseDamage * MainScale * DifficultyScale;
        Speed = data.baseSpeed * SpeedScale;
        AttackDuration = data.baseAttackDuration * SpeedScale;
        attackCooldown = data.baseAttackCooldown * SpeedScale;
        attackDistance = data.baseAttackDistance * DistanceScale;
        PushForce = data.basePushForce;
    }

    void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        TryAttack();
        Move();
    }
}
