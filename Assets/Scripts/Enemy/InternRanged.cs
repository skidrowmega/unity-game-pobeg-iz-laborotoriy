using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InternRanged: EnemyAll
{
    public GameObject projectileprefab;

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
