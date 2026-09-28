using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    [SerializeField] float TimeUntilSpawn = 0;
    [SerializeField] bool Repeat = false;
    float TimeUntilSpawnBuffer;
    float lastspawntime;
    [SerializeField] GameObject[] enemyprefab;

    // Update is called once per frame
    private void Awake()
    {
        TimeUntilSpawnBuffer = TimeUntilSpawn;
    }
    void Update()
    {
        if (Time.time - lastspawntime > TimeUntilSpawn)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy =  enemyprefab[Random.Range(0, enemyprefab.Length)];
        Instantiate(enemy,transform.position,transform.rotation);
        if (Repeat) TimeUntilSpawn = TimeUntilSpawnBuffer;
        else Destroy(gameObject);
        lastspawntime = Time.time;
    }

}
