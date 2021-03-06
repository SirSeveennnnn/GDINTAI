using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public enum PieceType
{
    General5,
    General4,
    General3,
    General2,
    General1,
    Colonel,
    LtColonel,
    Major,
    Captain,
    Lieutentant1,
    Lieutentant2,
    Sergeant,
    Private,
    Spy,
    Flag,
    Unknown
};

public class PieceManager : MonoBehaviour
{
    public GameObject piecePrefab;
    public Sprite[] pieceSprite = new Sprite[30];

    private List<BasePiece> whitePieces = null;
    private List<BasePiece> blackPieces = null;
    private int randSetUp = -1;
    private int countID = 0;
    public bool gameOver = false;

    public AI_Agent agent;

    private Dictionary<PieceType, int> pieceRanks = new Dictionary<PieceType, int>() {

        {PieceType.General5, 13},
        {PieceType.General4, 12},
        {PieceType.General3, 11},
        {PieceType.General2, 10},
        {PieceType.General1, 9},
        {PieceType.Colonel, 8},
        {PieceType.LtColonel, 7},
        {PieceType.Major, 6},
        {PieceType.Captain, 5},
        {PieceType.Lieutentant1, 4},
        {PieceType.Lieutentant2, 3},
        {PieceType.Sergeant, 3},
        {PieceType.Private, 2},
        {PieceType.Spy, 2},
        {PieceType.Flag, 1},

    };

    private PieceType[] pieceOrder = new PieceType[21]
    {
        PieceType.Flag,
        PieceType.Spy,
        PieceType.Spy,
        PieceType.Private,
        PieceType.Private,
        PieceType.Private,
        PieceType.Private,
        PieceType.Private,
        PieceType.Private,
        PieceType.Sergeant,
        PieceType.Lieutentant2,
        PieceType.Lieutentant1,
        PieceType.Captain,
        PieceType.Major,
        PieceType.LtColonel,
        PieceType.Colonel,
        PieceType.General1,
        PieceType.General2,
        PieceType.General3,
        PieceType.General4,
        PieceType.General5

    };

    private PieceType[] ai_PieceOrder1 = new PieceType[21]
    {
        PieceType.LtColonel,
        PieceType.Private,
        PieceType.General1,
        PieceType.General2,
        PieceType.Spy,
        PieceType.Colonel,
        PieceType.Private,
        PieceType.Major,
        PieceType.General5,
        PieceType.Spy,
        PieceType.Private,
        PieceType.General4,
        PieceType.Private,
        PieceType.General3,
        PieceType.Private,
        PieceType.Lieutentant2,
        PieceType.Flag,
        PieceType.Sergeant,
        PieceType.Lieutentant1,
        PieceType.Captain,
        PieceType.Private,
    };

    private PieceType[] ai_PieceOrder2 = new PieceType[21]
    {
        PieceType.Private,
        PieceType.Spy,
        PieceType.General4,
        PieceType.Private,
        PieceType.General5,
        PieceType.Spy,
        PieceType.Private,
        PieceType.Major,
        PieceType.General2,
        PieceType.LtColonel,
        PieceType.Private,
        PieceType.Captain,
        PieceType.Lieutentant2,
        PieceType.Colonel,
        PieceType.Private,
        PieceType.General3,
        PieceType.Flag,
        PieceType.Sergeant,
        PieceType.Lieutentant1,
        PieceType.Private,
        PieceType.General1
    };

    private PieceType[] ai_PieceOrder3 = new PieceType[21]
    {
       PieceType.Private,
       PieceType.General5,
       PieceType.Spy,
       PieceType.LtColonel,
       PieceType.Lieutentant2,
       PieceType.General4,
       PieceType.Private,
       PieceType.Private,
       PieceType.Lieutentant1,
       PieceType.General2,
       PieceType.Private,
       PieceType.General3,
       PieceType.Spy,
       PieceType.Major,
       PieceType.Captain,
       PieceType.Private,
       PieceType.General1,
       PieceType.Flag,
       PieceType.Colonel,
       PieceType.Private,
       PieceType.Sergeant
    };


