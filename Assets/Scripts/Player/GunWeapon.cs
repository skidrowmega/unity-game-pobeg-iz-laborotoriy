using UnityEngine;

public class GunWeapon : WeaponRanged
{
    [SerializeField] WeaponRangedStats statsonbreak;
    [SerializeField] GameObject tempNewWeapon;
    [SerializeField] WeaponRangedStats tempstats;
    public gunsound gunsound;
    public GunWeapon()
    {
        WeaponName = "Pistol";
        Damage = 5;
        PushStrength = 0;
        ReloadingTime = 1;
        RicochetCount = 0;
    }

    


    void BreakWeapon()
    {
        tempstats = new WeaponRangedStats(this);
        SwapStats(statsonbreak);
        OnBreak();
    }
    protected override void OnBreak()
    {
        Destroy(weaponReference);
        weaponReference = null;
        base.OnBreak();
    }

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
    public override void Shot(Vector3 Direction)
    {
        if (!IsUsable) return;
        Durability -= DurabilityDecrease;
        if (Durability <= 0)
        {
            IsUsable = false;
            BreakWeapon();
        }
        if (gunsound)
        {
            gunsound.PlaySound();
        }
        else
        {
            gunsound = GetComponentInChildren<gunsound>();
            if (gunsound)
            {
                gunsound.PlaySound();
            }
        }
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.direction = Direction;
        bullet.Shooter = WeaponHolder;
        bullet.damage = Damage;
        bullet.PushStrength = PushStrength;
        bullet = Instantiate<Projectile>(bullet, WeaponHolder.transform.position + WeaponHolder.transform.right * 2, Quaternion.identity);
    }
    GameObject PlaceWeapon(GameObject weapontoPlace, WeaponRangedStats newstats)
    {
        if (weaponReference != null) return null;
        weaponReference = Instantiate(weapontoPlace, HandReference.transform);
        SwapStats(newstats);
        return weaponReference;
    }
}
