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
    private Vector3 teleportPosition;
    private bool isTeleporting = false;
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
        baseTeleportDistance = stats.baseTeleportDistance;
        baseTeleportCooldown = stats.baseTeleportCooldown;
    }

    public override void Update()
    {
        base.Update();
        if (isTeleporting) return;
        TryAttack();
        TryTeleport();
    }

    protected override void TryAttack()
    {
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            if(animator)animator.Play("Udar");
            Invoke(nameof(Attack), 1f);
            Invoke(nameof(ResetAttack), AttackDuration);
        }
    }

    [SerializeField] bool CanTeleport = true;
    void TryTeleport()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (!isAttacking && CanTeleport&& distance<baseTeleportDistance&&distance > baseTeleportRadius)
        {
            TeleportToPlayer();
        }
    }

    void TeleportToPlayer()
    {
        bool foundPosition = false;
        Vector3 newposition=Vector3.zero;
        int maxiterations = 1000;
        int count = 0;
        RaycastHit hit;
        NavMeshPath path = new NavMeshPath();
        while (!foundPosition && count<maxiterations)
        {
            newposition=player.transform.position + (new Vector3(Random.Range(-baseTeleportRadius,baseTeleportRadius), 0, Random.Range(-baseTeleportRadius,baseTeleportRadius)));
            newposition.y = transform.position.y;
            if (Vector3.Distance(player.transform.position, newposition) < baseTeleportRadius / 2) continue;
            bool ishit = Physics.Raycast(newposition, player.transform.position, out hit);
            path = agent.path;
            foundPosition = (agent.SetDestination(newposition)); //&& ishit && hit.collider && hit.collider.gameObject.GetEntityId() == player.GetEntityId());
            agent.path = path;
            //if (!CanTeleport)
            //{
            //    print($" {ishit} {hit.collider.gameObject.name} {hit.collider.gameObject.GetEntityId() == player.GetEntityId()}");
            //}
            //CanTeleport = NavMesh.CalculatePath(transform.position, newposition, NavMesh.AllAreas , path);
            count++;
        }

        if (foundPosition)
        {
            CanTeleport = false;
            isTeleporting = true;
            if (agent) agent.isStopped = true;
            teleportPosition = newposition;
            if (animator) animator.Play("Teleport");
            Invoke(nameof(Teleport), 2f);
        }
        //else
        //{
        //    print("COULDNT TELEPORT BECAUSE REASONS");
        //}
        
    }

    
    public void Teleport()
    {
        //print($"SUCCESSFULLY TELEPORTED FROM {transform.position} to {whereto}, TRAVERSING {Vector3.Distance(transform.position, whereto)} UNITS");
        if (agent)
        {
            agent.Warp(teleportPosition);
            agent.isStopped = false;
        }
        else
        {
            transform.position = teleportPosition;
        }

        isTeleporting = false;
        Invoke(nameof(ResetTeleport), baseTeleportCooldown);
    }

    private void ResetTeleport()
    {
        CanTeleport = true;
    }
}
