using UnityEngine;

[CreateAssetMenu(fileName = "Tail-Dropper", menuName = "Roguelike/Amulets/Tail-Dropper")]
public class TailDropper : AmuletSO
{
    public GameObject explosionPrefab;
    public float explosionRadius;
    public float Damage;
    public float pushStrenght;
    public void ExplosionWithDodge()
    {
        Vector3 breakPos = AmuletManager.instance.transform.position;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, breakPos, Quaternion.identity);
        }

        Collider[] hitColliders = Physics.OverlapSphere(breakPos, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<Entity>().TakeDamage(Damage, breakPos, Player.player, pushStrenght, DamageType.Unparriable);
            }
        }
    }

    public override string DescriptionCreate()
    {
        return $"{amuletName}\nУрон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nПолучаемый урон: x{1 - resistBuff}\n{amuletDescription}\nУрон взрыва при перекате: {Damage}";
    }
}
