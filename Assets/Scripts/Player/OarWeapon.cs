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
    void OnBreak()
    {
        Destroy(weaponReference);
        weaponReference= null;
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
        weaponReference = Instantiate(weapontoPlace, MeleeHandReference.transform);
        SwapStats(newstats);
        return weaponReference;
    }
}