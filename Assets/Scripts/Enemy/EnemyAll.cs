using UnityEngine;
using UnityEngine.AI;

public class EnemyAll: Entity
{
    public Animator animator;
    protected Player player;
    protected bool isAttacking = false;
    public float rotationSpeed;
    protected float Damage;
    protected float AttackDuration;
    protected float attackDistance;
    protected float attackCooldown;
    protected float nextAttackTime = 0f;
    protected float PushForce;
    protected float distanceToPlayer;
    protected float DifficultyScale;

    public NavMeshAgent agent;

    public virtual void Initialize()
    {
        if (DifficultyTimer.Instance != null)
            DifficultyScale = DifficultyTimer.Instance.DifficultyFactor;
        else DifficultyScale = 1;
    }
    protected virtual void Start()
    {
        Initialize();
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
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            animator.Play("Udar");
            Attack();
            Invoke(nameof(ResetAttack), AttackDuration);
        }
    }

    protected virtual void Attack()
    {
        player.TakeDamage(Damage, Vector3.zero, this, 0, DamageType.Normal);
    }

    protected virtual void ResetAttack()
    {
        isAttacking = false;
    }
}
