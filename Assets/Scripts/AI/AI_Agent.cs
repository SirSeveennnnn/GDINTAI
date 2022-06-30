using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    BoardData board;
    KeyValuePair<Cell,Cell> move;

    


    public void SetUp(Board newBoard)
    {
        board = new();
        board.CopyCells(newBoard.allCells);
    }

    public void AgentMove()
    {
        Debug.Log("Agent Starts");
        //CreateSearchSpace();

        BoardState currentBoard = new();
        currentBoard.SetUp(board);

        GenerateChildBoardStates(currentBoard);
    }

    private void GenerateChildBoardStates(BoardState currentBoard)
    {
        foreach (KeyValuePair<Cell, Cell> move in currentBoard.agentMoves)
        {
            //Debug.Log("x: " + move.Key.boardPosition.x + " y: " + move.Key.boardPosition.y + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);

            BoardData possibleBoard = new();
            possibleBoard.CopyCells(board.allCells);

            int row, col, moveRow, moveCol;
            row = move.Key.boardPosition.x;
            col = move.Key.boardPosition.y;
            moveRow = move.Value.boardPosition.x;
            moveCol = move.Value.boardPosition.y;

            BasePiece piece = possibleBoard.allCells[row,col].currentPiece;
            possibleBoard.allCells[row, col].currentPiece = null;
            possibleBoard.allCells[moveRow, moveCol].currentPiece = piece;

            BoardState boardState = new();
            boardState.SetUp(possibleBoard);
            currentBoard.children.Add(boardState);

        }
    }


    public void CreateSearchSpace()
    {
        SearchSpace searchSpace = new();
        searchSpace.Initialize(board, 5, 5, 2);
    }
}
