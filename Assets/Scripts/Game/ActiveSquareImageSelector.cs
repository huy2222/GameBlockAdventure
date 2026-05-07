using UnityEngine;
using UnityEngine.UI;

public class ActiveSquareImageSelector : MonoBehaviour
{
    public SquareTextureData squareTextureData;
    public bool updateImageOnReachedThreshold = false;

    private void OnEnable()
    {
        UpdateSquareColorBaseOnCurrentPoints();
        if(updateImageOnReachedThreshold)
        {
            GameEvent.UpdateSquareColor += UpdateSquareColor;
        }
    }
    private void OnDisable()
    {
        if(updateImageOnReachedThreshold)
        {
            GameEvent.UpdateSquareColor -= UpdateSquareColor;
        }
    }


    private void UpdateSquareColor(SquareColor squareColor)
    {
        foreach (var squareTexture in squareTextureData.activeSquareTextures)
        {
            if (squareColor == squareTexture.color)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }
    private void UpdateSquareColorBaseOnCurrentPoints()
    {
        foreach (var squareTexture in squareTextureData.activeSquareTextures)
        {
            if (squareTextureData.currentColor == squareTexture.color)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }
}