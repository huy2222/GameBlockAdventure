using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public static class SettingsData
{
    private const string MusicKey = "Music";
    private const string SFXKey = "SoundEffects";

    public static bool MusicEnabled
    {
        get => PlayerPrefs.GetInt(MusicKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
            PlayerPrefs.Save();
        } 
    }
    public static bool SFXEnabled
    {
        get => PlayerPrefs.GetInt(SFXKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(SFXKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
