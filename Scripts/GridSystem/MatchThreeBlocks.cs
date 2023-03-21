using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoardManager : MonoBehaviour
{
    GridBoard gridBoard;
    GridBoardVisual gridBoardVisual;

    // Start is called before the first frame update
    void Start()
    {
        gridBoard = GetComponent<GridBoard>();
        gridBoardVisual = GetComponent<GridBoardVisual>();
        selectedBlock = gridBoard.GetCell(gridBoardVisual.GetSelection());
        //initialize grid 
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
    }

    bool hasSelected;
    bool hasFired;

    GridCell selectedBlock;

    [SerializeField] FeedCat feedCat;

    [SerializeField] AudioSource moveSound;

    void Turn(){
        //move selection
        InputSelection();

        //move selectionCursor to the selection
        gridBoardVisual.ShowSelectionCursor();

        //select blocks to swap
            if( hasFired == false && hasSelected == false && Input.GetAxis("Fire1") > 0){
                //highlightcells
                gridBoardVisual.HighlightCell(gridBoardVisual.GetSelection());

                //selectsBlock To swap
                selectedBlock = gridBoard.GetCell(gridBoardVisual.GetSelection());
                hasFired = true;
                hasSelected = true;
            }

            if(Input.GetAxis("Fire1") < 1){hasFired = false;}

            //select another block
            if( hasFired == false && hasSelected == true && Input.GetAxis("Fire1") > 0){

                //hide highlight
                gridBoardVisual.HighlightCellHide();

                //move block
                gridBoard.SwapCells(selectedBlock, gridBoard.GetCell(gridBoardVisual.GetSelection()));
                moveSound.Play();

                hasFired = true;
                hasSelected = false;
            }


        //Add and remove blocks
            //check blocks
            List<GridCell> blocksToRemove = ComparisonCheck();

            //score? or feed cat
            feedCat.CheckColorValidForFood(blocksToRemove);

            //remove blocks if matched 3
            //clears the visuals and clears the data
            foreach (var item in blocksToRemove)
            {
                gridBoardVisual.DestroyCellInGrid(item);
            }

            //move blocks down
            gridBoard.MoveOccupantsDown();

            //add new blocks
            gridBoardVisual.AddMoreToGrid();

            //move blocks visually
            gridBoardVisual.UpdateGrid();

    }

    List<GridCell> ComparisonCheck(){

        List<GridCell> listOfCellToReturn = new List<GridCell>();

        for (int x = 0; x < gridBoard.GetSize().x; x++)
        {
            for (int y = 0; y < gridBoard.GetSize().y; y++)
            {
                // //Horizontal
                // List<GridCell> cellsInRow = gridBoard.GetCellsInRow(gridBoard.GetCell(x, y));

                // //Vertical
                // List<GridCell> cellsInColumn = gridBoard.GetCellsInColumn(gridBoard.GetCell(x, y));

                int countColor = 0;
                List<GridCell> cellsInRow = new List<GridCell>();

                //horizontal
                for (int i = x; i < gridBoard.GetSize().x; i++)
                {
                    if(gridBoard.GetCell(x, y).occupant.GetComponent<Block>().GetColorType() == gridBoard.GetCell(i, y).occupant.GetComponent<Block>().GetColorType()){
                        countColor++;
                        cellsInRow.Add(gridBoard.GetCell(i, y));
                    }else{
                        break;
                    }
                }

                if(countColor >= 3){
                    if(listOfCellToReturn.Contains(gridBoard.GetCell(x, y)) == false)
                        listOfCellToReturn.Add(gridBoard.GetCell(x, y));
                    foreach (var item in cellsInRow)
                    {
                        if(listOfCellToReturn.Contains(item) == false)
                            listOfCellToReturn.Add(item);
                    }
                }

                //vertical
                countColor = 0;
                List<GridCell> cellsInColumn = new List<GridCell>();

                for (int i = y; i < gridBoard.GetSize().y; i++)
                {
                    if(gridBoard.GetCell(x, y).occupant.GetComponent<Block>().GetColorType() == gridBoard.GetCell(x, i).occupant.GetComponent<Block>().GetColorType()){
                        countColor++;
                        cellsInColumn.Add(gridBoard.GetCell(x, i));
                    }else{
                        break;
                    }
                }

                if(countColor >= 3){
                    if(listOfCellToReturn.Contains(gridBoard.GetCell(x, y)) == false)
                        listOfCellToReturn.Add(gridBoard.GetCell(x, y));
                    foreach (var item in cellsInColumn)
                    {
                        if(listOfCellToReturn.Contains(item) == false)
                            listOfCellToReturn.Add(item);
                    }
                }
            }
        }

        return listOfCellToReturn;

    }


    bool hasMoved;
    //TODO: limit the selection to around highlighted cell if the player has selected cell
    void InputSelection(){
        if((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && hasMoved == false){
            gridBoardVisual.SelectionMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            hasMoved = true;
        }

        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {hasMoved = false;}
    }


}
