using UnityEngine;

public class OarWeapon : WeaponMelee
{
    public void OnHit(int damage, Entity entity, Vector3 source)
    {
        entity.TakeDamage(damage, source, PushTime);
    }
}
