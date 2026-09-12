using UnityEngine;

public class OarWeapon : WeaponMelee
{
    public override void OnHit(Entity entity, Vector3 source)
    {
        entity.TakeDamage(Damage,source, PushTime);
    }
}
