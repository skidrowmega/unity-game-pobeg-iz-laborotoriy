using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InternRanged: EnemyAll
{
    public GameObject projectileprefab;
    [SerializeField] float MainScale = 0.2f;
    [SerializeField] float SpeedScale = 0.9f;
    [SerializeField] float DistanceScale = 10f;
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
        //Rotation();
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
