using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Chemist: EnemyAll
{
    public GameObject projectileprefab;
    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Rotation();
        Move();
        TryAttack();
    }

    protected override void Attack()//стреляет бутылочками которые наносят рандомный урон
    {
        Sin bullet = projectileprefab.GetComponent<Sin>();
        bullet.Shooter = this;
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Sin>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }

    public void MolotovСocktail()//стреляет бутылочками которые по параболе летят и оставляют лужу которая наносит урон если наступить
    {

    }
}
