using System;
using TMPro;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;
    private int bestScore;
    private const string BEST_SCORE_KEY = "BestScore";
    public SquareTextureData squareTextureData;
    void Start()
    {
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        UpdateScoreText();
        squareTextureData.SetStartColor();
        GameEvent.UpdateBestScoreBar?.Invoke(currentScore, bestScore);

    }
    void OnEnable()
    {
        GameEvent.AddScore += AddScore;
        GameEvent.SaveCurrentScore += SaveCurrentScore;
    }
    void OnDisable()
    {
        GameEvent.AddScore -= AddScore;
        GameEvent.SaveCurrentScore -= SaveCurrentScore;
    }

    private void SaveCurrentScore()
    {
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.Save();
    }

    private void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateScoreText();
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
            SaveBestScore();
        }
        GameEvent.UpdateBestScoreBar?.Invoke(currentScore, bestScore);
        // xu li doi mau
        UpdateSquareColor();

    }
    public void SaveBestScore()
    {
        PlayerPrefs.Save();
    }
    public void UpdateSquareColor()
    {
        if (currentScore >= squareTextureData.thresholdValue)
        {
            squareTextureData.UpdateColors(currentScore); // chỉ đổi màu qua của cac bien mau trong squareTextureData
            GameEvent.UpdateSquareColor?.Invoke(squareTextureData.currentColor); // truyền màu mới cho các square khác để đổi màu của chúng
        }
    }
}
