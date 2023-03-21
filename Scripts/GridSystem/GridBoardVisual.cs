using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoardVisual : MonoBehaviour
{
    GridBoard gridBoard;
    Vector2 selection;

    List<GameObject> listOfPrefabs;
    [SerializeField] List<GameObject> cellPrefabs;
    [SerializeField] Vector2 cellPrefabSize;
    Vector2 size;

    List<GridCell> listOfCells; // used in updategrid
    [SerializeField] float moveTimeLength; // used in update grid
    [SerializeField] Vector3 spawnPoint; //used in add more to grid



    #region Intialize Visuals
        // Start is called before the first frame update
        void Start()
        {
            gridBoard = GetComponent<GridBoard>();
            selection = new Vector2(0,0);

            size = gridBoard.GetSize();

            InitializeVisuals();
        }

        void InitializeVisuals(){

            listOfPrefabs = new List<GameObject>();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    GameObject temp = Instantiate(cellPrefabs[Random.Range(0, cellPrefabs.Count)], transform);
                    temp.transform.position += new Vector3(cellPrefabSize.x * x, cellPrefabSize.y * y, 0f);
                    listOfPrefabs.Add(temp);

                    gridBoard.GetCell(x,y).occupant = temp;
                    gridBoard.GetCell(x,y).isOccupied = true;
                }
            }
        }

    #endregion

    #region Main Function

        //this should be used to correct visuals based on gridboard
        public void UpdateGrid(){

            var tempGrid = gridBoard.GetGrid();

            listOfCells = new List<GridCell>();

            //check every cell to see if the gameobject it contains is where it should be.
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if(tempGrid[x][y].occupant != null){
                        if(tempGrid[x][y].occupant.transform.position != GetVector3Position(x, y)){
                            listOfCells.Add(tempGrid[x][y]);
                        }
                    }
                }
            }


            //Moves each cell in list to new position
            foreach (var item in listOfCells)
            {
                Vector3 temp = transform.position + cellPrefabs[0].transform.position + new Vector3(cellPrefabSize.x * item.position.x, cellPrefabSize.y * item.position.y, 0f);
                
                StartCoroutine(MoveObjects( item, temp, moveTimeLength));
            }

            //Make sure next iteration is ready
            listOfCells.Clear();
        }
    #endregion

    #region Helpers

        //get vector3 position based on position on grid board or cell position;
        Vector3 GetVector3Position(float x, float y){
            return transform.position + cellPrefabs[0].transform.position + new Vector3(cellPrefabSize.x * x, cellPrefabSize.y * y, 0f);
        }

        //get vector3 position based on position on grid board or cell position;
        Vector3 GetVector3Position(GridCell cell){
            return transform.position + cellPrefabs[0].transform.position + new Vector3(cellPrefabSize.x * cell.position.x, cellPrefabSize.y * cell.position.y, 0f);
        }

        float SignOfFloat(float pValue){
            if(pValue > 0){
                return 1;
            }

            if(pValue < 0){
                return -1;
            }

            return 0;
        }

    #endregion

    #region Mutator Visuals

        //use this only for showing visuals
        public void AddMoreToGrid(){
            var emptyCells = gridBoard.GetEmptyCells();

            foreach (var item in emptyCells)
            {
                item.occupant = Instantiate(cellPrefabs[Random.Range(0, cellPrefabs.Count)], transform);
                item.occupant.transform.position += spawnPoint;
                item.isOccupied = true;

                //TODO: add animation here
                //filler
            }
        }

        public void DestroyCellInGrid(GridCell cell){

            //TODO: add animation here
            //filler
            

            Destroy(cell.occupant);
            gridBoard.ClearCell(cell);
        }
    #endregion

    #region Highlight Cells

        public GameObject highlight;
        public GameObject selectionCursor;

        //highlights selected block that Selection Cursor is over
        public void HighlightCell(Vector2 passedPos){
            // var adjacentCells = gridBoard.GetAdjacentCells(gridBoard.GetCell((int)(selection.x), (int)(selection.y)));
            GridCell cellToHighlight = gridBoard.GetCell((int)(passedPos.x), (int)(passedPos.y));
            Vector3 tempPos = GetVector3Position(cellToHighlight);
            highlight.transform.position = new Vector3(tempPos.x, tempPos.y, highlight.transform.position.z);

            // return cellToHighlight;
        }

        public void HighlightCellHide(){
            highlight.transform.position = new Vector3(-15, 0, highlight.transform.position.z);
        }

    #endregion

    #region Cursor selection
        //updates the position of the cursor
        public void ShowSelectionCursor(){
            GameObject cell = gridBoard.GetCell((int)(selection.x), (int)(selection.y)).occupant;
            if(cell != null) selectionCursor.transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y, selectionCursor.transform.position.z);
            else print(cell);
        }

        public void SelectionMove(float x, float y){
            selection += new Vector2(SignOfFloat(x), SignOfFloat(y));

            if(selection.x >= size.x) selection.x = size.x - 1;
            if(selection.y >= size.y) selection.y = size.y - 1;

            if(selection.x < 0) selection.x = 0;
            if(selection.y < 0) selection.y = 0;
        }

        public Vector2 GetSelection(){
            return selection;
        }
    #endregion


    IEnumerator MoveObjects(GridCell cell, Vector3 targetPositions, float duration)
    {
        float elapsedTime = 0f;
        if(cell.isMoving == false){
            cell.isMoving = true;
            
            while (elapsedTime < duration)
            {
                // print("moving");
                float t = elapsedTime / duration;
                Vector3 newPosition;

                if(cell != null){
                    newPosition = Vector3.Lerp(cell.occupant.transform.position, targetPositions, t);
                    cell.occupant.transform.position = newPosition;
                }

                elapsedTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        

        // Ensure that all cell end up in their final positions
        if(elapsedTime >= duration && cell != null){
            cell.occupant.transform.position = targetPositions;
            cell.isMoving = false;
        }

        //TODO: Only add and remove when it's not moving   
    }

}
