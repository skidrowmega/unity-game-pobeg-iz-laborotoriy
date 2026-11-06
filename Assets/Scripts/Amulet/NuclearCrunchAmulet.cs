using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NuclearCrunchAmulet", menuName = "Roguelike/Amulets/NuclearCrunchAmulet")]

public class NuclearCrunchAmulet: AmuletSO
{
    public GameObject explosionPrefab;
    public float explosionRadius;
    public float Damage;//брать урон того оружия которое сломалось
    public float pushStrenght;
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
