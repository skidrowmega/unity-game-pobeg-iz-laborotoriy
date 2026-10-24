using UnityEngine;
using UnityEngine.AI;

public class EnemyAll: Entity
{
    [SerializeField] private GameObject LogCommon;
    [SerializeField] private GameObject LogGreen;
    [SerializeField] private GameObject LogRed;

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

    public virtual void Initialize()
    {
        DifficultyScale = GameManager.Instance.DifficultyFactor;
    }
    protected virtual void Awake()
    {
        player = Player.player;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
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

        Debug.Log(attackDistance);
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            animator.Play("Udar");
            Invoke(nameof(Attack), 0.5f);
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

    public override void Death()
    {
        float rand = Random.value;
        if (rand < 0.3f)
        {
            Instantiate(LogCommon, transform.position, Quaternion.identity);
        }
        else if (rand < 0.6f)
        {
            Instantiate(LogRed, transform.position, Quaternion.identity);
        }
        else if (rand < 0.9f)
        {
            Instantiate(LogGreen, transform.position, Quaternion.identity);
        }
        base.Death();
    }
}
