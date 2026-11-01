using Unity.VisualScripting;
using UnityEngine;

public class OarWeapon : WeaponMelee
{
    public override void OnHit(Entity entity, Vector3 source)
    {
        float CurrentDamage = WeaponHolder.amuletManager.GetModifiedDamage(Damage);
        Durability -= DurabilityDecrease;
        entity.TakeDamage(CurrentDamage,WeaponHolder.transform.position ,WeaponHolder, PushStrength, DamageType.Normal);
        print(CurrentDamage);
        if (Durability <= 0)
        {
            BreakWeapon();
        }
    }


/*#if UNITY_EDITOR
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
#endif*/


}