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

    Chemist()
    {
        attackDistance = 10;
    }

    public GameObject projectileprefab;
    public GameObject lingerPrefab;
    public GameObject molotovprefab;
    
    private bool MolotovReady = true;


    [SerializeField] private float MolotovReload = 5f;

    [SerializeField] private int MolotovDamage = 15;
    [SerializeField] private float BasicFlightTime=1f;
    [SerializeField] private float MolotovLinger=3f;
    [SerializeField] private float MolotovHitTimer = 0.5f;
    [SerializeField] private float MolotovArcHeight = 3.5f;

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Rotation();
        Move();
        TryMolotovСocktail();
        TryAttack();
    }
    protected override void Attack()//стреляет бутылочками которые наносят рандомный урон
    {
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.Shooter = this;
        bullet.direction = transform.right;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Projectile>(bullet, transform.position + transform.right * 2, Quaternion.identity);
    }


    public void TryMolotovСocktail()//стреляет бутылочками которые по параболе летят и оставляют лужу которая наносит урон если наступить
    {
        if (!isAttacking && MolotovReady)
        {
            isAttacking= true;
            StartCoroutine(MolotovBehavior());
            Invoke(nameof(ResetAttack), Reload);
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
        molotovLinger.Damage = MolotovDamage;
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
            /*            //arc = MolotovArcHeight * (nextX - startPos.x) * (nextX - targetPos.x) / (-0.25f * dist * dist);
                        nextPos = Vector3.Lerp(startPos, targetPos, elapsed / timeforflight);
                        nextPos.y = starty +;
                        molotov.transform.position = nextPos;
                        elapsed += Time.deltaTime;
                        yield return new WaitForEndOfFrame();*/

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
