using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AnomalySpawner: MonoBehaviour
{
    [SerializeField] protected float TimeUntilSpawn = 5f;
    float lastspawntime = 0;
    [SerializeField] protected GameObject anomalyprefab;
    [SerializeField] float spawnTriggerrangeMax = 50;
    [SerializeField] float spawnTriggerrangeMin = 20;
    [SerializeField] Vector2 MaxOffset = new Vector2(9, 9);
    Player player;

    protected void Start()
    {
        player = Player.player;
        SpawnAnomaly();
        lastspawntime = Time.time;
    }

    protected void Update()
    {
        if (Time.time - lastspawntime > TimeUntilSpawn)
        {
            print(lastspawntime);
            SpawnAnomaly();
        }
    }

    protected void SpawnAnomaly()
    {
        if (Vector3.Distance(Player.player.transform.position, transform.position) <= spawnTriggerrangeMax && Vector3.Distance(Player.player.transform.position, transform.position) >= spawnTriggerrangeMin)
        {
            Vector3 newOffset = new Vector3(Random.Range(-MaxOffset.x, MaxOffset.x), 0, Random.Range(-MaxOffset.y, MaxOffset.y));
            GameObject spawnedAnomaly = Instantiate(anomalyprefab, newOffset + transform.position, Quaternion.identity);
            lastspawntime = Time.time;
        }
        else
        {
            lastspawntime = Time.time;
        }
    }
}
