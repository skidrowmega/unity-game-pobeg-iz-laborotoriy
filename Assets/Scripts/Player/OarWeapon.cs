using Unity.VisualScripting;
using UnityEngine;

public class OarWeapon : WeaponMelee
{
    



    public override void OnHit(Entity entity, Vector3 source)
    {
        Durability -= DurabilityDecrease;
        entity.TakeDamage(Damage,WeaponHolder.transform.position ,WeaponHolder, PushStrength, DamageType.Normal);
        if (Durability <= 0)
        {
            BreakWeapon();
        }
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


}