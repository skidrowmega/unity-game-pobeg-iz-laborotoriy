using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.CullingGroup;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver, Victory, StartGame}
    public GameState CurrentState { get; private set; }

    public static event Action<GameState> OnStateChanged;

    private void Awake()
    {
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
        ChangeState(GameState.MainMenu);
    }

    void Update()
    {
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

        switch (newState)
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
            case GameState.StartGame:
                HandleStartGame();
                break;
        }

        OnStateChanged?.Invoke(newState);
    }

    private void HandleStartGame()
    {
        SceneManager.LoadScene("MainGeneration");
        ChangeState(GameState.Playing);
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
        Debug.Log("Игра окончена! Вы проиграли.");
    }

    private void HandleVictory()
    {
        Debug.Log("Победа!");
    }

    private void HandleMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        ChangeState(GameState.Playing);
    }
}
