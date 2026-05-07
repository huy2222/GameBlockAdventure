using UnityEngine;
using UnityEngine.UI;

public class WatchAdsToContinueButton : MonoBehaviour
{
    public Button watchAdsToContinueButton;

    void Start()
    {
        watchAdsToContinueButton.onClick.AddListener(OnWatchAdsToContinueButtonClicked);
    }

    private void OnWatchAdsToContinueButtonClicked()
    {
        AdsManager.Instance.ShowRewardedAd();
        watchAdsToContinueButton.interactable = false; 
    }
}
