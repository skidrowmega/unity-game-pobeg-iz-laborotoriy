using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using static UnityEngine.CullingGroup;



public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver, Victory, StartGame, Loading, Inventory, Craft }

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
        LoadScene("MainMenuScene", GameState.MainMenu);
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
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.Loading:
                HandleLoading();
                break;
            case GameState.Inventory:
                HandleStartGame();
                break;
            case GameState.Craft:
                HandleStartGame();
                break;
        }

        OnStateChanged?.Invoke(CurrentState);
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
        //SceneManager.LoadScene("MainMenuScene");
    }

    private void HandleLoading()
    {
        Time.timeScale = 0f;
    }

    private void HandleInventory()
    {
        Time.timeScale = 1f;
    }

    private void HandleCraft()
    {
        Time.timeScale = 1f;
    }


    /*    public void RestartLevel()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            ChangeState(GameState.Playing);
        }*/

    public void LoadScene(string sceneName, GameState scenestate)
    {
        StartCoroutine(LoadSceneAsync(sceneName, scenestate));
    }

    private IEnumerator LoadSceneAsync(string sceneName, GameState scenestate)
    {
        Instance.ChangeState(GameState.Loading);
        //CurrentState = GameState.Loading;
        //loadingPanel.SetActive(true);
        //loadingSlider.value = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        //operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            /*loadingSlider.value = progress;

            if (loadingText != null)
                loadingText.text = $"Загрузка... {Mathf.RoundToInt(progress * 100)}%";

            if (operation.progress >= 0.9f)
            {
                if (loadingText != null)
                    loadingText.text = "Нажмите любую клавишу для продолжения...";

                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }*/

            yield return null; // Ждем следующий кадр
        }

        //loadingPanel.SetActive(false);
        Instance.ChangeState(scenestate);
    }
}

