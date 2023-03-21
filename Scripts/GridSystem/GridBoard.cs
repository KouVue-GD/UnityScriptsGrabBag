using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class GridCell{
    public Vector2 position;
    public bool isOccupied;
    public GameObject occupant;
    public bool isMoving;
    public GridCell(int x, int y){
        position = new Vector2(x, y);
        isOccupied = false;
        occupant = null;
        isMoving = false;
    }
}

public class GridBoard : MonoBehaviour
{

    [SerializeField] Vector2 size;

    #region Intializing grid
        void Awake(){
            grid = new List<List<GridCell>>();

            // Loop through each row
            for (int x = 0; x < size.x; x++)
            {
                // Create a new list to hold the cells in this row
                List<GridCell> rowCells = new List<GridCell>();

                // Loop through each column in this row
                for (int y = 0; y < size.y; y++)
                {
                    // Create a new cell and add it to the row
                    GridCell cell = new GridCell(x, y);
                    rowCells.Add(cell);
                }

                // Add the row to the grid
                grid.Add(rowCells);
            }
        }
    #endregion

    #region Basic Grid Functions

        #region Get Grid information
            public Vector2 GetSize(){
                return size;
            }

            [SerializeField] List<List<GridCell>> grid;
            public List<List<GridCell>> GetGrid(){
                return grid;
            }
        #endregion

        #region Clear Cells
            public void ClearCell(GridCell cell){
                cell.isOccupied = false;
                cell.occupant = null;
            }

            public void ClearCellList(List<GridCell> cells){
                foreach (var item in cells)
                {
                    ClearCell(item);
                }
            }

        #endregion

        #region Get Cells

            public GridCell GetCell(int x, int y){
                if(!(x >= 0 && x < size.x && y >= 0 && y < size.y)) print("Tried getting x: " + x + ", y: " + y);
                return grid[x][y];
            }

            public GridCell GetCell(Vector2 passedV2){
                if(!(passedV2.x >= 0 && passedV2.x < size.x && passedV2.y >= 0 && passedV2.y < size.y)) print("Tried getting x: " + passedV2.x + ", y: " + passedV2.y);
                return grid[(int)(passedV2.x)][(int)(passedV2.y)];
            }

            public List<GridCell> GetCellsInColumn(GridCell cell){
                List<GridCell> cells = new List<GridCell>();

                for (int i = 0; i < size.y; i++)
                {
                    cells.Add(grid[(int)(cell.position.x)][i]);
                }

                return cells;
            }

            public List<GridCell> GetCellsInColumn(int x){
                List<GridCell> cells = new List<GridCell>();

                for (int i = 0; i < size.y; i++)
                {
                    cells.Add(grid[x][i]);
                }

                return cells;
            }

            public List<GridCell> GetCellsInRow(GridCell cell){
                List<GridCell> cells = new List<GridCell>();

                for (int i = 0; i < size.y; i++)
                {
                    cells.Add(grid[(i)][(int)(cell.position.y)]);
                }

                return cells;
            }

            public List<GridCell> GetCellsInRow(int y){
                List<GridCell> cells = new List<GridCell>();

                for (int i = 0; i < size.x; i++)
                {
                    cells.Add(grid[i][y]);
                }

                return cells;
            }

            public List<GridCell> GetEmptyCells(){
                List<GridCell> emptyCells = new List<GridCell>();

                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        if(grid[x][y].isOccupied == false){
                            emptyCells.Add(grid[x][y]);
                        }
                    }
                }

                return emptyCells;
            }
        #endregion

        #region Get Adjacent Cells
            public List<GridCell> GetAdjacentCells(GridCell cell){
                List<GridCell> adjacentCells = new List<GridCell>();

                //horizontal
                if(cell.position.x + 1 <= size.x)
                    adjacentCells.Add(grid[(int)(cell.position.x + 1)][(int)(cell.position.y)]);
                if(cell.position.x - 1 >= size.x)
                    adjacentCells.Add(grid[(int)(cell.position.x - 1)][(int)(cell.position.y)]);

                //vertical
                if(cell.position.y + 1 <= size.y)
                    adjacentCells.Add(grid[(int)(cell.position.x)][(int)(cell.position.y + 1)]);
                if(cell.position.y - 1 >= size.y)
                    adjacentCells.Add(grid[(int)(cell.position.x)][(int)(cell.position.y - 1)]);

                // //diagonal upper
                // adjacentCells.Add(grid[(int)(cell.position.x + 1)][(int)(cell.position.y + 1)]);
                // adjacentCells.Add(grid[(int)(cell.position.x - 1)][(int)(cell.position.y + 1)]);
                // //diagonal lower
                // adjacentCells.Add(grid[(int)(cell.position.x + 1)][(int)(cell.position.y - 1)]);
                // adjacentCells.Add(grid[(int)(cell.position.x - 1)][(int)(cell.position.y - 1)]);

                return adjacentCells;
            }

            public List<GridCell> GetAdjacentHorizontalCells(GridCell cell){
                List<GridCell> adjacentCells = new List<GridCell>();

                //vertical
                if(cell.position.y + 1 <= size.y)
                    adjacentCells.Add(grid[(int)(cell.position.x)][(int)(cell.position.y + 1)]);
                if(cell.position.y - 1 >= size.y)
                    adjacentCells.Add(grid[(int)(cell.position.x)][(int)(cell.position.y - 1)]);

                return adjacentCells;
            }

            public List<GridCell> GetAdjacentVerticalCells(GridCell cell){
                List<GridCell> adjacentCells = new List<GridCell>();

                //horizontal
                if(cell.position.x + 1 <= size.x)
                    adjacentCells.Add(grid[(int)(cell.position.x + 1)][(int)(cell.position.y)]);
                if(cell.position.x - 1 >= size.x)
                    adjacentCells.Add(grid[(int)(cell.position.x - 1)][(int)(cell.position.y)]);

                return adjacentCells;
            }
        #endregion

        #region Cell Mutator
            void SetCell(int x, int y, GridCell cell){
                grid[x][y] = cell;
            }

            public void MoveCell(GridCell cell, Vector2 direction){
                SwapCells(cell, grid[(int)(cell.position.x + direction.x)][(int)(cell.position.y + direction.y)]);
            }

            public void SwapCells(GridCell cell1, GridCell cell2){

                bool tempIsOccupied = cell1.isOccupied;
                cell1.isOccupied = cell2.isOccupied;
                cell2.isOccupied = tempIsOccupied;

                GameObject tempOccupant = cell1.occupant;
                cell1.occupant = cell2.occupant;
                cell2.occupant = tempOccupant;

                // print("Swap cell successful" + cell1.position + " is now " + cell2.position);
            }
        #endregion

    #endregion

    #region Extra functions
    //Meant for specific games

    //move grid cells down kinda like gravity
    public void MoveOccupantsDown(){

        //for every column
        for (int x = 0; x < size.x; x++)
        {
            var cellsInColumn = GetCellsInColumn(x);

            bool hasShifted = true;

            //Keep shifting until all cells in the columns have been lowered to their lowest positions.
            while(hasShifted == true){
                hasShifted = false;
                foreach (var cell in cellsInColumn)
                {
                    //check below the cell //make sure not to check at 0 //make sure there it is not occupied //make sure cell has something in it
                    if( cell.position.y - 1 >= 0 && grid[(int)(cell.position.x)][(int)(cell.position.y-1)].occupant == null && cell.isOccupied != false){
                        SwapCells(cell, grid[(int)(cell.position.x)][(int)(cell.position.y-1)]);
                        hasShifted = true;
                    }
                }
            }
        }
    }
    #endregion
}
