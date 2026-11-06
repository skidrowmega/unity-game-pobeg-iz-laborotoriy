using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SplinterHurricaneAmulet", menuName = "Roguelike/Amulets/SplinterHurricaneAmulet")]

public class SplinterHurricaneAmulet: AmuletSO
{
    private GameObject projectileprefab;
    private float Damage;
    public void ShootSplinter()
    {
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.Shooter = Player.player;
        bullet.direction = Player.player.transform.forward;
        bullet.damage = Damage;
        bullet.PushStrength = 0;
        bullet = Instantiate<Projectile>(bullet, Player.player.transform.position + Player.player.transform.forward * 0.01f + Player.player.transform.right * 0.3f, Quaternion.identity);
    }
}
