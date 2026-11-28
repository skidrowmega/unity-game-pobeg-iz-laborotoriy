using System;
using System.Collections;
using UnityEngine;

public class Mathematician : EnemyAll
{
    [Header("Все Префабы")]
    [SerializeField] ParticleSystem divisionParticles;
    [SerializeField] GameObject projectileprefab;
    [SerializeField] MathematicianStats stats;

    [Header("Увеличение статов по сравнению с базовыми")]
    [SerializeField] float MainScale = 0.8f;
    [SerializeField] float SpeedScale = 1.2f;
    [SerializeField] float DistanceScale = 12f;

    float DamageDivisionByZero;
    float ArithmeticDamageIncrease;
    float ArithmeticDamage;
    float ReloadArithmetic;
    float DelayBeforeDivisionByZero;
    float ReloadDivisionByZero;

    [Header("Деление на ноль")]
    [SerializeField] float CriticalDistance = 2.0f;
    [SerializeField] float RadiusDivisionByZero = 5.0f;

    protected internal bool ArithmeticShotReady = true;
    protected internal bool DivisioByZeroReady = true;


    public override void Initialize()
    {
        base.Initialize();
        MaxHealthpoints = stats.baseHP * MainScale * DifficultyScale;
        HealthPoints = MaxHealthpoints;
        Damage = stats.baseDamage * MainScale * DifficultyScale;
        DamageDivisionByZero = stats.baseMathematicianDivisionDamage * DifficultyScale;
        ArithmeticDamage = stats.baseMathematicianArithmeticDamage * DifficultyScale;
        ArithmeticDamageIncrease = stats.baseMathematicianArithmeticDamageIncrease * DifficultyScale;
        Speed = stats.baseSpeed * SpeedScale;
        AttackDuration = stats.baseAttackDuration * SpeedScale;
        ReloadArithmetic = stats.baseMathematicianArithmeticReload * SpeedScale;
        DelayBeforeDivisionByZero = stats.baseMathematicianDivisionDelay * SpeedScale;
        ReloadDivisionByZero = stats.baseMathematicianDivisionReload * SpeedScale;
        attackCooldown = stats.baseAttackCooldown * SpeedScale;
        attackDistance = stats.baseAttackDistance * DistanceScale;
        PushForce = stats.basePushForce;
    }

    public override void Update()
    {
        base.Update();
        TryDivisionByZero();
        TryArithmeticProgression();
        TryAttack();
    }

    protected override void Attack()
    {
        Vector3 spawnPos = transform.position + transform.forward * 0.01f + transform.right * 0.3f;
        Sin bullet = Instantiate(projectileprefab, spawnPos, transform.rotation).GetComponent<Sin>();
        bullet.Shooter = this;
        bullet.direction = transform.forward;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
    }

    private void ResetDivisionByZeroCooldown()
    {
        DivisioByZeroReady = true;
    }
    private void TryDivisionByZero()//Во время атаки не должен толкаться
    {
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && DivisioByZeroReady && distanceToPlayer <= CriticalDistance)
        {
            isAttacking = true;
            DivisioByZeroReady = false;
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.ResetPath();
            animator.Play("Boitsa");
            StartCoroutine(DivisionByZeroDelay());
            Invoke(nameof(ResetDivisionByZeroCooldown), ReloadDivisionByZero);
        }
    }

    private void ResetArithmeticShotCooldown()
    {
        ArithmeticShotReady = true;
    }

    private void TryArithmeticProgression()
    {
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && ArithmeticShotReady && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            ArithmeticShotReady = false;
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.ResetPath();
            animator.Play("Sin");
            StartCoroutine(ArithmeticProgression(ArithmeticDamage, 0.2f, 4));
            Debug.Log("ArithmeticProgression has been fired Fired");
            Invoke(nameof(ResetArithmeticShotCooldown), ReloadArithmetic);
        }
    }


    public void DivisionByZero()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, RadiusDivisionByZero);
        foreach (Collider hit in hitColliders)
        {
            if (hit.tag == gameObject.tag) continue;

            Entity target = hit.GetComponent<Entity>();

            if (target != null)
            {
                target.TakeDamage(DamageDivisionByZero, transform.position, this, PushForce, DamageType.Unparriable);

                Debug.Log($"Взрывом задето: {hit.name}");
            }
        }
    }


    IEnumerator ArithmeticProgression(float Damage, float delay, int numberofshots)
    {
        for (int i = 1; i <= numberofshots; i++)
        {
            Vector3 spawnPos = transform.position + transform.forward * 2;
            Sin bullet = Instantiate(projectileprefab, spawnPos, transform.rotation).GetComponent<Sin>();
            bullet.Shooter = this;
            bullet.PushStrength = 0;
            bullet.direction = transform.forward;
            bullet.damage = Damage + i * ArithmeticDamageIncrease;
            yield return new WaitForSeconds(delay);
        }
        ResetAttack();
    }

    IEnumerator DivisionByZeroDelay()
    {
        if (divisionParticles != null) divisionParticles.Play();
        float elapsed = 0f;
        while (elapsed < DelayBeforeDivisionByZero)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (divisionParticles != null) divisionParticles.Stop();
        DivisionByZero();
        ResetAttack();
    }

    protected override void CreateDrops()
    {
        Coin.GetComponent<CurrencyItem>().Value = UnityEngine.Random.Range(3, 7);
        base.CreateDrops();
    }
}
