using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchSpace
{
    BoardState rootState;


    public void Initialize(BoardState givenState)
    {
        rootState = givenState;
    }


    public void GenerateChildBoardStates(BoardState currentBoard, bool isAgent, int currentDepth, int maxDepth, int randAgent, int randPlayer)
    {
        List<int> storedNormalMoveIndices = new();
        List<int> storedCapturingMoveIndices = new();
        List<int> storedSaveFlagMoveIndices = new();

        int listSize = 0;
        int count = 0;


        if (isAgent && currentDepth < maxDepth)
        {
            bool checkNormalMoves = storedNormalMoveIndices.Count < currentBoard.agentMoves.Count;
            bool checkCapturingMoves = storedCapturingMoveIndices.Count < currentBoard.capturingMoves.Count;
            bool checkSaveFlagMoves = storedSaveFlagMoveIndices.Count < currentBoard.agentSaveFlagMoves.Count;

            while (count < randAgent && (checkNormalMoves || checkCapturingMoves || checkSaveFlagMoves))
            {
                // get random index from possible agent moves
                listSize = storedCapturingMoveIndices.Count;
                KeyValuePair<CellData, CellData> move = GetAgentMove(currentBoard, storedNormalMoveIndices, storedCapturingMoveIndices, storedSaveFlagMoveIndices);
                count += 1;
                //Debug.Log("Agent " + count + "  x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.column + " y: " + move.Value.row + "  " + currentBoard.score);


                // initialize new board data for the new board state
                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(currentBoard.board.allCells);


                // simulate possible agent move
                if (listSize == storedCapturingMoveIndices.Count) // if list did not increase, go normal
                    SimulateNormalMove(move, possibleBoard);
                else
                    SimulateCapturingMove(move, possibleBoard, isAgent);


                // initialize new board state
                BoardState newBoardState = CreateNewBoardState(possibleBoard, currentBoard);
                newBoardState.move = move;
                GenerateChildBoardStates(newBoardState, !isAgent, currentDepth + 1, maxDepth, randAgent, randPlayer);


                // reset values for while condition
                checkNormalMoves = storedNormalMoveIndices.Count < currentBoard.agentMoves.Count;
                checkCapturingMoves = storedCapturingMoveIndices.Count < currentBoard.capturingMoves.Count;
                checkSaveFlagMoves = storedSaveFlagMoveIndices.Count < currentBoard.agentSaveFlagMoves.Count;
            }
        }
        else if (!isAgent && currentDepth < maxDepth)
        {
            bool checkNormalMoves = storedNormalMoveIndices.Count < currentBoard.playerMoves.Count;
            bool checkCapturingMoves = storedCapturingMoveIndices.Count < currentBoard.playerCapturingMoves.Count;

            while (count < randPlayer && (checkNormalMoves || checkCapturingMoves))
            {
                // get random index from possible player moves
                listSize = storedNormalMoveIndices.Count;
                KeyValuePair<CellData, CellData> move = GetPlayerMove(currentBoard, storedNormalMoveIndices, storedCapturingMoveIndices);
                count += 1;
                //Debug.Log("Player    " + count + "  x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.row + " y: " + move.Value.column);


                // initialize new board data for the new board state
                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(currentBoard.board.allCells);


                // simulate possible agent move
                if (listSize == storedCapturingMoveIndices.Count)
                    SimulateNormalMove(move, possibleBoard);
                else
                    SimulateCapturingMove(move, possibleBoard, isAgent);


                // initialize new board state
                BoardState newBoardState = CreateNewBoardState(possibleBoard, currentBoard);
                GenerateChildBoardStates(newBoardState, !isAgent, currentDepth + 1, maxDepth, randAgent, randPlayer);


                // reset values for while condition
                checkNormalMoves = storedNormalMoveIndices.Count < currentBoard.playerMoves.Count;
                checkCapturingMoves = storedCapturingMoveIndices.Count < currentBoard.playerCapturingMoves.Count;
            }
        }

        storedNormalMoveIndices = null;
        storedCapturingMoveIndices = null;
        storedSaveFlagMoveIndices = null;
    }


    private KeyValuePair<CellData, CellData> GetAgentMove(BoardState currentBoard, List<int> normalMoveIndices, List<int> capturingMoveIndices, List<int> saveFlagMoveIndices)
    {
        System.Random rnd = new();
        KeyValuePair<CellData, CellData> move = currentBoard.agentMoves[0]; // TEMPORARY
        int randMoveChance = 0;
        int index = -1;


        if (currentBoard.score < -9999 && saveFlagMoveIndices.Count < currentBoard.agentSaveFlagMoves.Count)
            randMoveChance = 100;
        else if (capturingMoveIndices.Count < currentBoard.capturingMoves.Count)
            randMoveChance = rnd.Next(4);


        if (randMoveChance == 100)
        {
            int i = 0;

            foreach (KeyValuePair<CellData, CellData> saveMove in currentBoard.agentSaveFlagMoves)
            {
                if (!saveFlagMoveIndices.Contains(i))
                {
                    move = saveMove;
                }
                else
                {
                    i += 1;
                }
            }
        }
        else if (randMoveChance < 3 && normalMoveIndices.Count < currentBoard.agentMoves.Count)
        {
            do
            {
                index = rnd.Next(currentBoard.agentMoves.Count);
            } while (normalMoveIndices.Contains(index));

            normalMoveIndices.Add(index);
            move = currentBoard.agentMoves[index];
        }
        else if (randMoveChance >= 3 && randMoveChance < 5 && capturingMoveIndices.Count < currentBoard.capturingMoves.Count)
        {
            do
            {
                index = rnd.Next(currentBoard.capturingMoves.Count);
            } while (capturingMoveIndices.Contains(index));

            capturingMoveIndices.Add(index);
            move = currentBoard.capturingMoves[index];
        }

        return move;
    }


    private KeyValuePair<CellData, CellData> GetPlayerMove(BoardState currentBoard, List<int> normalMoveIndices, List<int> capturingMoveIndices)
    {
        System.Random rnd = new();
        KeyValuePair<CellData, CellData> move = currentBoard.playerMoves[0]; // TEMPORARY
        int randMoveChance = 0;
        int index = -1;


        if (capturingMoveIndices.Count < currentBoard.playerCapturingMoves.Count)
            randMoveChance = rnd.Next(10);


        if (randMoveChance >= 3 && capturingMoveIndices.Count < currentBoard.playerCapturingMoves.Count)
        {
            do
            {
                index = rnd.Next(currentBoard.playerMoves.Count);
            } while (capturingMoveIndices.Contains(index));

            capturingMoveIndices.Add(index);
            move = currentBoard.playerMoves[index];
        }
        else if (randMoveChance < 3 && normalMoveIndices.Count < currentBoard.playerMoves.Count)
        {
            do
            {
                index = rnd.Next(currentBoard.playerMoves.Count);
            } while (normalMoveIndices.Contains(index));

            normalMoveIndices.Add(index);
            move = currentBoard.playerMoves[index];
        }


        return move;
    }


    private void SimulateNormalMove(KeyValuePair<CellData, CellData> move, BoardData possibleBoard)
    {
        int row, col, moveRow, moveCol;
        row = move.Key.row;
        col = move.Key.column;
        moveRow = move.Value.row;
        moveCol = move.Value.column;


        possibleBoard.allCells[moveCol, moveRow].pieceID = possibleBoard.allCells[col, row].pieceID;

        if (possibleBoard.allCells[col, row].color == Color.black)
        {
            possibleBoard.allCells[moveCol, moveRow].pieceType = possibleBoard.allCells[col, row].pieceType;
            possibleBoard.allCells[moveCol, moveRow].color = Color.black;
        }
        else if (possibleBoard.allCells[col, row].color == Color.white)
        {
            possibleBoard.allCells[moveCol, moveRow].pieceType = PieceType.Unknown;
            possibleBoard.allCells[moveCol, moveRow].color = Color.white;
        }

        possibleBoard.allCells[col, row].pieceID = -1;
        possibleBoard.allCells[col, row].pieceType = PieceType.Unknown;
        possibleBoard.allCells[col, row].color = Color.clear;
    }


    private void SimulateCapturingMove(KeyValuePair<CellData, CellData> move, BoardData possibleBoard, bool isAgent)
    {
        int row, col, moveRow, moveCol;
        row = move.Key.row;
        col = move.Key.column;
        moveRow = move.Value.row;
        moveCol = move.Value.column;


        int rank;
        if (isAgent)
        {
            rank = (int)possibleBoard.allCells[col, row].pieceType;

            if (rank < 4)
            {
                possibleBoard.allCells[moveCol, moveRow].pieceID = possibleBoard.allCells[col, row].pieceID;
                possibleBoard.allCells[moveCol, moveRow].pieceType = possibleBoard.allCells[col, row].pieceType;
                possibleBoard.allCells[moveCol, moveRow].color = Color.black;
            }
        }
        else
        {
            rank = (int)possibleBoard.allCells[moveCol, moveRow].pieceType;

            if (rank >= 4)
            {
                possibleBoard.allCells[moveCol, moveRow].pieceID = possibleBoard.allCells[col, row].pieceID;
                possibleBoard.allCells[moveCol, moveRow].pieceType = possibleBoard.allCells[col, row].pieceType;
                possibleBoard.allCells[moveCol, moveRow].color = Color.white;
            }
        }


        possibleBoard.allCells[col, row].pieceID = -1;
        possibleBoard.allCells[col, row].pieceType = PieceType.Unknown;
        possibleBoard.allCells[col, row].color = Color.clear;
    }


    private BoardState CreateNewBoardState(BoardData possibleBoard, BoardState currentBoard)
    {
        BoardState newBoardState = new();
        newBoardState.SetUp(possibleBoard);
        newBoardState.parent = currentBoard;
        currentBoard.children.Add(newBoardState);

        return newBoardState;
    }
}


















