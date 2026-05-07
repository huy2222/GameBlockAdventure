using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;


    [SerializeField] private AudioClip placeShapeClip;
    [SerializeField] private AudioClip clearLineClip;
    [SerializeField] private AudioClip gameOverClip;
    private bool sfxEnabled = true;
    private Dictionary<SFXType, AudioClip> sfxClips;

    private static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxClips = new Dictionary<SFXType, AudioClip>
    {
        { SFXType.PlaceShape, placeShapeClip },
        { SFXType.ClearLine, clearLineClip },
        { SFXType.GameOver, gameOverClip }

    };
    }
    void Start()
    {
        musicSource.clip = musicClip;
        if (SettingsManager.Instance.musicEnabled)
        {
            musicSource.Play();
        }
        if(SettingsManager.Instance.sfxEnabled)
        {
            sfxEnabled = true;
        }
    }
    void OnEnable()
    {
        GameEvent.OnMusicChanged += HandleMusicChange;
        GameEvent.OnSFXChanged += HandleSFXChange;
        GameEvent.PlaySoundEffect += PlaySoundEffect;
    }



    void OnDisable()
    {
        GameEvent.OnMusicChanged -= HandleMusicChange;
        GameEvent.OnSFXChanged -= HandleSFXChange;
        GameEvent.PlaySoundEffect -= PlaySoundEffect;
    }
    private void HandleMusicChange(bool isOn)
    {
        if (isOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
    }
    private void HandleSFXChange(bool obj)
    {
        sfxEnabled = obj;
    }
    private void PlaySoundEffect(SFXType type)
    {
        if (sfxEnabled)
        {
            sfxSource.PlayOneShot(sfxClips[type]);
        }
    }

}
