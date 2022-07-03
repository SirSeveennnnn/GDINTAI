using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData
{
    public int pieceID = -1;
    public Color color;             //Team Color
    public PieceType pieceType;     //Piece

    public int row;                 //Row
    public int column;              //Column

    public int nConsecutiveMoves = 0;
}
