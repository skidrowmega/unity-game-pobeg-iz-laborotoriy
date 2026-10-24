using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using static UnityEngine.CullingGroup;



public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver, Victory, Loading, Inventory, Craft }

    public static event Action<GameState> OnStateChanged;

    [SerializeField] private GameObject bobr;
    [SerializeField] private GameObject craftManager;

    [SerializeField] private float difficultyScale = 0.1f;
    public float GameTime { get; private set; }
    public float DifficultyFactor => GameTime * difficultyScale + 10;


    [SerializeField] private bool debug = false;




    private void Awake()
    {
        if(debug) CurrentState=GameState.Playing;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (debug) bobr = Player.player.gameObject;
        if (!debug)
        {
            Instantiate(bobr);
            DontDestroyOnLoad(Instantiate(craftManager));
            LoadScene("MainMenuScene", GameState.MainMenu);
        }
        
    }

    void Update()
    {
        GameTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
            {
                ChangeState(GameState.Paused);
            }
            else if (CurrentState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
            }
        }
    }
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
            case GameState.Loading:
                HandleLoading();
                break;
            case GameState.Inventory:
                HandleInventory();
                break;
            case GameState.Craft:
                HandleCraft();
                break;
        }

        OnStateChanged?.Invoke(CurrentState);
    }

    private void HandlePlaying()
    {
        Time.timeScale = 1f;
    }

    private void HandlePaused()
    {
        Time.timeScale = 0f;
    }

    private void HandleGameOver()
    {
        Time.timeScale = 0f;
        LoadScene("MainMenuScene", GameState.MainMenu, 5f);
    }

    private void HandleVictory()
    {
        Time.timeScale = 0f;
        Debug.Log("Победа!");
    }

    private void HandleMainMenu()
    {
        Time.timeScale = 0f;
    }

    private void HandleLoading()
    {
        Time.timeScale = 0f;
    }

    private void HandleInventory()
    {
        Time.timeScale = 0.3f;
    }

    private void HandleCraft()
    {
        Time.timeScale = 0.3f;
    }


    /*    public void RestartLevel()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            ChangeState(GameState.Playing);
        }*/

    public void LoadScene(string sceneName, GameState scenestate, float delayInSeconds = 0f)
    {
        StartCoroutine(LoadSceneAsync(sceneName, scenestate, delayInSeconds));
    }

    private IEnumerator LoadSceneAsync(string sceneName, GameState scenestate, float delayInSeconds)
    {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        Instance.ChangeState(GameState.Loading);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
        if (sceneName == "MainGeneration")
        {
            Player.ChangeBobrPosition(GameObject.FindGameObjectWithTag("LiftEnter").transform);
        }
        Instance.ChangeState(scenestate);
    }
}

