using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    private AI_Agent agent;

    // board state main properties
    public BoardData board = new();
    public BoardState parent;
    public List<BoardState> children = new();


    // list of all pieces from the board state
    private List<CellData> simulatedPlayerPieces = new();
    private List<CellData> simulatedAgentPieces = new();


    // list of all potential moves in the board state
    public KeyValuePair<CellData, CellData> move;
    public List<KeyValuePair<CellData, CellData>> playerMoves = new();
    public List<KeyValuePair<CellData, CellData>> playerCapturingMoves = new();
    public List<KeyValuePair<CellData, CellData>> agentMoves = new();
    public List<KeyValuePair<CellData, CellData>> capturingMoves = new();


    // flag related properties
    private CellData agentFlag;
    private List<CellData> agentFlagChallengers = new();
    public List<KeyValuePair<CellData, CellData>> agentSaveFlagMoves = new();


    // score-related properties
    public float offenseScore = 1;
    private float defenseScore = 1;
    private float opennessScore = 0;
    private float overallAgentPieceWeight = 0;
    public float score = 0;


    public void SetUp(BoardData newBoard)
    {
        board.CopyCellData(newBoard.allCells);

        ScanBoard();
        ScanAgentMoves();
        ScanPlayerMoves();

        FindOffensivePieces();
        CalculateHeuristic();
        CheckFlagAtRisk();


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
                    simulatedAgentPieces.Add(board.allCells[i, j]);
                    overallAgentPieceWeight += GetPieceHeuristic(board.allCells[i, j].pieceType);

                    if (board.allCells[i, j].pieceType == PieceType.Flag)
                    {
                        agentFlag = board.allCells[i, j];
                    }
                }
                else if (board.allCells[i, j].pieceID != -1 && board.allCells[i, j].color == Color.white)
                {
                    simulatedPlayerPieces.Add(board.allCells[i, j]);
                }
            }
        }
    }


    private void ScanAgentMoves()
    {
        List<CellData> openCells = new();

        foreach (CellData cell in simulatedAgentPieces)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i + j == -1 || i + j == 1)
                    {
                        int row = cell.row + i;
                        int col = cell.column + j;

                        if (row >= 0 && row < 8 && col >= 0 && col < 9) // check if out of bounds
                        {
                            // check if it's the agent flag
                            if (cell.pieceType == PieceType.Flag)
                            {
                                if (board.allCells[col, row].pieceID == -1)
                                {
                                    KeyValuePair<CellData, CellData> move = new(cell, board.allCells[col, row]);
                                    agentMoves.Add(move);
                                    agentSaveFlagMoves.Add(move);
                                    opennessScore += 2.1f;
                                }
                            }
                            // check if it's a capturing move
                            else if (board.allCells[col, row].color == Color.white)
                            {
                                KeyValuePair<CellData, CellData> capturingMove = new(cell, board.allCells[col, row]);
                                capturingMoves.Add(capturingMove);

                                offenseScore += (GetPieceHeuristic(cell.pieceType) - (overallAgentPieceWeight / simulatedAgentPieces.Count)) * 1.5f;
                            }
                            // check if it's just a normal move
                            else if (board.allCells[col, row].pieceID == -1)
                            {
                                KeyValuePair<CellData, CellData> move = new(cell, board.allCells[col, row]);
                                agentMoves.Add(move);

                                if (!openCells.Contains(board.allCells[col, row])) // check list if cell is not yet an element 
                                {
                                    openCells.Add(board.allCells[col, row]);
                                    opennessScore++;
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

        foreach (CellData cell in simulatedPlayerPieces)
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
                            // if player piece is next to an agent piece
                            if (board.allCells[col, row].color == Color.black)
                            {
                                KeyValuePair<CellData, CellData> playerCapturingMove = new(cell, board.allCells[col, row]);
                                playerCapturingMoves.Add(playerCapturingMove);

                                if (board.allCells[col, row].color == Color.black && !agentCells.Contains(board.allCells[col, row]))
                                {
                                    agentCells.Add(board.allCells[col, row]);
                                    defenseScore += 1.2f * GetPieceHeuristic(PieceType.General5);
                                }

                                if (board.allCells[col, row].color == Color.black && board.allCells[col, row].pieceType == PieceType.Flag)
                                {
                                    agentFlagChallengers.Add(board.allCells[col, row]);
                                }
                            }
                            // if player piece is next to an empty cell
                            else if (board.allCells[col, row].pieceID == -1 || board.allCells[col, row].color != Color.white)
                            {
                                KeyValuePair<CellData, CellData> playerMove = new(cell, board.allCells[col, row]);
                                playerMoves.Add(playerMove);
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


    private void FindOffensivePieces()
    {
        foreach (CellData cell in simulatedAgentPieces)
        {
            if (cell.row < 4)
            {
                offenseScore += 1 * GetPieceHeuristic(cell.pieceType);
            }
        }
    }


    private void CalculateHeuristic()
    {
        float overallOffense = offenseScore * ((float)simulatedAgentPieces.Count / (float)simulatedPlayerPieces.Count);
        float overallDefense = defenseScore * ((float)simulatedPlayerPieces.Count / (float)simulatedAgentPieces.Count);
        float overallOpenness = (opennessScore / (float)simulatedAgentPieces.Count) * 1.3f;

        score = overallOffense - overallDefense + overallOpenness;

        //Debug.Log("Offense: " + offenseScore + "    overall Offense: " + overallOffense);
        //Debug.Log("Defense: " + defenseScore + "    overall Defense: " + overallDefense);
        //Debug.Log("Openness: " + overallOpenness);
        //Debug.Log("final socre: " + score);
    }


    private void CheckFlagAtRisk()
    {
        List<CellData> nearbyAgentPieces = new();
        List<CellData> nearbyPlayerPieces = new();

        if (agentFlagChallengers.Count != 0)
        {
            score = -99999;
            int sumPlayerCol = 0;
            int sumPlayerRow = 0;
            float bestDistance = 999;
            KeyValuePair<CellData, CellData> bestFlagMove = agentMoves[0];

            // search nearby pieces around the flag
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    int checkRow = agentFlag.row + i;
                    int checkCol = agentFlag.column + j;

                    if (checkCol >= 0 && checkCol < 9 && checkRow >= 0 && checkRow < 8)
                    {
                        if (board.allCells[checkCol, checkRow].color == Color.black)
                        {
                            nearbyAgentPieces.Add(board.allCells[checkCol, checkRow]);
                        }
                        else if (board.allCells[checkCol, checkRow].color == Color.white)
                        {
                            nearbyPlayerPieces.Add(board.allCells[checkCol, checkRow]);
                            sumPlayerCol += j;
                            sumPlayerRow += i;
                        }
                    }
                }
            }

            sumPlayerCol += Mathf.FloorToInt(sumPlayerCol / nearbyPlayerPieces.Count); sumPlayerCol *= -1;
            sumPlayerRow += Mathf.FloorToInt(sumPlayerRow / nearbyPlayerPieces.Count); sumPlayerRow *= -1;

            // search potential moves to save the flag
            foreach (KeyValuePair<CellData, CellData> move in agentSaveFlagMoves)
            {
                float temp = Mathf.Sqrt(Mathf.Pow(move.Value.column - sumPlayerCol, 2) + Mathf.Pow(move.Value.row - sumPlayerRow, 2));

                if (temp < bestDistance)
                {
                    bestFlagMove = move;
                }
            }

            agentSaveFlagMoves.Clear();
            agentSaveFlagMoves.Add(bestFlagMove);
        }
    }


    private float GetPieceHeuristic(PieceType type)
    {
        switch (type)
        {
            case PieceType.General5:
                return 9.7f;

            case PieceType.General4:
                return 8.6f;

            case PieceType.General3:
                return 7.5f;

            case PieceType.General2:
                return 6.45f;

            case PieceType.General1:
                return 5.35f;

            case PieceType.Colonel:
                return 4.35f;

            case PieceType.LtColonel:
                return 3.4f;

            case PieceType.Major:
                return 2.75f;

            case PieceType.Captain:
                return 2.2f;

            case PieceType.Lieutentant1:
                return 1.85f;

            case PieceType.Lieutentant2:
                return 1.50f;

            case PieceType.Sergeant:
                return 1.20f;

            case PieceType.Private:
                return 1.27f;

            case PieceType.Spy:
                return 5.1f;

            case PieceType.Flag:
                return 0f;

            case PieceType.Unknown:
                return 0;

        }
        return 0f;
    }

}
