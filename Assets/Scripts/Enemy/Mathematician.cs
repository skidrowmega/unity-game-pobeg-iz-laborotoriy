using System;
using System.Collections;
using Unity.ProjectAuditor.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Mathematician : EnemyAll
{
    [Header("Все Префабы")]
    [SerializeField] GameObject areaVisual;
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

    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        //Rotation();
        Move();
        TryDivisionByZero();
        TryArithmeticProgression();
        TryAttack();
    }

    protected override void Attack()
    {
        Sin bullet = projectileprefab.GetComponent<Sin>();
        bullet.Shooter = this;
        bullet.direction = transform.forward;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Sin>(bullet, transform.position + transform.forward * 2, Quaternion.identity);
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
            if (hit.gameObject == gameObject) continue;

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
            Sin bullet = projectileprefab.GetComponent<Sin>();
            bullet.Shooter = this;
            bullet.PushStrength = 0;
            bullet.direction = transform.forward;
            bullet.damage = Damage + i * ArithmeticDamageIncrease;
            Instantiate<Sin>(bullet, transform.position + transform.forward * 2, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        ResetAttack();
    }

    IEnumerator DivisionByZeroDelay()
    {
        Vector3 AreaPosition = transform.position;
        AreaPosition.y = 2.25f;
        GameObject Area = Instantiate(areaVisual, AreaPosition, Quaternion.identity);
        Area.transform.localScale = new Vector3(RadiusDivisionByZero * 2, 1f, RadiusDivisionByZero * 2);
        SpriteRenderer AreaColor = Area.GetComponentInChildren<SpriteRenderer>();
        float elapsed = 0f;
        Color startColor = AreaColor.color;
        Color targetColor = startColor;
        startColor.a = 0.2f;
        targetColor.a = 0.8f;
        while (elapsed < DelayBeforeDivisionByZero)
        {
            elapsed += Time.deltaTime;
            AreaColor.color = Color.Lerp(startColor, targetColor, elapsed / DelayBeforeDivisionByZero);
            yield return null;
        }
        DivisionByZero();
        ResetAttack();
        Destroy(Area);
    }

}
