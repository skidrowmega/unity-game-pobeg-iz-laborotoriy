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
    private GameObject gameoverPanel;
    private GameObject victoryPanel;
    private GameObject inventoryPanel;

    [SerializeField] bool debug = false;
    private void Awake()
    {
        GameObject spawnedCanvas;
        if (debug) spawnedCanvas = canvasPrefab;
        else spawnedCanvas = Instantiate(canvasPrefab);
        //spawnedCanvas.SetActive(true);

        mainMenuPanel = spawnedCanvas.transform.Find("MainMenuPanel").gameObject;
        gameHUDPanel = spawnedCanvas.transform.Find("gameHUD").gameObject;
        pausePanel = spawnedCanvas.transform.Find("PausePanel").gameObject;
        loadingPanel = spawnedCanvas.transform.Find("LoadingPanel").gameObject;
        gameoverPanel = spawnedCanvas.transform.Find("GameOverPanel").gameObject;
        victoryPanel = spawnedCanvas.transform.Find("VictoryPanel").gameObject;
        inventoryPanel = spawnedCanvas.transform.Find("InventoryUI/SlotsPanel").gameObject;

        Button playBtn = mainMenuPanel.transform.Find("Button").GetComponent<Button>();
        Button winBtn = pausePanel.transform.Find("Win").GetComponent<Button>();

        winBtn.onClick.AddListener(Win);
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
        gameoverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        inventoryPanel.SetActive(false);

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
                gameoverPanel.SetActive(true);
                break;
            case GameState.Loading:
                loadingPanel.SetActive(true);
                break;
            case GameState.Victory:
                //victoryPanel.SetActive(true);
                break;
            case GameState.Inventory:
                inventoryPanel.SetActive(true);
                break;
        }
    }

    private void OnPlayButtonClicked()
    {
        Instance.LoadScene("MainGeneration", GameState.Playing);
    }

    private void Win()
    {
        Instance.LoadScene("Scene_with_bober", GameState.Victory);
    }
    
}
