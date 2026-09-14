using UnityEngine;

public class EnemyAll: Entity
{
    protected CharacterController player;
    protected CharacterController controller;
    private bool isAttacking = false;
    public float rotationSpeed = 5.0f;
    public int Damage = 5;
    public int Reload = 1;
    public float attackDistance = 1.5f;
    public float attackCooldown = 1.5f;
    protected float nextAttackTime = 0f;
    public float stopDistance = 1.0f;

    protected virtual void Move()
    {
        Vector3 targetplayer = player.transform.position - transform.position;
        float distanceplayer = targetplayer.magnitude;
        if (!isAttacking && distanceplayer >= stopDistance)
        {
            if (IsStunned) return;
            float MoveX = targetplayer.x;
            float MoveZ = targetplayer.z;
            Quaternion targetRotation = Quaternion.LookRotation(targetplayer) * Quaternion.Euler(0, -90f, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (MoveX != 0 || MoveZ != 0)
                transform.position += new Vector3(MoveX, 0, MoveZ).normalized * Speed * Time.deltaTime;
        }
    }

    protected virtual void Punch()//во время атаки он не может двигаться пока не закончимтся анимация
    {
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;

        if (!isAttacking && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            Attack();
            Debug.Log("Игрок атаковал!");
            Invoke(nameof(ResetAttack), 1.0f);
        }
    }

    protected virtual void Attack()
    {
        player.TakeDamage(Damage, Vector3.zero, controller, 0);
    }
        void ResetAttack()
    {
        isAttacking = false;
    }
}
