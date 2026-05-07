using UnityEngine;
using UnityEngine.UI;

public class ShapeSquare : MonoBehaviour
{
    public Image occupiedImage;

    void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }

    public void DeactivateShape()
    {
        gameObject.SetActive(false);
    }

    public void ActivateShape()
    {
        gameObject.SetActive(true);
    }

    public void SetOccupied(bool isOccupied)
    {
        occupiedImage.gameObject.SetActive(isOccupied);
    }

    
    
}
