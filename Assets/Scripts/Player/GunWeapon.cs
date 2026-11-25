using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GunWeapon : WeaponRanged
{
    [SerializeField] WeaponRangedStats statsonbreak;
    [SerializeField] GameObject tempNewWeapon;
    [SerializeField] WeaponRangedStats tempstats;
    public GunWeapon()
    {
        WeaponName = "Pistol";
        Damage = 5;
        PushStrength = 0;
        ReloadingTime = 1;
        RicochetCount = 0;
    }


    AudioSource audioSource;
    private void Awake()
    {
        audioSource=GetComponent<AudioSource>();
    }

    protected override void OnBreak()
    {
        tempstats = new WeaponRangedStats(this);
        SwapStats(statsonbreak);
    }

/*#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            BreakWeapon();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            PlaceWeapon(tempNewWeapon, tempstats);
        }
    }
#endif*/
    public override void Shot(Vector3 Direction)
    {
        float currentDamage = WeaponHolder.amuletManager.GetDamageForHitpointBuff(WeaponHolder.amuletManager.GetModifiedRangedDamage(Damage));
        if (!IsUsable) return;
        Durability -= DurabilityDecrease;
        if (Durability <= 0)
        {
            IsUsable = false;
            BreakWeapon();
            return;
        }
        audioSource.PlayOneShot(DefaultGunSound, 1);
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.direction = Direction;
        bullet.Shooter = WeaponHolder;
        bullet.damage = currentDamage;
        bullet.PushStrength = PushStrength;
        bullet = Instantiate<Projectile>(bullet, WeaponHolder.transform.position + WeaponHolder.transform.right/2 + new Vector3 (0f, 1.3f, 0f), Quaternion.identity);
        /*EditorApplication.isPaused = true;*/
    }

}
