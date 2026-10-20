using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using static GameManager;

public class UIManager: MonoBehaviour
{
    [Header("Панели интерфейса")]
    [SerializeField] private GameObject canvasPrefab;

    private GameObject mainMenuPanel;
    private GameObject gameHUDPanel;
    private GameObject pausePanel;
    private GameObject loadingPanel;
    private GameObject youlosePanel;
    private GameObject victoryPanel;

    private void Awake()
    {
        GameObject spawnedCanvas = Instantiate(canvasPrefab);
        //spawnedCanvas.SetActive(true);

        mainMenuPanel = spawnedCanvas.transform.Find("Canvas/MainMenuPanel").gameObject;
        gameHUDPanel = spawnedCanvas.transform.Find("Canvas/gameHUDPanel").gameObject;
        pausePanel = spawnedCanvas.transform.Find("Canvas/PausePanel").gameObject;
        loadingPanel = spawnedCanvas.transform.Find("Canvas/LoadingPanel").gameObject;
        youlosePanel = spawnedCanvas.transform.Find("Canvas/YouLosePanel").gameObject;
        victoryPanel = spawnedCanvas.transform.Find("Canvas/VictoryPanel").gameObject;

        Button playBtn = mainMenuPanel.transform.Find("PlayButton").GetComponent<Button>();

        playBtn.onClick.AddListener(OnPlayButtonClicked);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(spawnedCanvas);
    }

    private void OnEnable()
    {
        OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        mainMenuPanel.SetActive(false);
        gameHUDPanel.SetActive(false);
        pausePanel.SetActive(false);
        loadingPanel.SetActive(false);
        youlosePanel.SetActive(false);
        victoryPanel.SetActive(false);

        switch (state)
        {
            case GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
            case GameState.Playing:
                gameHUDPanel.SetActive(true);
                break;
            case GameState.Paused:
                pausePanel.SetActive(true);
                break;
            case GameState.GameOver:
                youlosePanel.SetActive(true);
                break;
            case GameState.Loading:
                loadingPanel.SetActive(true);
                break;
            case GameState.Victory:
                victoryPanel.SetActive(true);
                break;
            case GameState.Inventory:
                //pausePanel.SetActive(true);
                break;
        }
    }

    private void OnPlayButtonClicked()
    {
        Instance.LoadScene("MainGeneration", GameState.Playing);
    }

    
}
