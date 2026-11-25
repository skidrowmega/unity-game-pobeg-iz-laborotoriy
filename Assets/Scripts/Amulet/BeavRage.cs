using UnityEngine;

[CreateAssetMenu(fileName = "BeavRage", menuName = "Roguelike/Amulets/BeavRage")]
public class BeavRage:AmuletSO
{
    [SerializeField] private float DamageForHitpointBuff;

    public float CalcDamageForHP(float basedamage)
    {
        return basedamage * (1 + (DamageForHitpointBuff * (1 - Player.player.HealthPoints/Player.player.MaxHealthpoints)));
    }

    public override string DescriptionCreate()
    {
        return $"{amuletName}\nУрон оружия ближнего боя: x{1 + damageMeleeBuff}\nУрон оружия дальнего боя: x{1 + damageRangedBuff}\nПолучаемый урон: x{1 - resistBuff}\n{amuletDescription}\nПри максимально возможной потере ХП урон: х{1 + DamageForHitpointBuff}";
    }
}
