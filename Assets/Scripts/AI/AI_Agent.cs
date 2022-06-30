using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    Board board;
    KeyValuePair<Cell,Cell> move;

    public void SetUp(Board newBoard)
    {
        board = newBoard;
    }

    public void SetUpPieces()
    {
        
    }

    public void AgentMove()
    {
        Debug.Log("Agent Starts");
        CreateSearchSpace();
    }


    public void CreateSearchSpace()
    {
        SearchSpace searchSpace = new();
        searchSpace.Initialize(board, 5, 5, 2);
    }
}
