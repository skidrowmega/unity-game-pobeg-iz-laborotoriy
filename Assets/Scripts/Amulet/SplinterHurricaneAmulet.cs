using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SplinterHurricaneAmulet", menuName = "Roguelike/Amulets/SplinterHurricaneAmulet")]

public class SplinterHurricaneAmulet: AmuletSO
{
    public GameObject projectileprefab;
    public float Damage;
    public void ShootSplinter()
    {
        Projectile bullet = projectileprefab.GetComponent<Projectile>();
        bullet.Shooter = Player.player;
        bullet.direction = Player.player.transform.forward;
        bullet.damage = Damage;//возможно стоит брать урон весла(предположительно нереализуемо)
        bullet.PushStrength = 0;
        bullet = Instantiate<Projectile>(bullet, Player.player.transform.position + Player.player.transform.forward * 0.01f + Player.player.transform.right * 0.3f, Quaternion.identity);
    }

    public override string DescriptionCreate()
    {
        return $"{amuletName}\nУрон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nПолучаемый урон: x{1 - resistBuff}\n{amuletDescription}\nУрон осколков: {Damage}";
    }
}
