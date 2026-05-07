using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    void Awake()
    {
        Instance = this;
    }
    public  void ShowRewardedAd()
    {
        Debug.Log("Show Reward Ad...");

        // Giả lập xem quảng cáo xong
        Invoke(nameof(OnAdCompleted), 2f);
    }

    private void OnAdCompleted()
    {
        Debug.Log("Rewarded ad completed.");
        GameEvent.OnRewardAdCompleted?.Invoke();

    }
}
