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
    public List<CellData> playerPieces;
    public List<Cell> agentPieces;
    public List<KeyValuePair<CellData,Cell>> playerMoves;

    public List<KeyValuePair<Cell, Cell>> agentMoves; // size can give openness score
    public List<KeyValuePair<Cell, Cell>> capturingPieces;

    public float offenseScore;
    public float defenseScore;
    public float opennessScore;
    public float sumRankOffense;


    public void SetUp(Board newBoard)
    {
        board = newBoard;
        playerPieces = new();
        agentPieces = new();
        playerMoves = new();
        agentMoves = new();

        offenseScore = 0;
        defenseScore = 0;
        opennessScore = 0;
        sumRankOffense = 0;

        ScanBoard();
        ScanAgentMoves();
        //ScanPlayerMoves();
        bool risk = isFlagAtRisk();

        Debug.Log("offense score:" + offenseScore);
        Debug.Log("defensive score:" + defenseScore);
        Debug.Log("openness score:" + opennessScore);


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
        List<Cell> openCells = new();

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
                                KeyValuePair<Cell, Cell> move = new(cell, board.allCells[col, row]);
                                agentMoves.Add(move);

                                if (!openCells.Contains(board.allCells[col, row])) // check list if cell is not yet an element 
                                {
                                    openCells.Add(board.allCells[col, row]);
                                    opennessScore++;
                                }

                                if (board.allCells[col, row].currentPiece != null && board.allCells[col, row].currentPiece.color == Color.white)
                                {
                                    KeyValuePair<Cell, Cell> capturingPiece = new(cell, board.allCells[col, row]);
                                    capturingPieces.Add(capturingPiece);

                                    offenseScore++;
                                }

                                
                            }
                        }
                    }
                }
            }
        }

        openCells.Clear();
        //Debug.Log(offenseScore);
    }


    private void ScanPlayerMoves()
    {
        List<Cell> agentCells = new();

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
                                KeyValuePair<CellData, Cell> playerMove = new(cell, board.allCells[col, row]);
                                playerMoves.Add(playerMove);

                                if (board.allCells[col, row].currentPiece != null && board.allCells[col, row].currentPiece.color == Color.black && !agentCells.Contains(board.allCells[col, row]))
                                {
                                    agentCells.Add(board.allCells[col, row]);
                                    defenseScore++;
                                }

                                Debug.Log(col + " " + row);
                            }
                        }
                    }
                }
            }
        }

        agentCells.Clear();
        //Debug.Log(defenseScore);
    }

    private bool isFlagAtRisk()
    {
        int flagRow = 0, flagCol = 0;

        foreach (Cell cell in agentPieces)
        {
            if (cell.currentPiece.pieceType == PieceType.Flag)
            {
                flagRow = cell.boardPosition.x;
                flagCol = cell.boardPosition.y;
            }

        }

        if (board.allCells[flagRow + 1, flagCol].currentPiece != null && board.allCells[flagRow + 1, flagCol].currentPiece.color == Color.white)
        {
            return true;
        }

        if (board.allCells[flagRow - 1, flagCol].currentPiece != null && board.allCells[flagRow - 1, flagCol].currentPiece.color == Color.white)
        {
            return true;
        }

        if (board.allCells[flagRow, flagCol + 1].currentPiece != null && board.allCells[flagRow, flagCol + 1].currentPiece.color == Color.white)
        {
            return true;
        }

        if (board.allCells[flagRow, flagCol - 1].currentPiece != null && board.allCells[flagRow, flagCol - 1].currentPiece.color == Color.white)
        {
            return true;
        }

        return false;
    }

    private void CalculateHeuristic()
    {
        
    }

    private float GetPieceHeuristic(PieceType type)
    {
        switch (type)
        {
            case PieceType.General5:
                return 7.8f;
     
            case PieceType.General4:
                return 6.95f;
      
            case PieceType.General3:
                return 6.15f;
     
            case PieceType.General2:
                return 5.4f;
    
            case PieceType.General1:
                return 5.7f;
       
            case PieceType.Colonel:
                return 4.05f;
           
            case PieceType.LtColonel:
                return 3.45f;
          
            case PieceType.Major:
                return 2.9f;
          
            case PieceType.Captain:
                return 2.4f;
          
            case PieceType.Lieutentant1:
                return 1.95f;

            case PieceType.Lieutentant2:
                return 1.55f;
        
            case PieceType.Sergeant:
                return 1.2f;
       
            case PieceType.Private:
                return 1.37f;
       
            case PieceType.Spy:
                return 7.5f;
        
            case PieceType.Flag:
                return 0f;
              
            case PieceType.Unknown:
                return 0;
             
        }
        return 0f;
    }

}
