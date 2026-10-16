using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Chemist: EnemyAll
{
    [Header("Все Префабы")]
    [SerializeField] GameObject projectileprefab;
    [SerializeField] GameObject lingerPrefab;
    [SerializeField] GameObject molotovprefab;
    [SerializeField] ChemistStats stats;

    private float MolotovReload;
    private float MolotovDamage;
    private bool MolotovReady = true;

    [Header("Увеличение статов по сравнению с базовыми")]
    [SerializeField] float MainScale = 1f;
    [SerializeField] float SpeedScale = 1f;
    [SerializeField] float DistanceScale = 10f;

    [Header("Молотов")]
    [SerializeField] private float BasicFlightTime=1f;
    [SerializeField] private float MolotovLinger=3f;
    [SerializeField] private float MolotovHitTimer = 0.5f;
    [SerializeField] private float MolotovArcHeight = 3.5f;

    public override void Initialize()
    {
        base.Initialize();
        MaxHealthpoints = stats.baseHP * MainScale * DifficultyScale;
        Damage = stats.baseDamage * MainScale * DifficultyScale;
        MolotovDamage = stats.baseChemistMolotovDamage * DifficultyScale;
        Speed = stats.baseSpeed * SpeedScale;
        AttackDuration = stats.baseAttackDuration * SpeedScale;
        MolotovReload = stats.baseChemistMolotovReload * SpeedScale;
        attackCooldown = stats.baseAttackCooldown * SpeedScale;
        attackDistance = stats.baseAttackDistance * DistanceScale;
        PushForce = stats.basePushForce;
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Move();
        TryAttack();
        TryMolotovСocktail();
    }
    protected override void Attack()
    {
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.Shooter = this;
        bullet.direction = transform.forward;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Projectile>(bullet, transform.position + transform.forward * 2, Quaternion.identity);
    }

    public void DoMolotovCocktail() 
    {
        MolotovReady = false;
        StartCoroutine(MolotovBehavior());
        Invoke(nameof(ResetAttack), AttackDuration);
    }
    public void TryMolotovСocktail()
    {
        bool isMoving = agent.hasPath && agent.velocity.sqrMagnitude > float.Epsilon;
        if (!isAttacking && !isMoving && MolotovReady && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
                agent.ResetPath();
            animator.Play("Molotov");
            Invoke(nameof(DoMolotovCocktail), 1f);
        }
    }

    private void ResetMolotovAttack()
    {
        MolotovReady = true;
    }

    private void MolotovBurst(Vector3 Position)
    {
        GameObject area=Instantiate(lingerPrefab, Position, Quaternion.identity);
        MolotovLinger molotovLinger = area.GetComponent<MolotovLinger>();
        molotovLinger.Damage = (int)(MolotovDamage);
        molotovLinger.HitTimer = MolotovHitTimer;
        Destroy(area, MolotovLinger);
        Invoke(nameof(ResetMolotovAttack), MolotovReload);
    }

    IEnumerator MolotovBehavior()
    {
        /*
         * всяки переменные
         * цикл полета - чем дальше летит тем дольше (арбитрарное время умножается на расстояние)
         * обозначение назначения, чем ближе - тем ярче
         * оставленная зона отчуждения после удара (там звук разбивающегося стекла)
         */

        Vector3 targetPos = player.transform.position;
        Vector3 startPos = transform.position;
        startPos.y -= player.transform.localScale.y / 2;
        float starty = startPos.y;
        GameObject molotov = Instantiate(molotovprefab, transform.position,Quaternion.identity);
        float dist = Vector3.Distance(startPos,targetPos);

        float timeforflight = BasicFlightTime + (dist/50);
        float elapsed=0;
        MolotovReady = false;
        while (elapsed < timeforflight)
        {
            molotov.transform.position = MathParabola.Parabola(startPos, targetPos, MolotovArcHeight, elapsed / timeforflight);
            elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();


        }
        Destroy(molotov);
        MolotovBurst(targetPos);
    }
}
public class MathParabola
{
    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
