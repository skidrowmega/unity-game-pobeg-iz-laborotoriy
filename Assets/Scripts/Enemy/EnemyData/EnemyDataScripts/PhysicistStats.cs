using UnityEngine;

[CreateAssetMenu(fileName = "PhysicistStats", menuName = "Roguelike/Enemy Stats/Physicist Stats")]
public class PhysicistStats: EnemyStats
{
    public float basePhysicistPushDamage = 5f;
    public float basePhysicistStanDamage = 10f;
    public float basePhysicistPushReload = 5f;
    public float basePhysicistStanReload = 7f;
    public float basePhysicistPushPushForce = 1f;
    public float basePhysicistStanStanTime = 0.5f;
}
