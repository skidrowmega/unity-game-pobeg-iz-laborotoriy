using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "PartingBlowoutAmulet", menuName = "Roguelike/Amulets/PartingBlowoutAmulet")]

public class PartingBlowoutAmulet: AmuletSO
{
    private GameObject explosionPrefab;
    private float explosionRadius;
    private float Damage;
    private float pushStrenght;
    public void Explosion()
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
}
