using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI bestScoreText;


    void OnEnable()
    {
        GameEvent.UpdateBestScoreBar += UpdateBestScoreBar; 
    }
    void OnDisable()
    {
        GameEvent.UpdateBestScoreBar -= UpdateBestScoreBar;
    }

    public void UpdateBestScoreBar(int currentScore, int bestScore)
    {
        if (bestScore <= 0)
        {
            fillImage.fillAmount = 0;
        }
        else
        {
            fillImage.fillAmount = Mathf.Clamp01((float)currentScore / bestScore);
        }

        bestScoreText.text = bestScore.ToString();
    }
}
