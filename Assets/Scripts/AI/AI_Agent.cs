using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;


public class AI_Agent : MonoBehaviour
{
    [SerializeField]
    Board gameBoard;

    BoardData board;

    [SerializeField]
    PieceManager pieceManager;

    int moveIndex;


    public void AgentMove()
    {
        board = new();
        board.CopyCells(gameBoard.allCells);

        BoardState currentBoard = new();
        currentBoard.SetUp(board);

        GenerateChildBoardStates(currentBoard, true, 0, 2, 7, 8);
        GenerateChildBoardStatesCapture(currentBoard, true, 0, 2, 7, 8);


        float bestScore = FindBestScore(currentBoard);
        BoardState favorableBoard = FindBestMove(currentBoard, moveIndex);
        AgentMovePiece(favorableBoard);


        pieceManager.SwitchSides(Color.black);
        EndTurn();
    }


    private void GenerateChildBoardStates(BoardState currentBoard, bool isAgent, int currentDepth, int maxDepth, int randAgent, int randPlayer)
    {
        List<int> storedIndices = new();
        System.Random rnd = new System.Random();

        int count = 0;
        int index = -1;

        if (isAgent && currentDepth < maxDepth)
        {
            while (count < randAgent && storedIndices.Count < currentBoard.agentMoves.Count)
            {
                do
                {
                    index = rnd.Next(currentBoard.agentMoves.Count);
                } while (storedIndices.Contains(index));

                count += 1;
                storedIndices.Add(index);
                KeyValuePair<CellData, CellData> move = currentBoard.agentMoves[index];
                //Debug.Log("Agent  " + count + "  x: " + move.Key.boardPosition.x + " y: " + move.Key.boardPosition.y + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(board.allCells);

                int row, col, moveRow, moveCol;
                row = move.Key.row;
                col = move.Key.column;
                moveRow = move.Value.row;
                moveCol = move.Value.column;

                int piece = possibleBoard.allCells[col,row].pieceID;
                possibleBoard.allCells[col,row].pieceID = -1;
                possibleBoard.allCells[moveCol, moveRow].pieceID = piece;

                BoardState boardState = new();
                boardState.move = move;
                boardState.SetUp(possibleBoard);
                boardState.parent = currentBoard;
                currentBoard.children.Add(boardState);
                GenerateChildBoardStates(boardState, !isAgent, currentDepth + 1, maxDepth, randAgent, randPlayer);
            }
        }
        else if (!isAgent && currentDepth < maxDepth)
        {
            while (count < randPlayer && storedIndices.Count < currentBoard.playerMoves.Count)
            {
                do
                {
                    index = rnd.Next(currentBoard.playerMoves.Count);
                } while (storedIndices.Contains(index));

                count += 1;
                storedIndices.Add(index);
                KeyValuePair<CellData, CellData> move = currentBoard.playerMoves[index];
                //Debug.Log("Player    " + count + "  x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(board.allCells);

                int row, col, moveRow, moveCol;
                row = move.Key.row;
                col = move.Key.column;
                moveRow = move.Value.row;
                moveCol = move.Value.column;

                int piece = possibleBoard.allCells[col, row].pieceID;
                possibleBoard.allCells[col, row].pieceID = -1;
                possibleBoard.allCells[moveCol, moveRow].pieceID = piece;

                BoardState boardState = new();
                boardState.SetUp(possibleBoard);
                boardState.parent = currentBoard;
                currentBoard.children.Add(boardState);
                GenerateChildBoardStates(boardState, !isAgent, currentDepth + 1, maxDepth, randAgent, randPlayer);
            }
        }

        storedIndices.Clear();
        storedIndices = null;
    }

    
    private void GenerateChildBoardStatesCapture(BoardState currentBoard, bool isAgent, int currentDepth, int maxDepth, int randAgent, int randPlayer)
    {
        List<int> storedIndices = new();
        System.Random rnd = new System.Random();

        int count = 0;
        int index = -1;

        if (isAgent && currentDepth < maxDepth)
        {
            while (count < randAgent && storedIndices.Count < currentBoard.capturingPieces.Count)
            {
                do
                {
                    index = rnd.Next(currentBoard.agentMoves.Count);
                } while (storedIndices.Contains(index));

                count += 1;
                storedIndices.Add(index);
                KeyValuePair<CellData, CellData> move = currentBoard.capturingPieces[index];
                //Debug.Log("Agent  " + count + "  x: " + move.Key.boardPosition.x + " y: " + move.Key.boardPosition.y + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(board.allCells);

                int row, col, moveRow, moveCol;
                row = move.Key.row;
                col = move.Key.column;
                moveRow = move.Value.row;
                moveCol = move.Value.column;

                int piece = possibleBoard.allCells[col, row].pieceID;
                possibleBoard.allCells[col, row].pieceID = -1;
                possibleBoard.allCells[moveCol, moveRow].pieceID = piece;

                BoardState boardState = new();
                boardState.SetUp(possibleBoard);
                boardState.parent = currentBoard;
                currentBoard.children.Add(boardState);
                GenerateChildBoardStatesCapture(boardState, !isAgent, currentDepth + 1, maxDepth, randAgent, randPlayer);
            }
        }
        else if (!isAgent && currentDepth < maxDepth)
        {
            while (count < randPlayer && storedIndices.Count < currentBoard.playerMoves.Count)
            {
                do
                {
                    index = rnd.Next(currentBoard.playerMoves.Count);
                } while (storedIndices.Contains(index));

                count += 1;
                storedIndices.Add(index);
                KeyValuePair<CellData, CellData> move = currentBoard.playerMoves[index];
                //Debug.Log("Player    " + count + "  x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(board.allCells);

                int row, col, moveRow, moveCol;
                row = move.Key.row;
                col = move.Key.column;
                moveRow = move.Value.row;
                moveCol = move.Value.column;

                int piece = possibleBoard.allCells[col, row].pieceID;
                possibleBoard.allCells[col, row].pieceID = -1;
                possibleBoard.allCells[moveCol, moveRow].pieceID = piece;

                BoardState boardState = new();
                boardState.SetUp(possibleBoard);
                boardState.parent = currentBoard;
                currentBoard.children.Add(boardState);
                GenerateChildBoardStates(boardState, !isAgent, currentDepth + 1, maxDepth, randAgent, randPlayer);
            }
        }

        storedIndices.Clear();
        storedIndices = null;
    }
    

    private void AgentMovePiece(BoardState favourableBoard)
    {
        KeyValuePair<CellData, CellData> move = favourableBoard.move;
        Debug.Log("x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.column + " y: " + move.Value.row);

        gameBoard.allCells[move.Key.column, move.Key.row].currentPiece.targetCell = gameBoard.allCells[move.Value.column, move.Value.row];
        gameBoard.allCells[move.Key.column, move.Key.row].currentPiece.Move();

        moveIndex = -1;

    }
  
    private float FindBestScore(BoardState currentBoard)
    {
        float saveScore = 0;
       
        for (int i = 0; i < currentBoard.children.Count; i++)
        {
            float tempScore = currentBoard.children[i].score + FindBestScore(currentBoard.children[i]);
            if (tempScore > saveScore)
            {
                saveScore = tempScore;
                moveIndex = i;
            }
        }
        return saveScore;
    }

    private BoardState FindBestMove(BoardState currentBoard, int index)
    {
        return currentBoard.children[index];
    }

    private void EndTurn()
    {
        board = null;
    }
}
