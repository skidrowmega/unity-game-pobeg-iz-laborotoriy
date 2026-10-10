using System;
using System.Collections;
using Unity.ProjectAuditor.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Mathematician: EnemyAll
{
    [SerializeField] float MainScale = 0.8f;
    [SerializeField] float SpeedScale = 1.2f;
    [SerializeField] float DistanceScale = 12f;
    [SerializeField] float PushForceScale = 0f;
    //public bool PushImmune = false;

    float DamageDivisionByZero;
    //float DamageArithmetic;
    float ArithmeticDamageIncrease;
    float ReloadArithmetic;
    [SerializeField] float CriticalDistance = 2.0f;
    [SerializeField] float RadiusDivisionByZero = 5.0f;
    float DelayBeforeDivisionByZero;
    float ReloadDivisionByZero;

    protected internal bool ArithmeticShotReady = true;
    protected internal bool DivisioByZeroReady=true;
    public GameObject areaVisual;
    public GameObject projectileprefab;

    public override void Initialize(EnemyStats data)
    {
        base.Initialize(data);
        MaxHealthpoints = data.baseHP * MainScale * DifficultyScale;
        Damage = data.baseDamage * MainScale * DifficultyScale;
        DamageDivisionByZero = data.baseDamage * MainScale * DifficultyScale * 3f;
        ArithmeticDamageIncrease = data.baseDamage * MainScale * DifficultyScale * 0.2f;
        Speed = data.baseSpeed * SpeedScale;
        Reload = data.baseReload * SpeedScale;
        ReloadArithmetic = data.baseReload * SpeedScale * 3f;
        DelayBeforeDivisionByZero = data.baseReload * SpeedScale * 0.3f;
        ReloadDivisionByZero = data.baseReload * SpeedScale * 10f;
        attackCooldown = data.baseAttackCooldown * SpeedScale;
        attackDistance = data.baseAttackDistance * DistanceScale;
        PushForce = data.basePushForce + PushForceScale;
    }
    protected override void Start()
    {
        base.Start();
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
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Sin>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }

    private void ResetDivisionByZeroCooldown()
    {
        DivisioByZeroReady = true;
    }

    private void TryDivisionByZero()//Во время атаки не должен толкаться
    {
        if (!isAttacking && DivisioByZeroReady && distanceToPlayer <= CriticalDistance)
        {
            isAttacking = true;
            DivisioByZeroReady=false;
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
        if (!isAttacking && ArithmeticShotReady && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            ArithmeticShotReady = false;
            StartCoroutine(ArithmeticProgression(Damage, 0.2f,4));
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

    /*    public void ArithmeticProgression(int Damage)
        {
            Sin bullet = projectileprefab.GetComponent<Sin>();
            bullet.direction = transform.right;
            bullet.damage = Damage;
            bullet.PushStrength = 0;
            bullet = Instantiate<Sin>(bullet, transform.position + transform.right * 2, Quaternion.identity);
        }*/

    IEnumerator ArithmeticProgression(float Damage, float delay,int numberofshots)
    {
        for (int i = 1; i < numberofshots; i++)
        {
            Sin bullet = projectileprefab.GetComponent<Sin>();
            bullet.Shooter = this;
            bullet.PushStrength = 0;
            bullet.direction = transform.right;
            bullet.damage = Damage + i * ArithmeticDamageIncrease;
            Instantiate<Sin>(bullet, transform.position + transform.right * 2, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        ResetAttack();
    }

    IEnumerator DivisionByZeroDelay()
    {
        Vector3 AreaPosition = transform.position;
        AreaPosition.y = 2.25f;
        GameObject Area = Instantiate(areaVisual, AreaPosition, Quaternion.identity);
        Area.transform.localScale = new Vector3(RadiusDivisionByZero*2, 1f, RadiusDivisionByZero*2);
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
