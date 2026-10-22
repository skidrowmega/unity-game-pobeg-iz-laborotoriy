using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InternRanged: EnemyAll
{
    [Header("Все Префабы")]
    public GameObject projectileprefab;

    [Header("Увеличение статов по сравнению с базовыми")]
    [SerializeField] float MainScale = 0.2f;
    [SerializeField] float SpeedScale = 0.9f;
    [SerializeField] float DistanceScale = 10f;
    [SerializeField] EnemyStats stats;

    public override void Initialize()
    {
        //base.Initialize();
        DifficultyScale = 1f;
        MaxHealthpoints = stats.baseHP * MainScale * DifficultyScale;
        Damage = stats.baseDamage * MainScale * DifficultyScale;
        Speed = stats.baseSpeed * SpeedScale;
        AttackDuration = stats.baseAttackDuration * SpeedScale;
        attackCooldown = stats.baseAttackCooldown * SpeedScale;
        attackDistance = stats.baseAttackDistance * DistanceScale;
        PushForce = stats.basePushForce;
    }

    void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Move();
        TryAttack();
    }

    protected override void Attack()
    {
        Microscope bullet = projectileprefab.GetComponent<Microscope>();
        bullet.Shooter = this;
        bullet.direction = transform.forward;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Microscope>(bullet, transform.position + transform.forward * 0.01f + transform.right * 0.3f, Quaternion.identity);
    }
}
