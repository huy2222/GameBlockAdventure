using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public bool musicEnabled;
    public bool sfxEnabled;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadSettings()
    {
        musicEnabled = SettingsData.MusicEnabled;
        sfxEnabled = SettingsData.SFXEnabled;
    }
     public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        SettingsData.MusicEnabled = musicEnabled;
        GameEvent.OnMusicChanged?.Invoke(musicEnabled);
    }

    public void ToggleSfx()
    {
        sfxEnabled = !sfxEnabled;
        SettingsData.SFXEnabled = sfxEnabled;
        GameEvent.OnSFXChanged?.Invoke(sfxEnabled);
    }
}
