using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public Button openButton;
    public Button closeButton;

    public void SettingOpened()
    {
        openButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        closeButton.interactable = true;
    }
    public void SettingClosed()
    {
        openButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        openButton.interactable = true;
    }
}
