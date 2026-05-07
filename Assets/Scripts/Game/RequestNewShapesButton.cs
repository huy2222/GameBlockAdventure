using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestNewShapesButton : MonoBehaviour
{
    public int numberOfShapesToRequest = 3;
    public TextMeshProUGUI numberText;
    public Button requestNewShapesButton;

    void Start()
    {
        UpdateNumberText();
        requestNewShapesButton.interactable = false;
        requestNewShapesButton.onClick.AddListener(OnRequestNewShapesButtonClicked);
    }

    void OnEnable()
    {
        GameEvent.OnRewardAdCompleted += UnlockRequestButton;        
    }
    void OnDisable()
    {
        GameEvent.OnRewardAdCompleted -= UnlockRequestButton;
    }
    void UpdateNumberText()
    {
        numberText.text = numberOfShapesToRequest.ToString();
    }

    void OnRequestNewShapesButtonClicked()
    {
        if (numberOfShapesToRequest > 0)
        {
            numberOfShapesToRequest--;
            // xu li yeu cau tao hinh moi
            GameEvent.RequestNewShapes?.Invoke();
            GameEvent.CheckIfPlayerLost?.Invoke();
            UpdateUIButton();
        }
        UpdateNumberText();
    }
    void UpdateUIButton()
    {
        if (numberOfShapesToRequest <= 0)
        {
            LockRequestButton();
        }
    }

    void LockRequestButton()
    {
        requestNewShapesButton.interactable = false;
    }
    void UnlockRequestButton()
    {
        requestNewShapesButton.interactable = true;
    }
}
