using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using System.Collections;

public class Mathematician: EnemyAll
{
    private int DamageSin = 40;
    private int DamageArithmetic = 40;
    private float CriticalDistance = 2.0f;
    private float radiusDivisionByZero = 5.0f;
    private float DelayBeforeDivisionByZero = 1.0f;
    public SpriteRenderer areaVisual;
    public GameObject projectileprefab;
    private void Update()
    {
        Rotation();
        Move();
        //TryAttack();
        TryDivisionByZero();
        //TryArithmeticProgression();
    }
    protected override void TryAttack()//Кидается синусоидой наверное
    {
        base.TryAttack();
    }

    protected override void Attack()
    {
        
    }
    private void TryDivisionByZero()
    {
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        if (!isAttacking && Time.time >= nextAttackTime && distanceToPlayer <= CriticalDistance)
        {
            isAttacking = true;
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(DivisionByZeroDelay());
            Debug.Log("Игрок атаковал!");
            Invoke(nameof(base.ResetAttack), Reload);
        }
    }
    private void TryArithmeticProgression()
    {

    }

    public void DivisionByZero()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusDivisionByZero);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject == gameObject) continue;

            Entity target = hit.GetComponent<Entity>();

            if (target != null)
            {
                target.TakeDamage(Damage, transform.position, this, PushForce);

                Debug.Log($"Взрывом задето: {hit.name}");
            }
        }
    }

    public void ArithmeticProgression()
    {

    }

    IEnumerator DivisionByZeroDelay()
    {
        float elapsed = 0f;
        Color startColor = areaVisual.color;
        startColor.a = 0.2f;
        Color targetColor = startColor;
        targetColor.a = 0.8f;

        while (elapsed < DelayBeforeDivisionByZero)
        {
            elapsed += Time.deltaTime;
            if (areaVisual != null)
            {
                areaVisual.color = Color.Lerp(startColor, targetColor, elapsed / DelayBeforeDivisionByZero);
            }
            yield return null;
        }
        DivisionByZero();
    }
}
