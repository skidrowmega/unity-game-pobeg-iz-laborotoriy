using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [Header("Панели интерфейса")]
    [SerializeField] private GameObject canvasPrefab;

    private GameObject mainMenuPanel;
    private GameObject gameHUDPanel;
    private GameObject pausePanel;

    private void Awake()
    {
        GameObject spawnedCanvas = Instantiate(canvasPrefab);
        spawnedCanvas.SetActive(true);

        mainMenuPanel = spawnedCanvas.transform.Find("Canvas/MainMenuPanel").gameObject;
        gameHUDPanel = spawnedCanvas.transform.Find("Canvas/gameHUDPanel").gameObject;
        pausePanel = spawnedCanvas.transform.Find("Canvas/PausePanel").gameObject;

        Button playBtn = mainMenuPanel.transform.Find("PlayButton").GetComponent<Button>();

        playBtn.onClick.AddListener(OnPlayButtonClicked);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(spawnedCanvas);
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameManager.GameState state)
    {
        Debug.Log("Оно живое");
        mainMenuPanel.SetActive(false);
        gameHUDPanel.SetActive(false);
        pausePanel.SetActive(false);

        switch (state)
        {
            case GameManager.GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                gameHUDPanel.SetActive(true);
                break;
            case GameManager.GameState.Paused:
                pausePanel.SetActive(true);
                break;
        }
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.StartGame);
    }
}
