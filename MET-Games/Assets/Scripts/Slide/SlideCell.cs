using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CellState { Empty, Red, Green };

public class SlideCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SlideGameplay slideGameplay;
    [SerializeField] private Image cellImage;
    public CellState cellState;
    public bool isEndCell;

    private void OnValidate()
    {
        UpdateCellColour();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (this.cellState != CellState.Empty)
        {
            SetSelected();
        }
        else if (slideGameplay.selectedIcon != null)
        {
            bool canMove = CheckIsMoveValid();
            if (canMove)
            {
                MoveCell(this, slideGameplay.selectedCell);
            }
        }
    }

    public void UpdateCellColour()
    {
        if (cellState == CellState.Empty)
        {
            SetCellEmpty();
        }
        else if (cellState == CellState.Red)
        {
            SetCellRed();
        }
        else if (cellState == CellState.Green)
        {
            SetCellGreen();
        }
    }

    public void SetCellGreen()
    {
        cellImage.color = Color.green;
    }

    public void SetCellRed()
    {
        cellImage.color = Color.red;
    }

    public void SetCellEmpty()
    {
        cellImage.color = Color.white;  
    }

    private void SetSelected()
    {
        slideGameplay.selectedCell = this;
        slideGameplay.selectedIcon.transform.position = this.transform.position;
    }

    private bool CheckIsMoveValid()
    {
        if (cellState != CellState.Empty)
        {
            return false;
        }
        else
        {
            bool valid = CheckIsNeighbour(this, slideGameplay.selectedCell);
            return valid;
        }
    }

    private bool CheckIsNeighbour(SlideCell thisCell, SlideCell secondCell)
    {
        bool valid = false;

        if (this.transform.position.x == secondCell.transform.position.x)
        {
            valid = true;
        }

        if (this.transform.position.y == secondCell.transform.position.y)
        {
            valid = true;
        }

        return valid;
    }

    private void MoveCell(SlideCell thisCell, SlideCell secondCell)
    {
        //SlideCell placeholder = secondCell;
        CellState placeholder = secondCell.cellState;

        secondCell.cellState = thisCell.cellState;
        secondCell.UpdateCellColour();

        //thisCell.cellState = CellState.Green;
        //Debug.Log(placeholder.cellState);
        thisCell.cellState = placeholder;
        thisCell.UpdateCellColour();
    }
}