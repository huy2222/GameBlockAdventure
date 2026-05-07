using System;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite bgOn;
    [SerializeField] private Sprite bgOff;

    [SerializeField] private float moveDistance = 30f;

    public SettingType settingType;
    private Button button;

    private bool isOn;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSetting);
    }
    void Start()
    {
        isOn = (settingType == SettingType.Music) ? SettingsManager.Instance.musicEnabled : SettingsManager.Instance.sfxEnabled;
        UpdateUI();
        MoveIcon();
    }

    private void ToggleSetting()
    {
        if (settingType == SettingType.Music)
        {
            SettingsManager.Instance.ToggleMusic();
            isOn = SettingsManager.Instance.musicEnabled;
        }
        else if (settingType == SettingType.SoundEffects)
        {
            SettingsManager.Instance.ToggleSfx();
            isOn = SettingsManager.Instance.sfxEnabled;
        }
        UpdateUI();
        MoveIcon();
    }

    private void UpdateUI()
    {
        backgroundImage.sprite = isOn ? bgOn : bgOff;
    }
    private void MoveIcon()
    {
        float targetX = isOn ? moveDistance : -moveDistance;
        icon.transform.localPosition = new Vector3(targetX, icon.transform.localPosition.y, icon.transform.localPosition.z);
    }
}
