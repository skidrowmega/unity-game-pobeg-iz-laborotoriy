using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
   /* // Start is called once before the first execution of Update after the MonoBehaviour is create
    [SerializeField] protected float TimeUntilSpawn = 0;
    [SerializeField] protected bool Repeat = false;
    [SerializeField] private Vector3 Offset = Vector3.zero;
    protected float TimeUntilSpawnBuffer;
    protected float lastspawntime = 0;
    [SerializeField] protected GameObject[] objectprefabs;

    // Update is called once per frame
    protected virtual void Awake()
    {
        TimeUntilSpawnBuffer = TimeUntilSpawn;
    }
    protected virtual void Update()
    {
        if (Time.time - lastspawntime > TimeUntilSpawn)
        {
            Spawn(objectprefabs[GetRandomObstacleIndex()],Offset);
        }
    }

    protected virtual GameObject Spawn(GameObject prefab, Vector3 offset)
    {
        if (Repeat) TimeUntilSpawn = TimeUntilSpawnBuffer;
        else Destroy(gameObject);
        lastspawntime = Time.time;
        return Instantiate(prefab, transform.position + offset, transform.rotation);
    }
    protected virtual int GetRandomObstacleIndex()
    {
        return Random.Range(0, objectprefabs.Length); 
    }*/
}
