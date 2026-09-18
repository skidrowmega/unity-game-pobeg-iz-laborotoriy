using System.Collections;
using Unity.ProjectAuditor.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Mathematician: EnemyAll
{
    public bool PushImmune = false;
    public int DamageSave = 0;
    public int DamageDivisionByZero = 40;
    public int DamageArithmetic = 10;
    public int ArithmeticDamageIncrease = 5;
    public float ReloadArithmetic = 5.0f;
    public float nextAttackTimeAritmetic = 0f;
    public float CriticalDistance = 2.0f;
    public float RadiusDivisionByZero = 5.0f;
    public float DelayBeforeDivisionByZero = 1.0f;
    public float nextAttackTimeDivision = 0f;
    public float ReloadDivisionByZero = 10f;
    public GameObject areaVisual;
    public GameObject projectileprefab;
    protected override void Start()
    {
        base.Start();
        DamageSave = DamageArithmetic;
    }
    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Rotation();
        Move();
        TryDivisionByZero();
        TryArithmeticProgression();
        TryAttack();
    }

    protected override void Attack()
    {
        Sin bullet = projectileprefab.GetComponent<Sin>();
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Sin>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }

    private void TryDivisionByZero()//Во время атаки не должен толкаться
    {
        if (!isAttacking && Time.time >= nextAttackTime && distanceToPlayer <= CriticalDistance)
        {
            isAttacking = true;
            nextAttackTimeDivision = Time.time + ReloadDivisionByZero;
            StartCoroutine(DivisionByZeroDelay());
            Debug.Log("Игрок атаковал!");
            Invoke(nameof(base.ResetAttack), Reload);
        }
    }
    private void TryArithmeticProgression()
    {
        if (!isAttacking && Time.time >= nextAttackTime && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            nextAttackTimeAritmetic = Time.time + ReloadArithmetic;
            ArithmeticProgression();
            Invoke(nameof(ArithmeticProgression), 0.2f);
            Invoke(nameof(ArithmeticProgression), 0.4f);
            Invoke(nameof(ArithmeticProgression), 0.6f);
            DamageArithmetic = DamageSave;
            Debug.Log("Игрок атаковал Арифметику!");
            Invoke(nameof(ResetAttack), Reload);
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
                target.TakeDamage(DamageDivisionByZero, transform.position, this, PushForce);

                Debug.Log($"Взрывом задето: {hit.name}");
            }
        }
    }

    public void ArithmeticProgression()
    {
        DamageArithmetic += ArithmeticDamageIncrease;
        Sin bullet = projectileprefab.GetComponent<Sin>();
        bullet.direction = transform.right;
        bullet.damage = DamageArithmetic;
        bullet.PushStrength = 0;
        bullet = Instantiate<Sin>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }

    IEnumerator DivisionByZeroDelay()
    {
        Vector3 AreaPosition = transform.position;
        AreaPosition.y = 2.25f;
        GameObject Area = Instantiate(areaVisual, AreaPosition, Quaternion.identity);
        Area.transform.localScale = new Vector3(RadiusDivisionByZero, 1f, RadiusDivisionByZero);
        SpriteRenderer AreaColor = Area.GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        Color startColor = AreaColor.color;
        startColor.a = 0.2f;
        Color targetColor = startColor;
        targetColor.a = 0.8f;
        while (elapsed < DelayBeforeDivisionByZero)
        {
            elapsed += Time.deltaTime;
            if (areaVisual != null)
            {
                AreaColor.color = Color.Lerp(startColor, targetColor, elapsed / DelayBeforeDivisionByZero);
            }
            yield return null;
        }
        DivisionByZero();
        Destroy(Area);
    }
}   
