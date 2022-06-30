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

    private Board board;
    private List<CellData> playerPieces;
    private List<Cell> playerMoves;
    public List<Cell> agentPieces;
    public List<Cell> agentMoves; // size can give openness score

    private float offenseScore;
    private float defenseScore;


    public void SetUp(Board newBoard)
    {
        board = newBoard;
        playerPieces = new();
        agentPieces = new();
        playerMoves = new();
        agentMoves = new();

        offenseScore = 0;
        defenseScore = 0;

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
        foreach (Cell cell in agentPieces)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i + j == -1 || i + j == 1)
                    {
                        int row = cell.boardPosition.y + i;
                        int col = cell.boardPosition.x + j;

                        if (row >= 0  && row < 8 && col >= 0 && col < 9) // check if out of bounds
                        {
                            if (board.allCells[col, row].currentPiece == null || board.allCells[col, row].currentPiece.color != Color.black) // check what the cell contains
                            {
                                if (!agentMoves.Contains(board.allCells[col, row])) // check list if cell is not yet an element 
                                {
                                    agentMoves.Add(board.allCells[col, row]);
                                    //Debug.Log(col + " " + row);
                                }

                                if (board.allCells[col, row].currentPiece != null && board.allCells[col, row].currentPiece.color == Color.white)
                                {
                                    offenseScore++;
                                }
                            }
                        }
                    }
                }
            }
        }

        //Debug.Log(offenseScore);
    }


    private void ScanPlayerMoves()
    {
        foreach (CellData cell in playerPieces)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i + j == -1 || i + j == 1)
                    {
                        int row = cell.row + i;
                        int col = cell.column + j;

                        if (row >= 0 && row < 8 && col >= 0 && col < 9)
                        {
                            if (board.allCells[col, row].currentPiece == null || board.allCells[col, row].currentPiece.color != Color.white)
                            {
                                if (!playerMoves.Contains(board.allCells[col, row])) // check list if cell is not yet an element 
                                {
                                    playerMoves.Add(board.allCells[col, row]);
                                    Debug.Log(col+ " " + row);
                                }

                                if (board.allCells[col, row].currentPiece != null && board.allCells[col, row].currentPiece.color == Color.black)
                                {
                                    defenseScore++;
                                }
                            }
                        }
                    }
                }
            }
        }

        //Debug.Log(defenseScore);
    }



    private void CalculateHeuristic()
    {
        
    }

}
