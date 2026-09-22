using System.Collections;
using Unity.ProjectAuditor.Editor;
using UnityEngine;

public class Physicist: EnemyAll
{
    public int DamageStan = 0;
    public float StanTime = 1.0f;
    public float StanFollowTime = 1f;
    public float StanFireTime = 0.3f;
    public float nextAttackStanTime = 0f;
    public float RadiusStan = 10f;
    public float StanDistance = 10.0f;
    public float ReloadStan = 2.5f;

    public int DmgGravPush = 0;
    public float GravPushStrength = 5f;
    public float GravPushDistance = 5f;
    public float GravPushFollowTime = 1f;
    public float GravPushFireTime = 0.5f;
    public float nextAttackGravTime = 0f;
    public float ReloadGrav = 5f;

    public GameObject projectileprefab;
    public GameObject areaVisual;
    public GameObject CubeVisual;
    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Rotation();
        Move();
        TryGravitationalPush();
        TryStan();
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
        if (!isAttacking && Time.time >= nextAttackGravTime && distanceToPlayer <= StanDistance)
        {
            print("Firing off GravPush");
            isAttacking = true;
            nextAttackGravTime = Time.time + ReloadGrav;
            StartCoroutine(GravitationalPushTimer());
        }
    }
    public void GravitationalPush(Vector3 center,Vector3 BoxDimensions,Quaternion BoxRotation)//Толстая линия от физика в сторону игрока на далеко, когда время всё на ней сдвигается в сторону от физика
    {
        Collider[] hitColliders = Physics.OverlapBox(center, BoxDimensions, BoxRotation);
        foreach (Collider hit in hitColliders)
        {
            if (hit.GetComponent<CharacterController>())
            {
                player.TakeDamage(DmgGravPush, transform.position, this, GravPushStrength, DamageType.Unparriable);
            }
        }
    }

    protected void TryStan()
    {
        if (!isAttacking && Time.time >= nextAttackStanTime && distanceToPlayer <= StanDistance)
        {
            print("Firing off stan");
            isAttacking = true;
            nextAttackStanTime = Time.time + ReloadStan;
            StartCoroutine(StanTimer());
        }
    }
    public void Stan(Vector3 stunposition)//Под игроком появляется круг и если не убежит стан (вроде легко)
    {
        if ((player.transform.position-stunposition).magnitude<=RadiusStan)
        player.TakeDamage(DamageStan, stunposition, this, PushForce, DamageType.Unparriable,StanTime);
    }
    protected void StanStop()
    {
        player.UnStun();
    }

    IEnumerator GravitationalPushTimer()
    {
        float BoxLength = 50f;
        float BoxWidth = 3f;
        Vector3 lookingatplayer = (player.transform.position - transform.position).normalized;
        Vector3 BoxPosition = Vector3.Lerp( (lookingatplayer)*BoxLength/2+transform.position,transform.position,0.5f);
        BoxPosition.y = transform.position.y-transform.localScale.y/2+.1f;
        GameObject Box = Instantiate(CubeVisual, BoxPosition, Quaternion.LookRotation(lookingatplayer));
        Vector3 newboxscale = Box.transform.localScale;
        newboxscale.z = BoxLength;
        newboxscale.x = BoxWidth; 
        Box.transform.localScale = newboxscale;
        SpriteRenderer BoxColor = Box.GetComponentInChildren<SpriteRenderer>();
        print(BoxColor.gameObject.name);
        float elapsed = 0f;
        Color startColor = BoxColor.color;
        startColor.a = 0;
        startColor.r = 0;
        Color targetColor = startColor;
        targetColor.a = 0.8f;
        targetColor.r = 0.5f;
        while (elapsed < GravPushFollowTime)
        {
            elapsed += Time.deltaTime;
            if (areaVisual != null)
            {
                lookingatplayer = (player.transform.position - transform.position).normalized;
                BoxColor.color = Color.Lerp(startColor, targetColor, elapsed / GravPushFollowTime);

                BoxPosition = Vector3.Lerp((lookingatplayer) * BoxLength + transform.position, transform.position, 0.5f);
                BoxPosition.y = transform.position.y - transform.localScale.y / 2 + .1f;

                Box.transform.position = BoxPosition;
                Box.transform.rotation = Quaternion.LookRotation(lookingatplayer);
            }
            yield return null;
        }
        targetColor.a = 1;
        targetColor.r = 1;
        BoxColor.color = Color.Lerp(startColor, targetColor, 1);
        yield return new WaitForSeconds(GravPushFireTime);
        GravitationalPush(BoxPosition,Box.transform.localScale,Box.transform.rotation);
        Invoke(nameof(ResetAttack), Reload);
        Destroy(Box);
    }

    IEnumerator StanTimer()
    {
        Vector3 AreaPosition = player.transform.position;
        AreaPosition.y = transform.position.y - transform.localScale.y / 2 + .1f;
        GameObject Area = Instantiate(areaVisual, AreaPosition, transform.rotation);
        Area.transform.localScale = new Vector3(RadiusStan*2, 1f, RadiusStan*2);
        SpriteRenderer AreaColor = Area.GetComponentInChildren<SpriteRenderer>();
        float elapsed = 0f;
        Color startColor = AreaColor.color;
        startColor.a = 0;
        startColor.r = 0;
        Color targetColor = startColor;
        targetColor.a = 0.8f;
        targetColor.r = 0.5f;
        while (elapsed < StanFollowTime)
        {
            elapsed += Time.deltaTime;
            if (areaVisual != null)
            {
                AreaColor.color = Color.Lerp(startColor, targetColor, elapsed / StanFollowTime);
            }
            yield return null;
        }
        targetColor.a = 1;
        targetColor.r = 1;
        AreaColor.color = Color.Lerp(startColor, targetColor, 1);
        yield return new WaitForSeconds(StanFireTime);
        Stan(AreaPosition);
        Invoke(nameof(ResetAttack),Reload);
        Destroy(Area);
    }
}
