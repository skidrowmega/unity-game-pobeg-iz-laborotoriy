using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class InternMelee : EnemyAll
{

    public override void Initialize(EnemyStats data)
    {
        base.Initialize(data);
        MaxHealthpoints = data.baseHP;
        Damage = data.baseDamage;
        Speed = data.baseSpeed;
        Reload = data.baseReload;
        attackDistance = data.baseAttackDistance;
        stopDistance = data.baseStopDistance;
        PushForce = data.basePushForce;
        attackCooldown = data.baseAttackCooldown;
    }

    void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        ////Rotation();
        TryAttack();
        Move();
    }
}
