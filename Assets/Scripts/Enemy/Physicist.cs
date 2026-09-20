using UnityEngine;
using System.Collections;

public class Physicist: EnemyAll
{
    public int DamageStan = 0;
    public float StanTime = 1.0f;
    public float DelayBeforeStan = 1.0f;
    public float nextAttackStanTime = 0f;
    public float RadiusStan = 5.0f;
    public float StanDistance = 10.0f;
    public float ReloadStan = 5.0f;
    public GameObject projectileprefab;
    public GameObject areaVisual;
    public GameObject CubeVisual;
    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Rotation();
        Move();
        TryGravitationalPush();
        //TryStan();
        //TryAttack();
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

    public void TryGravitationalPush()
    {
        if (!isAttacking && Time.time >= nextAttackStanTime && distanceToPlayer <= StanDistance)
        {
            isAttacking = true;
            nextAttackStanTime = Time.time + ReloadStan;
            StartCoroutine(GravitationalPushTimer());
            Debug.Log("Игрок атаковал!");
            Invoke(nameof(base.ResetAttack), Reload);
        }
    }
    public void GravitationalPush()//Толстая линия от физика в сторону игрока на далеко, когда время всё на ней сдвигается в сторону от физика
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(10f, 1f, 5f), transform.rotation);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject == gameObject) continue;

            Entity target = hit.GetComponent<Entity>();

            if (target != null)
            {
                target.TakeDamage(0, transform.position, this, 10);
                Debug.Log($"ТОЛЧОК: {hit.name}");
            }
        }
    }

    protected void TryStan()
    {
        if (!isAttacking && Time.time >= nextAttackStanTime && distanceToPlayer <= StanDistance)
        {
            isAttacking = true;
            nextAttackStanTime = Time.time + ReloadStan;
            StartCoroutine(StanTimer());
            Debug.Log("Игрок атаковал!");
            Invoke(nameof(base.ResetAttack), Reload);
        }
    }
    public void Stan()//Под игроком появляется круг и если не убежит стан (вроде легко)
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, RadiusStan);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject == gameObject) continue;

            Entity target = hit.GetComponent<Entity>();

            if (target != null)
            {
                target.TakeDamage(DamageStan, transform.position, this, PushForce);
                if (target == player)
                    player.IsStunned = true;
                Invoke(nameof(StanStop), StanTime);
                Debug.Log($"СТАН: {hit.name}");
            }
        }
    }
    protected override void StanStop()
    {
        player.IsStunned = false;
    }

    IEnumerator StanTimer()
    {
        Vector3 AreaPosition = player.transform.position;
        AreaPosition.y = 2.25f;
        GameObject Area = Instantiate(areaVisual, AreaPosition, Quaternion.identity);
        Area.transform.localScale = new Vector3(RadiusStan * 2, 1f, RadiusStan * 2);
        SpriteRenderer AreaColor = Area.GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        Color startColor = AreaColor.color;
        startColor.a = 0.2f;
        Color targetColor = startColor;
        targetColor.a = 0.8f;
        while (elapsed < DelayBeforeStan)
        {
            elapsed += Time.deltaTime;
            if (areaVisual != null)
            {
                AreaColor.color = Color.Lerp(startColor, targetColor, elapsed / DelayBeforeStan);
            }
            yield return null;
        }
        Stan();
        Destroy(Area);
    }

    IEnumerator GravitationalPushTimer()
    {
        Vector3 AreaPosition = transform.position;
        AreaPosition.y = 2.25f;
        GameObject Area = Instantiate(CubeVisual, AreaPosition, transform.rotation);
        Area.transform.localScale = new Vector3(10f, 1f, 5f);
        SpriteRenderer AreaColor = Area.GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        Color startColor = AreaColor.color;
        startColor.a = 0.2f;
        Color targetColor = startColor;
        targetColor.a = 0.8f;
        while (elapsed < DelayBeforeStan)
        {
            elapsed += Time.deltaTime;
            if (areaVisual != null)
            {
                AreaColor.color = Color.Lerp(startColor, targetColor, elapsed / DelayBeforeStan);
            }
            yield return null;
        }
        GravitationalPush();
        Destroy(Area);
    }
}