    public GameManager gameManager;
    public DeadListManager deadManager;



    public void Setup(Board board)
    {
        System.Random rnd = new();
        randSetUp = rnd.Next(3);

        whitePieces = CreatePieces(Color.white, new Color32(255, 255, 255, 255), board);
        blackPieces = CreatePieces(Color.black, new Color32(255, 255, 255, 255), board);

        PlacePieces(0, 3, whitePieces, board);
        PlacePieces(5, 8, blackPieces, board);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < 21; i++)
        {
            GameObject newPieceObject = Instantiate(piecePrefab);
            newPieceObject.AddComponent<BasePiece>();
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            PieceType key = PieceType.Unknown;

            if (teamColor == Color.white)
            {
                key = pieceOrder[i];
            }
            else if (teamColor == Color.black)
            {
                if (randSetUp == 0)
                    key = ai_PieceOrder1[i];
                else if (randSetUp == 1)
                    key = ai_PieceOrder2[i];
                else if (randSetUp == 2)
                    key = ai_PieceOrder3[i];
            }


            BasePiece newPiece = newPieceObject.GetComponent<BasePiece>();
            Image pieceImg = newPieceObject.GetComponent<Image>();
            newPiece.rank = pieceRanks[key];
            newPiece.pieceType = key;
            AssignSprite(teamColor, newPiece.pieceType, pieceImg);

            newPieces.Add(newPiece);
            newPiece.Setup(teamColor, spriteColor, this);
            newPiece.ID = countID; countID++;

        }

        return newPieces;
    }

    private void AssignSprite(Color teamColor, PieceType type, Image image)
    {
        if (teamColor == Color.white)
        {
            switch (type)
            {
                case PieceType.General5:
                    image.sprite = pieceSprite[14];
                    break;
                case PieceType.General4:
                    image.sprite = pieceSprite[13];
                    break;
                case PieceType.General3:
                    image.sprite = pieceSprite[12];
                    break;
                case PieceType.General2:
                    image.sprite = pieceSprite[11];
                    break;
                case PieceType.General1:
                    image.sprite = pieceSprite[10];
                    break;
                case PieceType.Colonel:
                    image.sprite = pieceSprite[9];
                    break;
                case PieceType.LtColonel:
                    image.sprite = pieceSprite[8];
                    break;
                case PieceType.Major:
                    image.sprite = pieceSprite[7];
                    break;
                case PieceType.Captain:
                    image.sprite = pieceSprite[6];
                    break;
                case PieceType.Lieutentant1:
                    image.sprite = pieceSprite[5];
                    break;
                case PieceType.Lieutentant2:
                    image.sprite = pieceSprite[4];
                    break;
                case PieceType.Sergeant:
                    image.sprite = pieceSprite[3];
                    break;
                case PieceType.Private:
                    image.sprite = pieceSprite[2];
                    break;
                case PieceType.Spy:
                    image.sprite = pieceSprite[1];
                    break;
                case PieceType.Flag:
                    image.sprite = pieceSprite[0];
                    break;
            }
        }

        if (teamColor == Color.black)
        {
            image.sprite = pieceSprite[30];
        }

    }

    private void PlacePieces(int bottomRow, int topRow, List<BasePiece> pieces, Board board)
    {
        int piece = 0;
        for (int i = bottomRow; i < topRow; i++)
        {
            for (int j = 1; j < 8; j++)
            {
                pieces[piece].Place(board.allCells[j, i]);
                piece++;
            }
        }
    }

    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (BasePiece piece in allPieces)
        {
            piece.enabled = value;
        }

    }

    public void DisableBlackPieceInteractive()
    {
        foreach (BasePiece piece in blackPieces)
        {
            piece.enabled = false;
        }
    }

    public void SwitchSides(Color color)
    {

        if (gameOver)
        {
            //GGs screen
            gameManager.EndGame(color);

        }

        bool isBlacksTurn = color == Color.white ? true : false;

        SetInteractive(whitePieces, !isBlacksTurn);
        //SetInteractive(blackPieces, isBlacksTurn); //for multiplayer
    }



}

