using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    private BoardData board = new();
    public BoardState parent;
    public List<BoardState> children = new();

    
    public List<CellData> playerPieces = new();
    public List<CellData> agentPieces = new();
    public List<KeyValuePair<CellData, CellData>> capturingPieces = new();


    public KeyValuePair<CellData, CellData> move;
    public List<KeyValuePair<CellData,CellData>> playerMoves = new();
    public List<KeyValuePair<CellData, CellData>> agentMoves = new(); 
    

    public float offenseScore = 0;
    public float defenseScore = 0;
    public float opennessScore = 0;
    public float score = 0;


    public void SetUp(BoardData newBoard)
    {
        board.CopyCellData(newBoard.allCells);

        ScanBoard();
        ScanAgentMoves();
        ScanPlayerMoves();
        FindOffensivePieces();
        bool risk = isFlagAtRisk();

        if (risk)
        {
            score = -999;
        }
        else
        {
            score = offenseScore + opennessScore - defenseScore;
        }

        //Debug.Log("offense score:" + offenseScore);
        //Debug.Log("defensive score:" + defenseScore);
        //Debug.Log("openness score:" + opennessScore);
        //Debug.Log("Final Score" + score);
    }


    private void ScanBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.allCells[i, j].pieceID != -1 && board.allCells[i, j].color == Color.black)
                {
                    agentPieces.Add(board.allCells[i, j]);
                }
                else if (board.allCells[i, j].pieceID != -1 && board.allCells[i, j].color == Color.white)
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

    private void FindOffensivePieces()
    {
        foreach (CellData cell in agentPieces)
        {
            if (cell.row < 4)
            {
                offenseScore += 1 * GetPieceHeuristic(cell.pieceType);
            }
        }
    }

    private void ScanAgentMoves()
    {
        List<CellData> openCells = new();

        foreach (CellData cell in agentPieces)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i + j == -1 || i + j == 1)
                    {
                        int row = cell.row + i;
                        int col = cell.column + j;

                        if (row >= 0  && row < 8 && col >= 0 && col < 9) // check if out of bounds
                        {
                            if (board.allCells[col, row].pieceID == -1 || board.allCells[col, row].color != Color.black) // check what the cell contains
                            {
                                KeyValuePair<CellData, CellData> move = new(cell, board.allCells[col, row]);
                                agentMoves.Add(move);

                                if (!openCells.Contains(board.allCells[col, row])) // check list if cell is not yet an element 
                                {
                                    openCells.Add(board.allCells[col, row]);
                                    opennessScore++;
                                }

                                if (board.allCells[col, row].pieceID != -1 && board.allCells[col, row].color == Color.white)
                                {
                                    KeyValuePair<CellData, CellData> capturingPiece = new(cell, board.allCells[col, row]);
                                    capturingPieces.Add(capturingPiece);

                                    offenseScore += 1 * GetPieceHeuristic(board.allCells[col, row].pieceType);
                                }

                                
                            }
                        }
                    }
                }
            }
        }

        openCells.Clear();
        openCells = null;
        //Debug.Log(offenseScore);
    }


    private void ScanPlayerMoves()
    {
        List<CellData> agentCells = new();

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
                            if (board.allCells[col, row].pieceID == -1 || board.allCells[col, row].color != Color.white)
                            {
                                KeyValuePair<CellData, CellData> playerMove = new(cell, board.allCells[col, row]);
                                playerMoves.Add(playerMove);

                                if (board.allCells[col, row].pieceID != -1 && board.allCells[col, row].color == Color.black && !agentCells.Contains(board.allCells[col, row]))
                                {
                                    agentCells.Add(board.allCells[col, row]);
                                    defenseScore += 1 * GetPieceHeuristic(board.allCells[col, row].pieceType);
                                }

                                //Debug.Log(col + " " + row);
                            }
                        }
                    }
                }
            }
        }

        agentCells.Clear();
        agentCells = null;
        //Debug.Log(defenseScore);
    }

    private bool isFlagAtRisk()
    {
        int flagRow = 0, flagCol = 0;

        foreach (CellData cell in agentPieces)
        {
            if (cell.pieceType == PieceType.Flag)
            {
                flagRow = cell.row;
                flagCol = cell.column;
            }

        }

        if (flagCol >= 0 && flagCol < 9 && flagRow + 1 >= 0 && flagRow + 1 < 8)
        {
            if (board.allCells[flagCol, flagRow + 1].pieceID != -1 && board.allCells[flagCol, flagRow + 1].color == Color.white)
            {
                return true;
            }
        }

        if (flagCol >= 0 && flagCol < 9 && flagRow - 1 >= 0 && flagRow - 1 < 8)
        {
            if (board.allCells[flagCol, flagRow - 1].pieceID != -1 && board.allCells[flagCol, flagRow - 1].color == Color.white)
            {
                return true;
            }
        }

        if (flagCol + 1 >= 0 && flagCol + 1 < 9 && flagRow >= 0 && flagRow < 8)
        {
            if (board.allCells[flagCol + 1, flagRow].pieceID != -1 && board.allCells[flagCol + 1, flagRow].color == Color.white)
            {
                return true;
            }
        }

        if (flagCol - 1 >= 0 && flagCol - 1 < 9 && flagRow + 1 >= 0 && flagRow + 1 < 8)
        {
            if (board.allCells[flagCol - 1, flagRow].pieceID != -1 && board.allCells[flagCol - 1, flagRow].color == Color.white)
            {
                return true;
            }
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
