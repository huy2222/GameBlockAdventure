using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;


    void OnEnable()
    {
        GameEvent.RequestNewShapes += RequestNewShape;
    }
    void OnDisable()
    {
        GameEvent.RequestNewShapes -= RequestNewShape;
    }

    void Start()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }


    public Shape GetCurrentSelectShape()
    {
        foreach (var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }
        return null;
    }

    public void RequestNewShape()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }
}
