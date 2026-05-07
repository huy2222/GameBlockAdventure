using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image hoverImage;
    public Image activeImage;
    public Image normalImage;

    public List<Sprite> normalImages;
    private SquareColor currentSquareColor = SquareColor.NotSet;
    public SquareColor GetCurrentSquareColor() { 
        
        return currentSquareColor;
     }


    public bool SquareOccupied { get; set; }
    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    void Start()
    {
        SquareOccupied = false;
        Selected = false;
    }
    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (SquareOccupied == false)
        {
            Selected = true;
            hoverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied(true);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (SquareOccupied == false)
        {
            hoverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (SquareOccupied == false)
        {
            Selected = false;
            hoverImage.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied(false);
        }
    }


    public bool CanBeUseThisSquare()
    {
        return hoverImage.gameObject.activeSelf;
    }
    public void PlaceShapeOnBoard(SquareColor squareColor)
    {
        currentSquareColor = squareColor;
        ActivateSquare();
    }
    public void ActivateSquare()
    {
        hoverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }
    public void DeactivateSquare()
    {
        activeImage.gameObject.SetActive(false);
        currentSquareColor = SquareColor.NotSet;
    }
    public void ClearOccupied()
    {
        Selected = false;
        SquareOccupied = false;
        currentSquareColor = SquareColor.NotSet;
    }
}
