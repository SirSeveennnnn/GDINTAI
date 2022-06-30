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

        GenerateChildBoardStates(currentBoard, true, 0, 2, 7, 8);
        //GenerateChildBoardStatesCapture(currentBoard, true, 0, 2, 7, 8);
        float bestScore = FindBestScore(currentBoard);
        //Debug.Log("BestScore: " + bestScore);

        //BoardState favorableBoard = FindBestMove(currentBoard, moveIndex);
        //AgentMovePiece(favorableBoard);

        pieceManager.SwitchSides(Color.black);
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
                KeyValuePair<Cell, Cell> move = currentBoard.agentMoves[index];
                //Debug.Log("Agent  " + count + "  x: " + move.Key.boardPosition.x + " y: " + move.Key.boardPosition.y + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


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
                KeyValuePair<CellData, Cell> move = currentBoard.playerMoves[index];
                //Debug.Log("Player    " + count + "  x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


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
                KeyValuePair<Cell, Cell> move = currentBoard.capturingPieces[index];
                //Debug.Log("Agent  " + count + "  x: " + move.Key.boardPosition.x + " y: " + move.Key.boardPosition.y + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


                BoardData possibleBoard = new();
                possibleBoard.CopyCells(board.allCells);

                int row, col, moveRow, moveCol;
                row = move.Key.boardPosition.y;
                col = move.Key.boardPosition.x;
                moveRow = move.Value.boardPosition.y;
                moveCol = move.Value.boardPosition.x;

                BasePiece piece = possibleBoard.allCells[col, row].currentPiece;
                possibleBoard.allCells[col, row].currentPiece = null;
                possibleBoard.allCells[moveCol, moveRow].currentPiece = piece;

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
                KeyValuePair<CellData, Cell> move = currentBoard.playerMoves[index];
                //Debug.Log("Player    " + count + "  x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


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
                boardState.parent = currentBoard;
                currentBoard.children.Add(boardState);
                GenerateChildBoardStatesCapture(boardState, !isAgent, currentDepth + 1, maxDepth, randAgent, randPlayer);
            }
        }

        storedIndices.Clear();
        storedIndices = null;
    }

    private void AgentMovePiece(BoardState favourableBoard)
    {
        KeyValuePair<Cell, Cell> move = favourableBoard.move;
        Debug.Log("x: " + move.Key.boardPosition.x + " y: " + move.Key.boardPosition.y + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);

        //gameBoard.allCells[move.Key.boardPosition.x, move.Key.boardPosition.y].currentPiece.targetCell = gameBoard.allCells[move.Value.boardPosition.x, move.Value.boardPosition.y];
        //gameBoard.allCells[move.Key.boardPosition.x, move.Key.boardPosition.y].currentPiece.Move();
        pieceManager.SwitchSides(Color.black);

        moveIndex = 0;

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

    public void CreateSearchSpace()
    {
        SearchSpace searchSpace = new();
        searchSpace.Initialize(board, 5, 5, 2);
    }
}
