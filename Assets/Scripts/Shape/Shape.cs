using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject squareShape;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0, 700f);

    public ShapeData CurrentShapeData;

    public int TotalSquareNumber { get; set; }

    private List<GameObject> currentShapeSquares = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    private bool _shapeDraggable = true;
    private Canvas _canvas;
    private Vector3 _startPosition;
    private bool _shapeActive = true;
    private Vector2 _startAnchorMin;
    private Vector2 _startAnchorMax;
    private Vector2 _startPivot;

    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _shapeDraggable = true;
        _shapeActive = true;
        _startPosition = _transform.localPosition;
        _startAnchorMin = _transform.anchorMin;
        _startAnchorMax = _transform.anchorMax;
        _startPivot = _transform.pivot;
    }

    void OnEnable()
    {
        GameEvent.SetShapeInactive += SetShapeInactive;
        GameEvent.MoveShapeToStartPosition += MoveShapeToStartPosition;
    }
    void OnDisable()
    {
        GameEvent.SetShapeInactive -= SetShapeInactive;
        GameEvent.MoveShapeToStartPosition -= MoveShapeToStartPosition;
    }

    void Start()
    {

    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }
    public bool IsAnyOfShapeSquareActive()
    {
        foreach (var square in currentShapeSquares)
        {
            if (square.activeSelf)
            {
                return true;
            }
        }
        return false;
    }


    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        // Reset anchor, pivot và vị trí về ban đầu
        _transform.anchorMin = _startAnchorMin;
        _transform.anchorMax = _startAnchorMax;
        _transform.pivot = _startPivot;
        _transform.localPosition = _startPosition;
        _shapeActive = true;


        foreach (var square in currentShapeSquares)
        {
            Destroy(square);
        }

        currentShapeSquares.Clear();  // Xóa các ô vuông cũ khỏi danh sách

        TotalSquareNumber = GetNumberOfSquare(shapeData);
        while (currentShapeSquares.Count < TotalSquareNumber)
        {
            var square = Instantiate(squareShape, this.transform);
            currentShapeSquares.Add(square);
        }

        foreach (var square in currentShapeSquares)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShape.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);


        int currentIndexInList = 0;
        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    currentShapeSquares[currentIndexInList].SetActive(true);
                    currentShapeSquares[currentIndexInList].GetComponent<RectTransform>().localPosition =
                    new Vector2(GetXPositionForShapeSquare(shapeData, column, moveDistance),
                    GetYPositionForShapeSquare(shapeData, row, moveDistance));
                    currentIndexInList++;
                }
            }
        }
    }
    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        return -(row - (shapeData.rows - 1) / 2f) * moveDistance.y;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        return (column - (shapeData.columns - 1) / 2f) * moveDistance.x;
    }

    private int GetNumberOfSquare(ShapeData shapeData)
    {
        int number = 0;
        foreach (var row in shapeData.board)
        {
            foreach (var column in row.column)
            {
                if (column) number++;
            }
        }
        return number;
    }

    public void SetShapeInactive()
    {
        if (IsOnStartPosition() == false && IsAnyOfShapeSquareActive())
        {
            foreach (var square in currentShapeSquares)
            {
                square.SetActive(false);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvent.CheckIfShapeCanBePlaced();
    }


    public void MoveShapeToStartPosition()
    {
        _transform.anchorMin = _startAnchorMin;
        _transform.anchorMax = _startAnchorMax;
        _transform.pivot = _startPivot;
        _transform.localPosition = _startPosition;
    }
}
