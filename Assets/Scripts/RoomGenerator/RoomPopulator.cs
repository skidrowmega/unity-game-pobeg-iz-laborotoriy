using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomPopulator : MonoBehaviour //чтобы потом не только нижняя граница но и верхняя граница префабов смещалась
{
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

    private int minEnemyIndex = 0;
    private int maxEnemyIndex = 2;
    private float lastDeleteWeakEnemy = 0f;
    private float lastCreateStrongEnemy = 0f;
    private bool cantDeleteWeakEnemy = false;
    private bool cantCanCreateStrongEnemy = false;
    [SerializeField] float timeToDeleteWeakEnemy = 20f;
    [SerializeField] float timeToCreateMoreDifficultEnemy = 20f;

    float difficultyMulPointPool;
    [SerializeField] Vector2 MaxOffset = new Vector2(9, 9);
    Player player;
    [SerializeField] int WeakEnemiesAmount = 1;

    protected void Start()
    {
        minPoint = PrefabPoints.Min();
        player = Player.player;
        currentPointPool = (int)(basePointPool);
        SpawnForPoints();
    }

    protected void Update()
    {
        time = DifficultyTimer.Instance.GameTime;

        if (!cantDeleteWeakEnemy && time - lastDeleteWeakEnemy >= timeToDeleteWeakEnemy)
        {
            DeleteWeakEnemy();
            lastDeleteWeakEnemy += time;
        }
        if (!cantCanCreateStrongEnemy && time - lastCreateStrongEnemy >= timeToCreateMoreDifficultEnemy)
        {
            CreateStrongEnemy();
            lastCreateStrongEnemy += time;
        }

        if (Time.time - lastspawntime > TimeUntilSpawn)
        {
            difficultyMulPointPool = DifficultyTimer.Instance.DifficultyFactor;
            SpawnForPoints();
            currentPointPool = (int)(basePointPool + difficultyMulPointPool);
        }
    }

    protected void SpawnForPoints()
    {
        if (Vector3.Distance(Player.player.transform.position, transform.position) <= spawnTriggerrangeMax && Vector3.Distance(Player.player.transform.position, transform.position) >= spawnTriggerrangeMin)
        {
            while (currentPointPool >= minPoint)
            {
                int ObstacleIndex = GetRandomObstacleIndex();
                int PointsToSubstract = PrefabPoints[ObstacleIndex];
                if (currentPointPool - PointsToSubstract >= 0)
                {
                    GameObject Obstacle = objectprefabs[ObstacleIndex];
                    Vector3 newOffset = new Vector3(Random.Range(-MaxOffset.x, MaxOffset.x), 0, Random.Range(-MaxOffset.y, MaxOffset.y));
                    GameObject spawnedEnemy = Instantiate(Obstacle, newOffset, Quaternion.identity);
                    currentPointPool -= PointsToSubstract;
                    lastspawntime = Time.time;
                }
            }

        }
    }

    protected int GetRandomObstacleIndex()
    {
        return Random.Range(minEnemyIndex, maxEnemyIndex);
    }

    private void DeleteWeakEnemy()
    {
        minEnemyIndex++;
        minEnemyIndex = Mathf.Clamp(minEnemyIndex, 0, maxEnemyIndex - 2);
        minPoint = PrefabPoints[minEnemyIndex..maxEnemyIndex].Min();
        if (minEnemyIndex == objectprefabs.Length - 2)
        {
            cantDeleteWeakEnemy = true;
        }
    }

    private void CreateStrongEnemy()
    {
        maxEnemyIndex++;
        maxEnemyIndex = Mathf.Clamp(maxEnemyIndex, minEnemyIndex + 1, objectprefabs.Length);
        if (maxEnemyIndex == objectprefabs.Length)
        {
            cantCanCreateStrongEnemy = true;
        }
    }
}
