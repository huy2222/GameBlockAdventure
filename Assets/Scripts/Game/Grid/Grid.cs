using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int columns = 0;
    public int rows = 0;
    public float squareScale = 0.5f;
    public GameObject gridSquare;
    private List<GameObject> _gridSquares = new List<GameObject>();
    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    public float everySquareOffset = 0.0f;
    public float squaresGap = 0.1f;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public ShapeStorage ShapeStorage;
    private LineIndicator _lineIndicator;

    public SquareTextureData squareTextureData;

    // mau hoat dong hien tai
    private SquareColor currentActiveSquareColor = SquareColor.NotSet;
    private List<SquareColor> colorsInTheGrid = new List<SquareColor>();

    void OnEnable()
    {
        GameEvent.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvent.CheckIfPlayerLost += CheckIfPlayerLost;
        GameEvent.UpdateSquareColor += OnUpdateSquareColor;
    }

    void OnDisable()
    {
        GameEvent.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvent.CheckIfPlayerLost -= CheckIfPlayerLost;
        GameEvent.UpdateSquareColor -= OnUpdateSquareColor;
    }


    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        currentActiveSquareColor = squareTextureData.activeSquareTextures[0].color;
        CreateGrid();
    }

    // thay đổi màu
    private void OnUpdateSquareColor(SquareColor color)
    {
        currentActiveSquareColor = color;
    }

    // tao luoi
    private void CreateGrid()
    {
        SpawnGridSquare();
        SetGridSquarePosition();
    }
    void SpawnGridSquare()
    {
        int square_index = 0;
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                square_index++;
            }
        }
    }
    void SetGridSquarePosition()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_move = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();
        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (var square in _gridSquares)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0;
                // go to the next column
                column_number = 0;
                row_number++;
                row_move = false;
            }

            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }
            if (row_number > 0 && row_number % 3 == 0 && row_move == false)
            {
                row_move = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            column_number++;
        }
    }


    // check xem vi tri do dat hinh co hop le khong
    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.CanBeUseThisSquare() == true)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
        }
        // kiểm tra người dùng đang kéo khối nào
        var currentSelectShape = ShapeStorage.GetCurrentSelectShape();

        if (currentSelectShape == null)
        {
            return;
        }
        if (currentSelectShape.TotalSquareNumber == squareIndexes.Count)
        {
            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor);
            }
            CheckNumberOfShape();
            CheckIfAnyLineIsComplete();
            GameEvent.PlaySoundEffect?.Invoke(SFXType.PlaceShape);
        }
        else
        {
            GameEvent.MoveShapeToStartPosition?.Invoke();
        }
    }
    // kiemer tra sau khi dat shape con bao nhieu shape con lai de request shape moi
    private void CheckNumberOfShape()
    {
        var shapeLeft = 0;
        foreach (var shape in ShapeStorage.shapeList)
        {
            if (shape.IsAnyOfShapeSquareActive() && shape.IsOnStartPosition())
            {
                shapeLeft++;
            }
        }
        if (shapeLeft == 0)
        {
            GameEvent.RequestNewShapes?.Invoke();
        }
        else
        {
            GameEvent.SetShapeInactive?.Invoke(); // xoa shape da dat tren board
        }
    }

    private void CheckIfAnyLineIsComplete()
    {
        List<int[]> lines = new List<int[]>();
        // columns
        foreach (var column in _lineIndicator.columnIndex)
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));
        }
        // rows
        for (int row = 0; row < 9; row++)
        {
            List<int> data = new List<int>();
            for (int column = 0; column < 9; column++)
            {
                data.Add(_lineIndicator.line_data[row, column]);
            }
            lines.Add(data.ToArray());
        }
        // squares 3x3
        for (int square = 0; square < 9; square++)
        {
            List<int> data = new List<int>();
            for (int index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.square_data[square, index]);
            }
            lines.Add(data.ToArray());
        }

        colorsInTheGrid = GetAllSquareColorInTheGrid();

        var linesCompleted = CheckIfSquareAreCompleted(lines);
        var totalScore = linesCompleted * 10;
        var bonusScore = ShouldPlayColorBonusAnimation();
        if (linesCompleted > 0)
        {
            GameEvent.AddScore?.Invoke(totalScore + bonusScore);
            GameEvent.PlaySoundEffect?.Invoke(SFXType.ClearLine);
        }
        if (linesCompleted >= 2)
        {
            GameEvent.ShowCongratulationWritings?.Invoke();
        }



        GameEvent.CheckIfPlayerLost?.Invoke();
    }
    private List<SquareColor> GetAllSquareColorInTheGrid()
    {
        var colors = new List<SquareColor>();
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.SquareOccupied)
            {
                var color = gridSquare.GetCurrentSquareColor();
                if (colors.Contains(color) == false)
                {
                    colors.Add(color);
                }
            }
        }
        return colors;
    }
    // xu li bonus
    private int ShouldPlayColorBonusAnimation()
    {
        var colorsInTheGridAfterLineRemoved = GetAllSquareColorInTheGrid();
        List<SquareColor> colorsRemoved = new List<SquareColor>();
        foreach (var color in colorsInTheGrid)
        {
            if (colorsInTheGridAfterLineRemoved.Contains(color) == false && color != currentActiveSquareColor)
            {
                colorsRemoved.Add(color);
            }
        }
        if (colorsRemoved.Count == 0)
        {
            return 0;
        }
        // StartCoroutine(PlayBonusSequence(colorsRemoved));

            GameEvent.ShowBonusScreen?.Invoke(colorsRemoved);
        
        return 50* colorsRemoved.Count;

    }






    private int CheckIfSquareAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
        foreach (var line in data)
        {
            var lineCompleted = true;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied == false)
                {
                    lineCompleted = false;
                    break;
                }
            }
            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }
        // xoa cac dong da hoan thanh
        var linesCompleteNumber = 0;
        foreach (var line in completedLines)
        {
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.DeactivateSquare();
                comp.ClearOccupied();
            }
            linesCompleteNumber++;
        }
        return linesCompleteNumber;
    }


    private void CheckIfPlayerLost()
    {
        var validShapes = 0;
        for (var index = 0; index < ShapeStorage.shapeList.Count; index++)
        {
            var isActiveShape = ShapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
            if (CheckIfShapeCanBePlaceOnGrid(ShapeStorage.shapeList[index]) && isActiveShape)
            {
                validShapes++;
            }
        }
        if (validShapes == 0)
        {
            GameEvent.SaveCurrentScore?.Invoke();
            GameEvent.GameOver?.Invoke();
            GameEvent.PlaySoundEffect?.Invoke(SFXType.GameOver);
        }

    }

    private bool CheckIfShapeCanBePlaceOnGrid(Shape currentShape)  // kiem tra thua xem cac khoi con dat nx duoc khong
    {
        var currentShapeData = currentShape.CurrentShapeData;
        var shapeColumns = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        List<int> originalShapeSquareFillUpSquares = new List<int>();
        var squareIndex = 0;
        for (var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < shapeColumns; columnIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columnIndex])
                {
                    originalShapeSquareFillUpSquares.Add(squareIndex);
                }
                squareIndex++;
            }
        }
        if (currentShape.TotalSquareNumber != originalShapeSquareFillUpSquares.Count)
        {
            Debug.LogError("Lỗi dữ liệu hình dạng: số ô vuông điền vào không khớp với dữ liệu hình dạng gốc.");
            return false;
        }
        // BƯỚC 2: Lấy tất cả vị trí có thể đặt shape trên grid
        var squareList = GetAllSquareCombination(shapeColumns, shapeRows);
        // BƯỚC 3: Kiểm tra từng vị trí có thể đặt shape
        bool canPlaceShape = false;
        foreach (var number in squareList)
        {
            bool shapeCanBePlaceOnThBoard = true;
            foreach (var squareIndexToCheck in originalShapeSquareFillUpSquares)
            {
                // Kiểm tra xem ô vuông có thể đặt shape không
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    shapeCanBePlaceOnThBoard = false;
                    break;
                }
            }
            if (shapeCanBePlaceOnThBoard)
            {
                canPlaceShape = true;
                break;
            }
        }
        return canPlaceShape;
    }
    private List<int[]> GetAllSquareCombination(int columns, int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;
        while (lastRowIndex + (rows - 1) < 9)
        {
            var rowData = new List<int>();
            for (var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(_lineIndicator.line_data[row, column]);
                }
            }
            squareList.Add(rowData.ToArray());
            lastColumnIndex++;
            if (lastColumnIndex + (columns - 1) >= 9)
            {
                lastColumnIndex = 0;
                lastRowIndex++;
            }
            safeIndex++;
            if (safeIndex > 100)
            {
                Debug.LogError("Vòng lặp vô hạn trong GetAllSquareCombination. Kiểm tra logic điều kiện dừng.");
                break;
            }
        }
        return squareList;
    }



}