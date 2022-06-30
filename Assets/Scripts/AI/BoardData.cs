using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData 
{
    public Cell[,] allCells;


    public void CopyCells(Cell[,] copyCell)
    {
        allCells = copyCell;
    }
}
