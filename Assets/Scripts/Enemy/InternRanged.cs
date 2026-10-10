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
        Move();
        TryAttack();
    }

    protected override void Attack()
    {
        Microscope bullet = projectileprefab.GetComponent<Microscope>();
        bullet.Shooter = this;
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Microscope>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }
}
