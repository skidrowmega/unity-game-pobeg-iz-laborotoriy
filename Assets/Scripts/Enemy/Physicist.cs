using System.Collections;
using Unity.ProjectAuditor.Editor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Physicist: EnemyAll
{
    [SerializeField] float MainScale = 1f;
    [SerializeField] float SpeedScale = 0.9f;
    [SerializeField] float DistanceScale = 8f;

    float DamageStan;
    [SerializeField] float StanTime = 1.0f;
    [SerializeField] float StanFollowTime = 1f;
    [SerializeField] float StanFireTime = 0.3f;
    [SerializeField] float nextAttackStanTime = 0f;
    [SerializeField] float RadiusStan = 10f;
    float StanDistance;
    float ReloadStan;

    float DmgGravPush;
    float GravPushStrength;
    float GravPushDistance;
    [SerializeField] float GravPushFollowTime = 1f;
    [SerializeField] float GravPushFireTime = 0.5f;
    [SerializeField] float nextAttackGravTime = 0f;
    float ReloadGrav;

    public GameObject projectileprefab;
    public GameObject areaVisual;
    public GameObject CubeVisual;

    public override void Initialize(EnemyStats data)
    {
        base.Initialize(data);
        MaxHealthpoints = data.baseHP * MainScale * DifficultyScale;
        Damage = data.baseDamage * MainScale * DifficultyScale;
        DamageStan = data.baseDamage * MainScale * DifficultyScale * 0.1f;
        DmgGravPush = data.baseDamage * MainScale * DifficultyScale * 0.4f;
        Speed = data.baseSpeed * SpeedScale;
        Reload = data.baseReload * SpeedScale;
        attackCooldown = data.baseAttackCooldown * SpeedScale;
        attackDistance = data.baseAttackDistance * DistanceScale;
    }
    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        //Rotation();
        Move();
        TryGravitationalPush();
        TryStan();
        //TryStan();
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

    public void TryGravitationalPush()
    {
        if (!isAttacking && Time.time >= nextAttackGravTime && distanceToPlayer <= StanDistance)
        {
            animator.SetBool("isGravitationalPush", true);
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
            if (hit.GetComponent<Player>())
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
        Vector3 levelposplayer = player.transform.position;
        Vector3 levelposenemy = transform.position;
        levelposplayer.y = levelposenemy.y;
        Vector3 lookingatplayer = (levelposplayer - levelposenemy).normalized;
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
                levelposplayer = player.transform.position;
                levelposenemy = transform.position;
                levelposplayer.y = levelposenemy.y;
                lookingatplayer = (levelposplayer - levelposenemy).normalized;
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
