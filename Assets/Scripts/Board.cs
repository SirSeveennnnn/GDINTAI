using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
};

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;

    [SerializeField]
    public Cell[,] allCells = new Cell[9, 8];


    public void Create()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject newCell = Instantiate(cellPrefab, transform);

                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((i * 100) + 50, (j * 100) + 50);



                allCells[i, j] = newCell.GetComponent<Cell>();
                allCells[i, j].Setup(new Vector2Int(i, j), this);
            }
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                allCells[i, j].GetComponent<Image>().color = new Color32(230, 220, 186, 255);

            }
        }
    }

    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        if (targetX < 0 || targetX > 8)
        {
            return CellState.OutOfBounds;
        }

        if (targetY < 0 || targetY > 7)
        {
            return CellState.OutOfBounds;
        }

        Cell targetCell = allCells[targetX, targetY];

        if (targetCell.currentPiece != null)
        {
            if (checkingPiece.color == targetCell.currentPiece.color)
            {
                return CellState.Friendly;
            }

            if (checkingPiece.color != targetCell.currentPiece.color)
            {
                return CellState.Enemy;
            }

        }

        return CellState.Free;
    }

    public Cell GetCell(int row, int col)
    {
        return allCells[row, col];
    }

    
}
