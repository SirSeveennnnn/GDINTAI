using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{
    public Image outlineImage;

    public Vector2Int boardPosition = Vector2Int.zero;
    public Board board = null;
    public RectTransform rectTransform = null;

    public BasePiece currentPiece = null;

    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        boardPosition = newBoardPosition;
        board = newBoard;

        rectTransform = GetComponent<RectTransform>();
    }

    public bool RemovePiece(BasePiece challenger)
    {
        if (currentPiece != null)
        {
            if (challenger.pieceType == PieceType.Spy)
            {
                if (currentPiece.pieceType == PieceType.Private)
                {
                    return false;
                }
                else if (currentPiece.pieceType == PieceType.Spy)
                {
                    currentPiece.Kill();
                    return false;
                }

                currentPiece.Kill();
                return true;
            }

            if (currentPiece.pieceType == PieceType.Spy)
            {
                if (challenger.pieceType == PieceType.Private)
                {
                    currentPiece.Kill();
                    return true;
                }
                else if (challenger.pieceType == PieceType.Spy)
                {
                    currentPiece.Kill();
                    return false;
                }

                
                return false;
            }

            if (challenger.rank > currentPiece.rank)
            {
                currentPiece.Kill();
                
                return true;
                
            }

            if (currentPiece.rank > challenger.rank)
            {
              
                return false;
                
            }

            if (challenger.rank == currentPiece.rank)
            {
                currentPiece.Kill();
              
                return false;
                
            }

        }
       
        return true;
        
    }

   
}
