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
    [SerializeField] float timeToCreateStrongEnemy = 20f;

    float difficultyMulPointPool;
    [SerializeField] Vector2 MaxOffset = new Vector2(9, 9);
    private int limit = 0;

    protected void Start()
    {
        minPoint = PrefabPoints.Min();
        currentPointPool = (int)(basePointPool);
        SpawnForPoints();
    }

    protected void Update()
    {
        time = GameManager.Instance.GameTime;

        if (!cantDeleteWeakEnemy && time - lastDeleteWeakEnemy >= timeToDeleteWeakEnemy)
        {
            //Debug.Log("Time to Delete Enemy");
            //Debug.Log(Time.time);
            DeleteWeakEnemy();
            lastDeleteWeakEnemy += time;
        }
        if (!cantCanCreateStrongEnemy && time - lastCreateStrongEnemy >= timeToCreateStrongEnemy)
        {
            //Debug.Log("Time to Create Enemy");
            //Debug.Log(Time.time);
            CreateStrongEnemy();
            lastCreateStrongEnemy += time;
        }

        if (Time.time - lastspawntime > TimeUntilSpawn)
        {
            //Debug.Log("Time to Spawn Enemy");
            //Debug.Log(Time.time);
            difficultyMulPointPool = GameManager.Instance.DifficultyFactor * 5;
            currentPointPool = (int)(basePointPool + difficultyMulPointPool);
            SpawnForPoints();
        }
    }

    protected void SpawnForPoints()
    {
        limit = 0;
        if (Vector3.Distance(Player.player.transform.position, transform.position) <= spawnTriggerrangeMax && Vector3.Distance(Player.player.transform.position, transform.position) >= spawnTriggerrangeMin)
        {
            //Debug.Log(currentPointPool);
            while (currentPointPool >= minPoint && limit < 5)
            {
                //Debug.Log("SPAWN!!!!!!");
                limit++;
                int ObstacleIndex = GetRandomObstacleIndex();
                int PointsToSubstract = PrefabPoints[ObstacleIndex];
                if (currentPointPool - PointsToSubstract >= 0)
                {
                    GameObject Obstacle = objectprefabs[ObstacleIndex];
                    Vector3 newOffset = new Vector3(Random.Range(-MaxOffset.x, MaxOffset.x), 0, Random.Range(-MaxOffset.y, MaxOffset.y));
                    GameObject spawnedEnemy = Instantiate(Obstacle, newOffset + transform.position, Quaternion.identity);
                    currentPointPool -= PointsToSubstract;
                }
            }
        }
        lastspawntime = Time.time;
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
