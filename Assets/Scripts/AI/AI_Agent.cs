using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;


public class AI_Agent : MonoBehaviour
{
    [SerializeField]
    Board gameBoard;

    [SerializeField]
    PieceManager pieceManager;

    BoardData board;
    SearchSpace searchSpace;

    public List<CellData> playerPieces = new();
    public List<CellData> agentPieces = new();
    

    public void AgentMove()
    {
        // create an instance that represents the board from the agent's perspective
        board = new();
        board.CopyCells(gameBoard.allCells);


        // scan the board state of the current board
        BoardState currentBoard = new();
        currentBoard.SetUp(board);


        // initialize search space with the children board states
        searchSpace = new();
        searchSpace.Initialize(currentBoard); 
        searchSpace.GenerateChildBoardStates(currentBoard, true, 0, 3, 15, 15);


        // get the best score among all board states
        KeyValuePair<float, int> bestMove = FindBestScore(currentBoard, 0);
        BoardState favorableBoard = FindBestMove(currentBoard, bestMove.Value); 
        AgentMovePiece(favorableBoard);


        // end turn
        pieceManager.SwitchSides(Color.black);
        EndTurn();
    }


    private KeyValuePair<float, int> FindBestScore(BoardState currentBoard, int depth)
    {
        float saveScore = 0;
        int index = 0;

        for (int i = 0; i < currentBoard.children.Count; i++)
        {
            float childScore = FindBestScore(currentBoard.children[i], depth + 1).Key;
            float tempScore = currentBoard.children[i].score + childScore;

            if (tempScore > saveScore)
            {
                saveScore = tempScore;
                index = i;
            }
        }

        KeyValuePair<float, int> bestMove = new(saveScore, index);
        return bestMove;
    }


    private BoardState FindBestMove(BoardState currentBoard, int index)
    {
        //Debug.Log("current board moves: " + currentBoard.children.Count);
        //Debug.Log("index: " + index);
        return currentBoard.children[index];
    }


    private void AgentMovePiece(BoardState favourableBoard)
    {
        KeyValuePair<CellData, CellData> move = favourableBoard.move;
        //Debug.Log("x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.column + " y: " + move.Value.row);

        gameBoard.allCells[move.Key.column, move.Key.row].currentPiece.targetCell = gameBoard.allCells[move.Value.column, move.Value.row];
        gameBoard.allCells[move.Key.column, move.Key.row].currentPiece.Move();
    }


    private void EndTurn()
    {
        board = null;
        searchSpace = null;
    }
}
