using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI Config")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI remainingText; // Referência ao texto "Restantes"

    [Header("Scene Management")]
    public int targetScore = 100;
    public string nextSceneName;

    private int _currentScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        _currentScore += points;
        UpdateScoreDisplay();

        if (_currentScore >= targetScore && !string.IsNullOrEmpty(nextSceneName))
        {
            LoadNextScene();
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Pontos: {_currentScore}";
        }

        if (remainingText != null)
        {
            int remaining = Mathf.Max(0, targetScore - _currentScore);
            remainingText.text = $"Restantes: {remaining}";
        }
    }

    private void LoadNextScene()
    {
        Debug.Log($"Pontuação alcançada! Carregando cena: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }

    public void ResetScore()
    {
        _currentScore = 0;
        UpdateScoreDisplay();
    }
}