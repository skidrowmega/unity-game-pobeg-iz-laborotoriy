using System.Collections;
using UnityEngine;

public class TurretScript : EnemyAll
{
    [SerializeField] float MainScale = 0.8f;
    [SerializeField] float SpeedScale = 1.2f;
    [SerializeField] float DistanceScale = 12f;


    [SerializeField] int burstshotamount = 3;

    

    [SerializeField] TurretStats stats;

    [SerializeField] Projectile bulletprefab;

    protected override void Start()
    {
        base.Start();
        Projectile bullet = bulletprefab.GetComponent<Projectile>();
        bullet.Shooter = this;
        bullet.PushStrength = 0;
        bullet.damage = Damage;

        if(!GameManager.Instance||GameManager.Instance.debug) Initialize();
    }

    
    public override void Initialize()
    {
        base.Initialize();
        burstshotamount = stats.burstshotamount;
        //burstshotamount = Mathf.FloorToInt(stats.burstshotamount * MainScale * DifficultyScale);


        MaxHealthpoints = stats.baseHP * MainScale * DifficultyScale;
        HealthPoints = MaxHealthpoints;
        Damage = stats.baseDamage * MainScale * DifficultyScale;
        Speed = stats.baseSpeed * SpeedScale;
        AttackDuration = stats.baseAttackDuration * SpeedScale;
        attackCooldown = stats.baseAttackCooldown * SpeedScale;
        attackDistance = stats.baseAttackDistance * DistanceScale;
        PushForce = stats.basePushForce;
    }


    public override void Update()
    {
        PlayerTracking();
        TryBurstShot();
    }

    void PlayerTracking()
    {
        Vector3 vectortoplayer = Player.player.transform.position - transform.position;
        //transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(vectortoplayer.z, vectortoplayer.x), 0);
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(-vectortoplayer.z, vectortoplayer.x)+90, Vector3.up);
    }

    private void TryBurstShot()
    {
        //print( (!isAttacking , distanceToPlayer , attackDistance) );
        //Debug.Break();
        if (!isAttacking && distanceToPlayer <= attackDistance)
        {
            isAttacking = true;


            StartCoroutine(BurstShot(Damage, AttackDuration/burstshotamount, burstshotamount));
            //Play Shot sound
            Debug.Log("Turret burst shot has been fired Fired");
            
        }
    }


    IEnumerator BurstShot(float Damage, float delay, int numberofshots)
    {
        for (int i = 1; i <= numberofshots; i++)
        {
            
            Projectile bullet = Instantiate<Projectile>(bulletprefab, transform.position + transform.forward * .5f, Quaternion.identity);
            bullet.direction = transform.forward;
            yield return new WaitForSeconds(delay);
        }
        Invoke(nameof(ResetAttack), attackCooldown);
    }




}
