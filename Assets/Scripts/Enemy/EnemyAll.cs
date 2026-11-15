using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class EnemyAll : Entity
{
    [SerializeField] private GameObject LogCommon;
    [SerializeField] private GameObject LogGreen;
    [SerializeField] private GameObject LogRed;

    public Animator animator;
    protected Player player;
    protected bool isAttacking = false;
    public float rotationSpeed = 20f;
    protected float Damage;
    protected float AttackDuration;
    protected float attackDistance;
    protected float attackCooldown;
    protected float nextAttackTime = 0f;
    protected float PushForce;
    protected float distanceToPlayer  { get { return Vector3.Distance(Player.player.transform.position, transform.position); } }
    protected float DifficultyScale;

    public virtual void Update()
    {
        Move();
        if (agent&&!(agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon)) Rotation();
    }
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
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.transform.position);
        }

        
        if(agent)agent.enabled = !IsStunned;
        bool walking = false;
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {

                if (!agent.hasPath || Mathf.Abs(agent.velocity.sqrMagnitude) < float.Epsilon)

                    walking = false;

            }
        }
        
        else
        {

            walking = true;
        }
        animator.SetBool("isWalking", walking);
    }

    protected virtual void TryAttack()
    {

        //Debug.Log(attackDistance);
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            animator.Play("Udar");
            Invoke(nameof(Attack), 1f);
            Invoke(nameof(ResetAttack), AttackDuration);
        }
    }

    protected virtual void Attack()
    {
        if (distanceToPlayer <= attackDistance)
        {
            player.TakeDamage(Damage, Vector3.zero, this, 0, DamageType.Normal);
        }
    }

    protected virtual void ResetAttack()
    {
        isAttacking = false;
    }


    private static int DecayTime=10;

    public override void Death()
    {
        CreateDrops();

        RagdollOnDeath ragdoller = GetComponent<RagdollOnDeath>();
        if (ragdoller){
            ragdoller.StartRagdoll();
            Invoke(nameof(DeathInvoke), EnemyAll.DecayTime);
            this.enabled = false;
        }
        else
        {
            base.Death();
        }
    }



    public override void Death(Vector3 Source)
    {

        CreateDrops();
        RagdollOnDeath ragdoller = GetComponent<RagdollOnDeath>();

        if (ragdoller)
        {
            ragdoller.StartRagdoll((transform.position - Source).normalized * 10,"all");
            this.enabled = false;
            Invoke(nameof(DeathInvoke), EnemyAll.DecayTime);
        }
        else base.Death();
    }

    public override void Death(Vector3 Source, float Damage)
    {
        CreateDrops();
        RagdollOnDeath ragdoller = GetComponent<RagdollOnDeath>();

        if (ragdoller)
        {
            ragdoller.StartRagdoll((transform.position - Source).normalized * Damage, "all");
            this.enabled = false;
            Invoke(nameof(DeathInvoke), EnemyAll.DecayTime);
        }
        else base.Death();
    }

    private void CreateDrops()
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
    }

    private void DeathInvoke()
    {
        base.Death();
    }

    protected void Rotation()
    {
        Vector3 direction = player.transform.position - transform.position;

        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
