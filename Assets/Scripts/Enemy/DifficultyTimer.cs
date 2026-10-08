using UnityEngine;

public class DifficultyTimer: MonoBehaviour
{
    public static DifficultyTimer Instance { get; private set; }
    [SerializeField] private float difficultyScale = 0.1f;
    public float GameTime { get; private set; }

    public float DifficultyFactor => GameTime * difficultyScale;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        GameTime += Time.deltaTime;
    }
}
