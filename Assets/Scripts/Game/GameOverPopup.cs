using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public TextMeshProUGUI finalScoreText;
    void Start()
    {
        gameOverPopup.SetActive(false);
    }
    void OnEnable()
    {
        GameEvent.GameOver += ShowGameOverPopup;
    }
    void OnDisable()
    {
        GameEvent.GameOver -= ShowGameOverPopup;
    }
    public void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true);
        int finalScore = PlayerPrefs.GetInt("CurrentScore", 0);
        finalScoreText.text = finalScore.ToString();
    }
}
