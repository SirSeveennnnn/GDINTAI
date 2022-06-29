using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    private Board board;
    private List<CellData> playerPieces;
    private List<Cell> agentPieces;
    private List<Cell> agentMoves; // size can give openness score
    private List<Cell> playerMoves;


    public void SetUp(Board newBoard)
    {
        board = newBoard;
        playerPieces = new();
        agentPieces = new();
        playerMoves = new();
        agentMoves = new();
    }

    public void AgentMove()
    {
        Debug.Log("Agent Starts");
        //BoardState currentState;
        ScanBoard();
        ScanAgentMoves();
        ScanPlayerMoves();
    }


    
    private void ScanBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.allCells[i, j].currentPiece != null && board.allCells[i, j].currentPiece.color == Color.black)
                {
                    agentPieces.Add(board.allCells[i, j]);
                }
                else if (board.allCells[i, j].currentPiece != null && board.allCells[i, j].currentPiece.color == Color.white)
                {
                    CellData cell = new();
                    cell.color = Color.white;
                    cell.row = j;
                    cell.column = i;
                    cell.pieceType = PieceType.Unknown;

                    playerPieces.Add(cell);
                }
            }
        }
    }


    private void ScanAgentMoves()
    {
        int[] move = { -1, 1 };

        foreach (Cell cell in agentPieces)
        { 
            for (int i = 0; i < move.Length; i++)
            {
                int row = cell.boardPosition.y + move[i];
                int col = cell.boardPosition.x;

                if (row >= 0 && row < 8) // check if out of bounds
                {
                    if (board.allCells[col, row].currentPiece == null || board.allCells[col, row].currentPiece.color != Color.black) // check what the cell contains
                    {
                        if (!agentMoves.Contains(board.allCells[col, row])) // check list if cell is not yet an element 
                        {
                            agentMoves.Add(board.allCells[col, row]);
                            //Debug.Log(col + " " + row);
                        }
                    }
                }

                row = cell.boardPosition.y;
                col = cell.boardPosition.x + move[i];

                if (col >= 0 && col < 9) // check if out of bounds
                {
                    if (board.allCells[col, row].currentPiece == null || board.allCells[col, row].currentPiece.color != Color.black) // check what the cell contains
                    {
                        if (!agentMoves.Contains(board.allCells[col, row])) // check list if cell is not yet an element 
                        {
                            agentMoves.Add(board.allCells[col, row]);
                            //Debug.Log(col + " " + row);
                        }
                    }
                }
            }
        }
    }


    private void ScanPlayerMoves()
    {
        int[] move = { -1, 1 };

        foreach (CellData cell in playerPieces)
        {
            for (int i = 0; i < move.Length; i++)
            {
                if (cell.row + move[i] >= 0 && cell.row + move[i] < 8) // check if out of bounds
                {
                    if (board.allCells[cell.column, cell.row + move[i]].currentPiece == null || board.allCells[cell.column, cell.row + move[i]].currentPiece.color != Color.white) // check what the cell contains
                    {
                        if (!playerMoves.Contains(board.allCells[cell.column, cell.row + move[i]])) // check list if cell is not yet an element 
                        {
                            playerMoves.Add(board.allCells[cell.column, cell.row + move[i]]);
                            Debug.Log(cell.column + " " + (cell.row + move[i]));
                        }
                    }
                }


                if (cell.column + move[i] >= 0 && cell.column + move[i] < 9) // check if out of bounds
                {
                    if (board.allCells[cell.column + move[i], cell.row].currentPiece == null || board.allCells[cell.column + move[i], cell.row].currentPiece.color != Color.white) // check what the cell contains
                    {
                        if (!playerMoves.Contains(board.allCells[cell.column + move[i], cell.row])) // check list if cell is not yet an element 
                        {
                            playerMoves.Add(board.allCells[cell.column + move[i], cell.row]);
                            Debug.Log((cell.column + move[i]) + " " + cell.row);
                        }
                    }
                }
            }
        }
    }


    private void EndTurn()
    {
        agentPieces.Clear();
        playerPieces.Clear();
        agentMoves.Clear();
        playerMoves.Clear();
    }
}
