using UnityEngine;

[CreateAssetMenu(fileName = "BeavRage", menuName = "Roguelike/Amulets/BeavRage")]
public class BeavRage:AmuletSO
{
    [SerializeField] private float DamageForHitpointBuff;

    public float CalcDamageForHP(float basedamage)
    {
        return basedamage * (1 + (DamageForHitpointBuff * (1 - Player.player.HealthPoints/Player.player.MaxHealthpoints)));
    }
}
