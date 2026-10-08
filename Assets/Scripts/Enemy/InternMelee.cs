using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class InternMelee : EnemyAll
{
    [SerializeField] float MainScale = 0.1f;
    [SerializeField] float SpeedScale = 1.1f;
    [SerializeField] float DistanceScale = 1f;
    [SerializeField] float PushForceScale = 0f;
    public override void Initialize(EnemyStats data)
    {
        base.Initialize(data);
        MaxHealthpoints = data.baseHP * MainScale * DifficultyScale;
        Damage = data.baseDamage * MainScale * DifficultyScale;
        Speed = data.baseSpeed * SpeedScale;
        Reload = data.baseReload * SpeedScale;
        attackCooldown = data.baseAttackCooldown * SpeedScale;
        attackDistance = data.baseAttackDistance * DistanceScale;
        stopDistance = data.baseStopDistance * DistanceScale;
        PushForce = data.basePushForce + PushForceScale;
    }

    void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        ////Rotation();
        TryAttack();
        Move();
    }
}
