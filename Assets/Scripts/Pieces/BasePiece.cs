using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BasePiece : EventTrigger
{
    public Color color = Color.clear;

    private Cell originalCell = null;
    private Cell currentCell = null;

    private RectTransform rectTransform = null;
    private PieceManager pieceManager;

    //Stats
    public int rank;
    public Image rankImage;
    public TextMeshProUGUI pieceText;
    public PieceType pieceType;

    //Movement
    private List<Cell> highlightedCell = new List<Cell>();


    
    public Cell targetCell = null;

    public void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        pieceManager = newPieceManager;
        color = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        rectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        currentCell = newCell;
        originalCell = newCell;
        currentCell.currentPiece = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    private void CreateCellPath()
    {
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        CellState cellState = CellState.None;

        cellState = currentCell.board.ValidateCell(currentX + 1, currentY, this);
        if (cellState == CellState.Free || cellState == CellState.Enemy)
        {
            highlightedCell.Add(currentCell.board.allCells[currentX + 1, currentY]);
        }

        cellState = currentCell.board.ValidateCell(currentX - 1, currentY, this);
        if (cellState == CellState.Free || cellState == CellState.Enemy)
        {
            highlightedCell.Add(currentCell.board.allCells[currentX - 1, currentY]);
        }

        cellState = currentCell.board.ValidateCell(currentX , currentY + 1, this);
        if (cellState == CellState.Free || cellState == CellState.Enemy)
        {
            highlightedCell.Add(currentCell.board.allCells[currentX, currentY + 1]);
        }

        cellState = currentCell.board.ValidateCell(currentX , currentY - 1, this);
        if (cellState == CellState.Free || cellState == CellState.Enemy)
        {
            highlightedCell.Add(currentCell.board.allCells[currentX, currentY - 1]);
        }

    }

    private void PregameCellPath(Color teamColor)
    {
        if (teamColor == Color.white)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    highlightedCell.Add(currentCell.board.allCells[i, j]);
                }
            }
        }

        if (teamColor == Color.black)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 5; j < 8; j++)
                {
                    highlightedCell.Add(currentCell.board.allCells[i, j]);
                }
            }
        }
    }

    //Movements
    private void ShowCells()
    {
        foreach (Cell cell in highlightedCell)
        {
            cell.outlineImage.enabled = true;
        }
    }

    private void ClearCells()
    {
        foreach (Cell cell in highlightedCell)
        {
            cell.outlineImage.enabled = false;
        }
        
        highlightedCell.Clear();
    }

    //Events
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (GameManager.pregame == true)
        {
            PregameCellPath(this.color);
            ShowCells();
        }

        if (GameManager.pregame == false)
        {
            CreateCellPath();
            ShowCells();
        }
        

    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position += (Vector3)eventData.delta;

        foreach (Cell cell in highlightedCell)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.rectTransform, Input.mousePosition))
            {
                targetCell = cell;
                break;
            }

            targetCell = null;
        }
        
        
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ClearCells();

        if (!targetCell)
        {
            transform.position = currentCell.gameObject.transform.position;
            return;
        }

        if (GameManager.pregame == true)
        {
            //Swap
            Swap();

        }
        else if (GameManager.pregame == false)
        {
            Move();
            pieceManager.SwitchSides(color);
            pieceManager.agent.AgentMove();
        }

        
    }

    public void Kill()
    {
        if (this.pieceType == PieceType.Flag)
        {
            pieceManager.gameOver = true;
        }

        if (this.color == Color.white)
        {
            pieceManager.deadManager.UpdateDeadList(this.pieceType);
        }
        else if (this.color == Color.black)
        {
            pieceManager.deadManager.UpdateEnemyDeadList();
        }

        currentCell.currentPiece = null;
        gameObject.SetActive(false);
    }

    public void Swap()
    {
        
        if (targetCell.currentPiece != null)
        {
            BasePiece targetPiece;
            Cell currentTempCell;
            targetPiece = targetCell.currentPiece;

            targetCell.currentPiece = this;
            currentCell.currentPiece = targetPiece;

            transform.position = targetCell.transform.position;
            targetPiece.transform.position = currentCell.transform.position;

            currentTempCell = currentCell;

            currentCell = targetCell;
            targetPiece.currentCell = currentTempCell;

            targetCell = null;
            targetPiece.targetCell = null;
            ////////////////////////////////////////////////////////////////

            return;

        }

        currentCell.currentPiece = null;
        currentCell = targetCell;
        currentCell.currentPiece = this;
        transform.position = currentCell.transform.position;
        targetCell = null;

    }

    public void Move()
    {
        bool win = true;
        win = targetCell.RemovePiece(this);
        if (!win)
        {
            this.Kill();
            return;
        }

        if (this.pieceType == PieceType.Flag && this.color == Color.white && targetCell.boardPosition.y == 8)
        {
            pieceManager.gameOver = true;
        }

        if (this.pieceType == PieceType.Flag && this.color == Color.black && targetCell.boardPosition.y == 0)
        {
            pieceManager.gameOver = true;
        }

        currentCell.currentPiece = null;
        currentCell = targetCell;
        currentCell.currentPiece = this;
        transform.position = currentCell.transform.position;
        targetCell = null;


    }
}
