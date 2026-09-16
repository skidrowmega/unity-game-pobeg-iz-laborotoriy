using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InternRanged: EnemyAll
{
    public GameObject projectileprefab;
    void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<CharacterController>();
        }
    }

    void Update()
    {
        Rotation();
        Move();
        TryAttack();
    }
    protected override void Attack()
    {
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Projectile>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }
}
