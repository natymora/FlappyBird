using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI currentHighScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;


    [Header("Audio")]
    public AudioController audioController;

    private int score;
    private int highScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ResetCurrentScore();
        LoadHighScore();
        UpdateScoreUI();
    }
    public void SaveCurrentScore()
    {
        PlayerPrefs.SetInt("CurrentScore", score);
    }

    public void ResetCurrentScore()
    {
        score = 0;
        SaveCurrentScore();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", score);
    }

    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highScore.ToString();
        currentHighScoreText.text = highScore.ToString();

    }

    private void UpdateScoreUI()
    {
        currentScoreText.text = score.ToString();
        finalScoreText.text = score.ToString();
    }

    public void UpdateScore()
    {
        if (GameController.instance.canPlay)
        {
            score++;
            SaveCurrentScore();
            UpdateScoreUI();
            CheckHighScore();

            audioController.PointSFX();
        }
    }
    private void CheckHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
            highScoreText.text = score.ToString();
            currentHighScoreText.text = score.ToString();
        }
    }
}