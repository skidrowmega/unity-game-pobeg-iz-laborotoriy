using UnityEngine;

public class EnemyAll: Entity
{
    protected CharacterController player;
    protected bool isAttacking = false;
    public float rotationSpeed = 5.0f;
    public int Damage = 5;
    public float Reload = 1.0f;
    public float attackDistance = 1.5f;
    public float attackCooldown = 1.5f;
    protected float nextAttackTime = 0f;
    public float stopDistance = 1.0f;
    public int PushForce = 0;
    protected float distanceToPlayer;

    protected virtual void Start()
    {
        player = FindAnyObjectByType<CharacterController>();
    }

    protected virtual void Rotation()
    {
        Vector3 targetplayer = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetplayer) * Quaternion.Euler(0, -90f, 0);
        targetRotation.x = transform.rotation.x;
        targetRotation.z = transform.rotation.z;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    protected virtual void Move()
    {
        Vector3 targetplayer = player.transform.position - transform.position;
        if (!isAttacking && distanceToPlayer >= stopDistance)
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

    protected virtual void TryAttack()
    {
        if (!isAttacking && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            Attack();
            Debug.Log("Игрок атаковал!");
            Invoke(nameof(ResetAttack), Reload);
        }
    }

    protected virtual void Attack()
    {
        player.TakeDamage(Damage, Vector3.zero, this, 0, DamageType.Normal);
    }

    protected void ResetAttack()
    {
        isAttacking = false;
    }
}