/*
    public void GenerateChildBoardStatesCapture(BoardState currentBoard, bool isAgent, int currentDepth, int maxDepth, int randAgent, int randPlayer)
    {
        List<int> storedIndices = new();
        System.Random rnd = new System.Random();

        int count = 0;
        int index = -1;

        if (isAgent && currentDepth < maxDepth)
        {
            while (count < randAgent && storedIndices.Count < currentBoard.capturingMoves.Count)
            {
                // get random index from possible agent capturing moves
                do
                {
                    index = rnd.Next(currentBoard.capturingMoves.Count);
                } while (storedIndices.Contains(index));

                count += 1;
                storedIndices.Add(index);
                KeyValuePair<CellData, CellData> move = currentBoard.capturingMoves[index];
                //Debug.Log("Agent capture " + count + "  x: " + move.Key.row + " y: " + move.Key.column+ " To: " + " x: " + move.Value.row + " y: " + move.Value.column);


                // initialize new board data for the new board state
                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(currentBoard.board.allCells);

                // simulate possible agent move
                int row, col, moveRow, moveCol;
                row = move.Key.row;
                col = move.Key.column;
                moveRow = move.Value.row;
                moveCol = move.Value.column;

                int piece = possibleBoard.allCells[col, row].pieceID;
                possibleBoard.allCells[col, row].pieceID = -1;
                possibleBoard.allCells[moveCol, moveRow].pieceID = piece;


                // initialize new board state
                BoardState boardState = new();
                boardState.move = move;
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
                // get random index from possible player moves
                do
                {
                    index = rnd.Next(currentBoard.playerMoves.Count);
                } while (storedIndices.Contains(index));

                count += 1;
                storedIndices.Add(index);
                KeyValuePair<CellData, CellData> move = currentBoard.playerMoves[index];
                //Debug.Log("Player    " + count + "  x: " + move.Key.column + " y: " + move.Key.row + " To: " + " x: " + move.Value.boardPosition.x + " y: " + move.Value.boardPosition.y);


                // initialize new board data for the new board state
                BoardData possibleBoard = new();
                possibleBoard.CopyCellData(currentBoard.board.allCells);

                // simulate possible agent move
                int row, col, moveRow, moveCol;
                row = move.Key.row;
                col = move.Key.column;
                moveRow = move.Value.row;
                moveCol = move.Value.column;

                int piece = possibleBoard.allCells[col, row].pieceID;
                possibleBoard.allCells[col, row].pieceID = -1;
                possibleBoard.allCells[moveCol, moveRow].pieceID = piece;


                // initialize new board state
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
}
*/