using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public List<CellData> cells;
    public Color teamColor;

    public List<float> scoreList;

    public BoardState parent;
    public List<BoardState> children;


    

    private void CalculateHeuristic()
    {
        
    }

}
