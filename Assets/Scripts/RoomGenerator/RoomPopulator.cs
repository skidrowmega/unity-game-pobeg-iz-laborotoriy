using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomPopulator : EnemySpawner
{
    [SerializeField] int[] PrefabPoints;
    [SerializeField] float spawnTriggerrange=10;
    [SerializeField] static int PointPool = 100;
    [SerializeField] Vector2 MaxOffset = new Vector2(19,19);
    CharacterController player;

    protected override void Awake()
    {
        player = PlayerHandler.player;
        base.Awake();
    }
    protected void SpawnForPoints()
    {
        if (Vector3.Distance(PlayerHandler.player.transform.position, transform.position) <= spawnTriggerrange)
        {
            GameObject Obstacle = GetRandomObstacle();
            int PointsToSubstract = PrefabPoints[objectprefabs.ToList().IndexOf(Obstacle)];
            if (PointPool - PointsToSubstract > 0)
            {
                PointPool -= PointsToSubstract;
                Vector3 newOffset = new Vector3(Random.Range(-MaxOffset.x, MaxOffset.x), 0, Random.Range(-MaxOffset.y, MaxOffset.y));
                Spawn(Obstacle, newOffset);
            }
        }
    }
}
