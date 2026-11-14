using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class Teleporter : EnemyAll
{
    [Header("Увеличение статов по сравнению с базовыми")]
    [SerializeField] float MainScale = 0.1f;
    [SerializeField] float SpeedScale = 1.1f;
    [SerializeField] float DistanceScale = 1f;
    [SerializeField] TeleporterStats stats;


    [SerializeField] float baseTeleportRadius;
    [SerializeField] float baseTeleportDistance;
    [SerializeField] float baseTeleportCooldown;

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
        baseTeleportRadius = stats.baseTeleportRadius;
        baseTeleportDistance = stats.baseTeleportRadius;
        baseTeleportCooldown = stats.baseTeleportRadius;
    }

    public override void Update()
    {
        base.Update();
        TryAttack();
        TryTeleport();
    }

    protected override void TryAttack()
    {

        //Debug.Log(attackDistance);
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            animator.Play("Udar");
            Invoke(nameof(Attack), 1f);
            Invoke(nameof(ResetAttack), AttackDuration);
        }
    }


    void TryTeleport()
    {
        if (!isAttacking && CanTeleport&& Vector3.Distance(player.transform.position,transform.position)<baseTeleportDistance)
        {
            TeleportToPlayer();
        }
    }

    void TeleportToPlayer()
    {
        bool CanTeleport = false;
        Vector3 newposition=Vector3.zero;
        int maxiterations = 1000;
        int count = 0;
        NavMeshPath path = new NavMeshPath();
        while (!CanTeleport&&count<maxiterations)
        {
            newposition=player.transform.position + (new Vector3(Random.value, 0, Random.value).normalized*baseTeleportRadius);
            newposition.y = transform.position.y;
            CanTeleport = NavMesh.CalculatePath(transform.position, newposition, NavMesh.AllAreas , path);
            count++;
        }

        if (CanTeleport) Teleport(newposition);
        
    }

    bool CanTeleport = true;
    public void Teleport(Vector3 whereto)
    {
        transform.position = whereto;
        CanTeleport = false;
        Invoke(nameof(ResetTeleport), baseTeleportCooldown);
    }

    private void ResetTeleport()
    {
        CanTeleport = true;
    }
}
