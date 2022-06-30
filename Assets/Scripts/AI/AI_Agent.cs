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
        //Debug.Log("Agent Starts");
        //CreateSearchSpace();

        BoardState currentBoard = new();
        currentBoard.SetUp(board);

        GenerateChildBoardStates(currentBoard, true, 0, 2);
    }

    private void GenerateChildBoardStates(BoardState currentBoard, bool isAgent, int currentDepth, int maxDepth)
    {

        if (isAgent && currentDepth < maxDepth)
        {
            foreach (KeyValuePair<Cell, Cell> move in currentBoard.agentMoves)
            {
                Debug.Log("Agent   x: " + move.Key.boardPosition.x + " y: " + move.Key.boardPosition.y + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);

                BoardData possibleBoard = new();
                possibleBoard.CopyCells(board.allCells);

                int row, col, moveRow, moveCol;
                row = move.Key.boardPosition.y;
                col = move.Key.boardPosition.x;
                moveRow = move.Value.boardPosition.y;
                moveCol = move.Value.boardPosition.x;

                BasePiece piece = possibleBoard.allCells[col,row].currentPiece;
                possibleBoard.allCells[col,row].currentPiece = null;
                possibleBoard.allCells[moveCol, moveRow].currentPiece = piece;

                BoardState boardState = new();
                boardState.SetUp(possibleBoard);
                currentBoard.children.Add(boardState);
                GenerateChildBoardStates(boardState, !isAgent, currentDepth + 1, maxDepth);
            }
        }
        else if (!isAgent && currentDepth < maxDepth)
        {
            foreach (KeyValuePair<CellData, Cell> move in currentBoard.playerMoves)
            {
                Debug.Log("Player   x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);

                BoardData possibleBoard = new();
                possibleBoard.CopyCells(board.allCells);

                int row, col, moveRow, moveCol;
                row = move.Key.row;
                col = move.Key.column;
                moveRow = move.Value.boardPosition.y;
                moveCol = move.Value.boardPosition.x;

                BasePiece piece = possibleBoard.allCells[col, row].currentPiece;
                possibleBoard.allCells[col, row].currentPiece = null;
                possibleBoard.allCells[moveCol, moveRow].currentPiece = piece;

                BoardState boardState = new();
                boardState.SetUp(possibleBoard);
                currentBoard.children.Add(boardState);
                GenerateChildBoardStates(boardState, !isAgent, currentDepth + 1, maxDepth);
            }
        }
    }


    public void CreateSearchSpace()
    {
        SearchSpace searchSpace = new();
        searchSpace.Initialize(board, 5, 5, 2);
    }
}
