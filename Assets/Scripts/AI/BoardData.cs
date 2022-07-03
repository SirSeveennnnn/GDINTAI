using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData
{
    public CellData[,] allCells;

    public void CopyCells(Cell[,] copyCell)
    {
        allCells = new CellData[9, 8];

        for (int i = 0; i < copyCell.GetLength(0); i++)
        {
            for (int j = 0; j < copyCell.GetLength(1); j++)
            {
                CellData temp = new();
                temp.row = copyCell[i, j].boardPosition.y;
                temp.column = copyCell[i, j].boardPosition.x;


                if (copyCell[i, j].currentPiece != null && copyCell[i, j].currentPiece.color == Color.black)
                {
                    temp.color = Color.black;
                    temp.pieceType = copyCell[i, j].currentPiece.pieceType;
                    temp.pieceID = copyCell[i, j].currentPiece.ID;
                }
                else if (copyCell[i, j].currentPiece != null && copyCell[i, j].currentPiece.color == Color.white)
                {
                    temp.color = Color.white;
                    temp.pieceType = PieceType.Unknown;
                    temp.pieceID = copyCell[i, j].currentPiece.ID;
                }


                allCells[i, j] = temp;
            }
        }
    }


    public void CopyCellData(CellData[,] copyCell)
    {
        allCells = new CellData[9, 8];

        for (int i = 0; i < copyCell.GetLength(0); i++)
        {
            for (int j = 0; j < copyCell.GetLength(1); j++)
            {
                CellData temp = new();
                temp.row = copyCell[i, j].row;
                temp.column = copyCell[i, j].column;
                temp.pieceID = copyCell[i, j].pieceID;

                if (copyCell[i, j].pieceID != -1 && copyCell[i, j].color == Color.black)
                {
                    temp.color = Color.black;
                    temp.pieceType = copyCell[i, j].pieceType;
                }
                else if (copyCell[i, j].pieceID != -1 && copyCell[i, j].color == Color.white)
                {
                    temp.color = Color.white;
                    temp.pieceType = PieceType.Unknown;
                }

                allCells[i, j] = temp;
            }
        }
    }
}
