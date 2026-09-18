using UnityEngine;

public class Physicist: EnemyAll
{
    public GameObject projectileprefab;
    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Rotation();
        Move();
        TryAttack();
    }
    
    protected override void Attack()
    {
        Sin bullet = projectileprefab.GetComponent<Sin>();
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Sin>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }
    public void GravitationalPush()//Толстая линия от физика в сторону игрока на далеко, когда время всё на ней сдвигается в сторону от физика
    {
         
    }

    public void Stan()//Под игроком появляется круг и если не убежит стан (вроде легко)
    {

    }
}
