using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    Board board;
    BoardState currentState;


    public void SetUp(Board newBoard)
    {
        board = newBoard;
        currentState = new();
    }


    public void SetUpPieces()
    {
        
    }


    public void AgentMove()
    {
        Debug.Log("Agent Starts");
        currentState.SetUp(board);
    }
}
