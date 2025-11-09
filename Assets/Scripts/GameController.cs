using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // static significa que siempre va a estar y nunca va a cambiar. acceso en todos los scripts
    public static GameController instance;

    public PlayerController playerController;
    public AudioController audioController;

    [Header("Booleanos")]
    public bool canPlay = false;
    public bool gameOver = false;
    public bool gamePaused = false;

    [Header("Asignar Objectos")]
    public Button playButton;
    public Button pauseButton;
    public GameObject playPanel;
    public GameObject gameOverPanel;
    public GameObject ScorePanel;
    public GameObject selectColorPanel;
    public GameObject infoPanel;

    [Header("Sprites del botón Pausa/Play")]
    public Sprite pauseSprite;
    public Sprite playSprite;

    private Image pauseButtonImage;

    private void Awake()
    {
        // Singleton
        instance = this;

        canPlay = false;
        Time.timeScale = 0f;

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);

        // Asignar evento al botón
        if (playButton != null)
            playButton.onClick.AddListener(StartGame);

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
            pauseButtonImage = pauseButton.GetComponent<Image>();
        }
    }

    private void StartGame()
    {
        canPlay = true;
        playerController.ToggleRigidBody();
        Time.timeScale = 1f;

        // Activando lo necesario
        playPanel.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        ScorePanel.SetActive(true);

        // Aseguramos que el icono empiece como "pausa"
        if (pauseButtonImage != null && pauseSprite != null)
            pauseButtonImage.sprite = pauseSprite;

        // 🎵 Inicia música del juego
        audioController.GameThemeMusic();
    }

    public void TogglePause()
    {
        if (gamePaused)
        {
            // Reanudar
            gamePaused = false;
            Time.timeScale = 1f;
            canPlay = true;

            if (pauseButtonImage != null && pauseSprite != null)
                pauseButtonImage.sprite = pauseSprite;

            // Reanudar música de fondo
            if (audioController != null && audioController.audioSourceTheme != null)
                audioController.audioSourceTheme.UnPause();
        }
        else
        {
            // Pausar
            gamePaused = true;
            Time.timeScale = 0f;
            canPlay = false;

            audioController.StopAllSFX();

            if (pauseButtonImage != null && playSprite != null)
                pauseButtonImage.sprite = playSprite;

            // Pausar música de fondo
            if (audioController != null && audioController.audioSourceTheme != null)
                audioController.audioSourceTheme.Pause();
        }
    }

    public void ToggleCanPlay()
    {
        if (canPlay)
        {
            canPlay = false;
            playerController.ToggleRigidBody();
        }
        else
        {
            canPlay = true;
            playerController.ToggleRigidBody();
        }
    }

    public void CallGameOver()
    {
        gameOverPanel.SetActive(true);
        ScorePanel.SetActive(false);

        gameOver = true;
        ToggleCanPlay();

        Time.timeScale = 0f;


        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);


        audioController.GameOverMusic();
        audioController.StopAllSFX();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Invoke(nameof(ReloadScene), 0.2f);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenColorSelection()
    {
        selectColorPanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void ToggleInfoPanel()
    {
        if (infoPanel != null)
        {
            bool isActive = infoPanel.activeSelf;
            infoPanel.SetActive(!isActive);
        }
    }
}
