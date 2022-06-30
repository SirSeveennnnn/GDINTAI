using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchSpace : MonoBehaviour
{
    BoardData board;
    BoardState rootState;


    public void Initialize(BoardData newBoard, int nAgentMoves, int nPlayerMoves, int depth)
    {
        board = newBoard;
        rootState = new();

        rootState.SetUp(board);
    }


    private void SimulateBoardState(int currentDepth, int maxDepth)
    {

    }
}
