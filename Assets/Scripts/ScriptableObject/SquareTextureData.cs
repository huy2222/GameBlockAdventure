using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
[System.Serializable]
public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public Sprite texture;
        public SquareColor color;  // enum 
    }
    public int thresholdValue;
    private const int StartThresholdValue = 10;

    public List<TextureData> activeSquareTextures;

    public SquareColor currentColor;
    private SquareColor _nextColor;


    private void Awake()
    {
        SetStartColor();
    }
    public int GetCurrentColorIndex()
    {
        var currentIndex = 0;
        for(int index = 0; index < activeSquareTextures.Count; index++)
        {
            if(activeSquareTextures[index].color == currentColor)
            {
                currentIndex = index;
                break;
            }
        }
        return currentIndex;
    }

    public void UpdateColors(int currentScore)
    {
        currentColor = _nextColor;
        var currentIndex = GetCurrentColorIndex();
        if(currentIndex == activeSquareTextures.Count - 1)
        {
            _nextColor = activeSquareTextures[0].color;
            return;
        }
        _nextColor = activeSquareTextures[currentIndex + 1].color;
        thresholdValue = StartThresholdValue + currentScore;
    }
    public void SetStartColor()
    {
        thresholdValue = StartThresholdValue;
        currentColor = activeSquareTextures[0].color;
        _nextColor = activeSquareTextures[1].color;
    }
}
