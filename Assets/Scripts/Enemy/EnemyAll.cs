using UnityEngine;
using UnityEngine.AI;

public class EnemyAll: Entity
{
    public Animator animator;
    protected Player player;
    protected bool isAttacking = false;
    public float rotationSpeed;
    protected float Damage;
    protected float AttackDuration;//переименовать везде
    protected float attackDistance;
    protected float attackCooldown;
    protected float nextAttackTime = 0f;
    protected float PushForce;
    protected float distanceToPlayer;
    protected float DifficultyScale;

    NavMeshAgent agent;

    public virtual void Initialize(EnemyStats data)
    {
        DifficultyScale = DifficultyTimer.Instance.DifficultyFactor;
    }
    protected virtual void Start()
    {
        player = Player.player;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    protected virtual void Move()
    {
        agent.SetDestination(player.transform.position);
        agent.enabled = !IsStunned;
        bool walking = false;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {

            if (!agent.hasPath || Mathf.Abs(agent.velocity.sqrMagnitude) < float.Epsilon)

                walking = false;

        }
        else
        {

            walking = true;
        }
        animator.SetBool("isWalking", walking);
    }

    protected virtual void TryAttack()
    {
        if (!isAttacking && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            animator.SetBool("isAttacking", true);
            Attack();
            Invoke(nameof(ResetAttack), AttackDuration);
        }
    }

    protected virtual void Attack()
    {
        player.TakeDamage(Damage, Vector3.zero, this, 0, DamageType.Normal);
    }

    protected void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
