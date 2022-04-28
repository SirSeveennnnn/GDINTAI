using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeadListManager : MonoBehaviour
{

    public TextMeshProUGUI[] deadPiecesText = new TextMeshProUGUI[15];
    private int[] deadPieces = new int[15]
    {
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
    };

    private int enemyDeadCounter = 0;
    public TextMeshProUGUI enemyCounter;

    public void UpdateEnemyDeadList()
    {
        enemyDeadCounter++;
        enemyCounter.text = enemyDeadCounter.ToString();
    }

    public void UpdateDeadList(PieceType type)
    {
        switch (type)
        {
            case PieceType.General5:
                deadPieces[14]++;
                deadPiecesText[14].text = deadPieces[14].ToString();
                break;
            case PieceType.General4:
                deadPieces[13]++;
                deadPiecesText[13].text = deadPieces[13].ToString();
                break;
            case PieceType.General3:
                deadPieces[12]++;
                deadPiecesText[12].text = deadPieces[12].ToString();
                break;
            case PieceType.General2:
                deadPieces[11]++;
                deadPiecesText[11].text = deadPieces[11].ToString();
                break;
            case PieceType.General1:
                deadPieces[10]++;
                deadPiecesText[10].text = deadPieces[10].ToString();
                break;
            case PieceType.Colonel:
                deadPieces[9]++;
                deadPiecesText[9].text = deadPieces[9].ToString();
                break;
            case PieceType.LtColonel:
                deadPieces[8]++;
                deadPiecesText[8].text = deadPieces[8].ToString();
                break;
            case PieceType.Major:
                deadPieces[7]++;
                deadPiecesText[7].text = deadPieces[7].ToString();
                break;
            case PieceType.Captain:
                deadPieces[6]++;
                deadPiecesText[6].text = deadPieces[6].ToString();
                break;
            case PieceType.Lieutentant1:
                deadPieces[5]++;
                deadPiecesText[5].text = deadPieces[5].ToString();
                break;
            case PieceType.Lieutentant2:
                deadPieces[4]++;
                deadPiecesText[4].text = deadPieces[4].ToString();
                break;
            case PieceType.Sergeant:
                deadPieces[3]++;
                deadPiecesText[3].text = deadPieces[3].ToString();
                break;
            case PieceType.Private:
                deadPieces[2]++;
                deadPiecesText[2].text = deadPieces[2].ToString();
                break;
            case PieceType.Spy:
                deadPieces[1]++;
                deadPiecesText[1].text = deadPieces[1].ToString();
                break;
            case PieceType.Flag:
                deadPieces[0]++;
                deadPiecesText[0].text = deadPieces[0].ToString();
                break;
            
        }
    }

    

}
