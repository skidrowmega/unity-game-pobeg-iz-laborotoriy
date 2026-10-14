using Unity.VisualScripting;
using UnityEngine;

public class OarWeapon : WeaponMelee
{
    [SerializeField] WeaponMeleeStats statsonbreak;
    [SerializeField] GameObject tempNewWeapon;
    [SerializeField] WeaponMeleeStats tempstats;


    public override void OnHit(Entity entity, Vector3 source)
    {
        Durability -= DurabilityDecrease;
        entity.TakeDamage(Damage,WeaponHolder.transform.position ,WeaponHolder, PushStrength, DamageType.Normal);
        if (Durability <= 0)
        {
            BreakWeapon();
        }
    }
    void BreakWeapon()
    {
        tempstats = new WeaponMeleeStats(this);
        SwapStats(statsonbreak);
        OnBreak();
    }
    protected override void OnBreak()
    {
        Destroy(weaponReference);
        weaponReference= null;
        base.OnBreak();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            BreakWeapon();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlaceWeapon(tempNewWeapon,tempstats);
        }
    }

    GameObject PlaceWeapon(GameObject weapontoPlace,WeaponMeleeStats newstats)
    {
        if (weaponReference != null)return null;
        weaponReference = Instantiate(weapontoPlace, HandReference.transform);
        SwapStats(newstats);
        return weaponReference;
    }
}