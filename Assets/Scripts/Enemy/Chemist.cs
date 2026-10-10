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
    public GameObject projectileprefab;
    public GameObject lingerPrefab;
    public GameObject molotovprefab;

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

    public override void Initialize(EnemyStats data)
    {
        base.Initialize(data);
        MaxHealthpoints = data.baseHP * MainScale * DifficultyScale;
        Damage = data.baseDamage * MainScale * DifficultyScale;
        MolotovDamage = data.baseChemistMolotovDamage * DifficultyScale;
        Speed = data.baseSpeed * SpeedScale;
        AttackDuration = data.baseAttackDuration * SpeedScale;
        MolotovReload = data.baseChemistMolotovReload * SpeedScale;
        attackCooldown = data.baseAttackCooldown * SpeedScale;
        attackDistance = data.baseAttackDistance * DistanceScale;
        PushForce = data.basePushForce;
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Move();
        TryMolotovСocktail();
        TryAttack();
    }
    protected override void Attack()
    {
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.Shooter = this;
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Projectile>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }


    public void TryMolotovСocktail()
    {
        if (!isAttacking && MolotovReady && distanceToPlayer <= attackDistance)
        {
            isAttacking= true;
            StartCoroutine(MolotovBehavior());
            Invoke(nameof(ResetAttack), AttackDuration);
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

    public class MathParabola
    {
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }
    }

    }
