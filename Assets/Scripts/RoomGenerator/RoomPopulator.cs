using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomPopulator : MonoBehaviour //чтобы потом не только нижняя граница но и верхняя граница префабов смещалась
{
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] protected float TimeUntilSpawn = 5f;
    float time;
    float lastspawntime = 0;
    [SerializeField] protected GameObject[] objectprefabs;
    [SerializeField] int[] PrefabPoints;
    [SerializeField] float spawnTriggerrangeMax = 30;
    [SerializeField] float spawnTriggerrangeMin = 10;
    int minPoint;
    [SerializeField] float basePointPool = 30f;
    int currentPointPool;
    float difficultyMulPointPool;
    [SerializeField] Vector2 MaxOffset = new Vector2(9, 9);
    CharacterController player;
    private bool isLateGame = false;
    [SerializeField] float TimeDeleteWeakEnemies = 30f;
    [SerializeField] int WeakEnemiesAmount = 1;

    protected void Awake()
    {
        minPoint = PrefabPoints.Min();
        player = PlayerHandler.player;
        currentPointPool = (int)(basePointPool);
    }

    protected void Update()
    {
        time = DifficultyTimer.Instance.GameTime;
        if (!isLateGame && time >= TimeDeleteWeakEnemies)
        {
            isLateGame = true;
            minPoint = PrefabPoints[WeakEnemiesAmount..].Min();
        }
        if (time - lastspawntime > TimeUntilSpawn)
        {
            difficultyMulPointPool = DifficultyTimer.Instance.DifficultyFactor;
            SpawnForPoints();
            currentPointPool = (int)(basePointPool + difficultyMulPointPool);
        }
    }

    protected void SpawnForPoints()
    {
        if (Vector3.Distance(PlayerHandler.player.transform.position, transform.position) <= spawnTriggerrangeMax && Vector3.Distance(PlayerHandler.player.transform.position, transform.position) >= spawnTriggerrangeMin)
        {
            while (currentPointPool >= minPoint)
            {
                int ObstacleIndex = GetRandomObstacleIndex();
                int PointsToSubstract = PrefabPoints[ObstacleIndex];
                if (currentPointPool - PointsToSubstract > 0)
                {
                    GameObject Obstacle = objectprefabs[ObstacleIndex];
                    Vector3 newOffset = new Vector3(Random.Range(-MaxOffset.x, MaxOffset.x), 0, Random.Range(-MaxOffset.y, MaxOffset.y));
                    GameObject spawnedEnemy = Instantiate(Obstacle, newOffset, Quaternion.identity);
                    if (spawnedEnemy.TryGetComponent<EnemyAll>(out EnemyAll enemyScript))
                    {
                        enemyScript.Initialize(enemyStats);
                    }
                    currentPointPool -= PointsToSubstract;
                    lastspawntime = time;
                }
            }

        }
    }
    protected int GetRandomObstacleIndex()
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
