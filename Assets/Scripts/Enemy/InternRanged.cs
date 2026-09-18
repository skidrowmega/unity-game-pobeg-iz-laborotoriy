using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InternRanged: EnemyAll
{
    public GameObject projectileprefab;

    void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Rotation();
        Move();
        TryAttack();
    }
    protected override void Attack()
    {
        Microscope bullet = projectileprefab.GetComponent<Microscope>();
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Microscope>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }
}
