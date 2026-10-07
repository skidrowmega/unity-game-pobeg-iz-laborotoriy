using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomPopulator : EnemySpawner //Сделать чтобы враги со временем усилялись статами
{
    [SerializeField] int[] PrefabPoints;
    [SerializeField] float spawnTriggerrangeMax = 30;
    [SerializeField] float spawnTriggerrangeMin = 10;
    int minPoint;
    [SerializeField] float basePointPool = 30f;
    int currentPointPool;
    [SerializeField] float difficultyMulPointPool = 0.5f;
    [SerializeField] Vector2 MaxOffset = new Vector2(9,9);
    CharacterController player;
    private bool isLateGame = false;
    [SerializeField] float TimeDeleteWeakEnemies = 30f;
    [SerializeField] int WeakEnemiesAmount= 1;

    protected override void Awake()
    {
        minPoint = PrefabPoints.Min();
        player = PlayerHandler.player;
        currentPointPool = (int)(basePointPool);
        base.Awake();
    }

    protected override void Update()
    {
        if (!isLateGame && Time.time >= TimeDeleteWeakEnemies)
        {
            isLateGame = true;
            minPoint = PrefabPoints[WeakEnemiesAmount..].Min();
        }
        if (Time.time - lastspawntime > TimeUntilSpawn)
        {
            SpawnForPoints();
            currentPointPool = (int)(basePointPool + (Time.time * difficultyMulPointPool));
        }
    }

    protected void SpawnForPoints()
    {
        if (Vector3.Distance(PlayerHandler.player.transform.position, transform.position) <= spawnTriggerrangeMax && Vector3.Distance(PlayerHandler.player.transform.position, transform.position) >= spawnTriggerrangeMin)
        {
            while (currentPointPool > minPoint)
            {
                int ObstacleIndex = GetRandomObstacleIndex();
                int PointsToSubstract = PrefabPoints[ObstacleIndex];
                if (currentPointPool - PointsToSubstract > 0)
                {
                    GameObject Obstacle = objectprefabs[ObstacleIndex];
                    currentPointPool -= PointsToSubstract;
                    Vector3 newOffset = new Vector3(Random.Range(-MaxOffset.x, MaxOffset.x), 0, Random.Range(-MaxOffset.y, MaxOffset.y));
                    Spawn(Obstacle, newOffset);
                }
            }
            
        }
    }
    protected override int GetRandomObstacleIndex()
    {
        if (isLateGame)
        {
            return Random.Range(WeakEnemiesAmount, objectprefabs.Length);
        }
        else
        {
            return Random.Range(0, objectprefabs.Length);
        }
    }
}
