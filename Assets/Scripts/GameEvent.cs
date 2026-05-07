using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static Action RequestNewShapes;
    public static Action CheckIfShapeCanBePlaced;
    public static Action SetShapeInactive;
    public static Action MoveShapeToStartPosition;
    public static Action<int> AddScore;
    public static Action<int, int> UpdateBestScoreBar;
    public static Action GameOver;
    public static Action CheckIfPlayerLost;
    public static Action SaveCurrentScore;
    public static Action<SquareColor> UpdateSquareColor;
    public static Action OnRewardAdCompleted;
    public static Action ShowCongratulationWritings;
    public static Action<List<SquareColor>> ShowBonusScreen;
    public static Action<bool> OnMusicChanged;
    public static Action<bool> OnSFXChanged;
    public static Action<SFXType> PlaySoundEffect;
}
