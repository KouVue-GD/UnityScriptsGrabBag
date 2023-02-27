using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Unfinished
/// </summary>
public class GUIGrid : MonoBehaviour
{
    //remember x is y and y is x for col and row
    //each row is y
    //each col is x
    //tldr x and y is swapped

    //but they are placed inside a vector3/vector2
    //which is not swapped
    //it is a wholely different system

    [Serializable]
    public struct Size{
        public int x, y;
    }

    [SerializeField]
    Size size;
    [SerializeField]
    Vector2 cellSize;
    [SerializeField]
    Vector2 padding;
    [SerializeField]
    GameObject defaultCell;

    public struct CellPos{
        public int x, y;

        public CellPos(int x, int y){
            this.x = x;
            this.y = y;
        }
    }
    class Row{
        public List<GameObject> col;
    }

    class Grid{
        public List<Row> row;
    }

    Grid grid;
    RectTransform rect;

    [SerializeField]
    float divideSizeDelta;

    #region Grid Initialization
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = gameObject.transform.position;
        grid = new Grid();
        grid.row = new List<Row>();
        rect = GetComponent<RectTransform>();
        //float sizeSquared = Mathf.Sqrt(size.x + size.y);

        //height
        for (int i = 0; i <= size.x; i++)
        {
            grid.row.Add(new Row());
            grid.row[i].col = new List<GameObject>();
            //width
            for (int j = 0; j <= size.y; j++)
            {
                //so a cell that is a 1x1 for 1m position is ( 0.5, 0.5, 0) and 1x1 for 2m is (1, 1, 0) or the center of the cell
                grid.row[i].col.Add(GameObject.Instantiate(defaultCell, gameObject.transform));
                grid.row[i].col[j].transform.position = transform.position + new Vector3((i * (cellSize.x + padding.x))/2 * (rect.sizeDelta/divideSizeDelta).x,
                                                                                        ( j * (cellSize.y + padding.y))/2 * (rect.sizeDelta/divideSizeDelta).x,
                                                                                          0);
            }
        }

        gridVacancy = new GridVacancy();
        gridVacancy.rowVacancy = new List<RowVacancy>();

        //height
        for (int i = 1; i <= size.x; i++)
        {
            gridVacancy.rowVacancy.Add(new RowVacancy());
            gridVacancy.rowVacancy[i-1].colVacancy = new List<bool>();
            //width
            for (int j = 1; j <= size.y; j++)
            {
                gridVacancy.rowVacancy[i - 1].colVacancy.Add(new bool());
                gridVacancy.rowVacancy[i - 1].colVacancy[j - 1] = false;
            }
        }
    }

    #endregion

    #region Grid Information
    
    /// <summary>
    /// Used to know how far to move to reach the next cell on the x axis
    /// </summary>
    /// <returns></returns>
    public float DistanceBetweenCellX(){
        if(size.x <= 1){
            print("Need to make grid in GridSystem size.x bigger than 1");
        }
        return Vector3.Distance(grid.row[0].col[0].transform.position, grid.row[0].col[1].transform.position);
    }

    /// <summary>
    /// Used to know how far to move to reach the next cell on the y axis
    /// </summary>
    /// <returns></returns>
    public float DistanceBetweenCellY(){
        if(size.y <= 1){
            print("Need to make grid in GridSystem size.y bigger than 1");
        }
        return Vector3.Distance(grid.row[0].col[0].transform.position, grid.row[1].col[0].transform.position);
    }

    /// <summary>
    /// Used to know how far to move to reach the next cell diagonally
    /// </summary>
    /// <returns></returns>
    public float DistanceBetweenCellXY(){
        if(size.x <= 1){
            print("Need to make grid in GridSystem size.x bigger than 1");
        }
        if(size.y <= 1){
            print("Need to make grid in GridSystem size.y bigger than 1");
        }
        return Vector3.Distance(grid.row[0].col[0].transform.position, grid.row[1].col[1].transform.position);
    }

    #endregion

    #region Grid Functions

    CellPos tempCellPos;
    public void SnapGameObjectToGrid(GameObject target){
        tempCellPos = GetClosestCellPos(target);
        if(tempCellPos.x != -1 && tempCellPos.y != -1){
            target.transform.position = grid.row[tempCellPos.x].col[tempCellPos.y].transform.position;
        }
    }

    public Vector3 SnapVector3toGrid(Vector3 passedPos){
        Vector3 temp = Vector3.zero;
        tempCellPos = GetClosestCellPos(passedPos);
        if(tempCellPos.x != -1 && tempCellPos.y != -1){
            temp = grid.row[tempCellPos.x].col[tempCellPos.y].transform.position;
        }

        return temp;
    }

    float closestDistance;
    CellPos closestCell;
    float tempDistance;

    /// <summary>
    /// Get the closest valid cell 
    /// </summary>
    /// <param name="passedPos"></param>
    /// <returns></returns>
    public CellPos GetClosestCellPos(Vector3 passedPos){
        tempDistance = float.PositiveInfinity;
        closestDistance = float.PositiveInfinity;

        // calculate projected cell position

        for (int i = 0; i < grid.row.Count; i++)
        {
            for (int j = 0; j < grid.row.Count; j++)
            {
                tempDistance = Vector3.Distance(passedPos, grid.row[i].col[j].transform.position);
                if(tempDistance < closestDistance){
                    closestCell.x = i;
                    closestCell.y = j;
                    closestDistance = tempDistance;
                }
            }
        }

        if(closestDistance > 5){
            return new CellPos(-1, -1);
        }

        return closestCell;
    }

    /// <summary>
    /// Calculate projected cell position 
    /// </summary>
    /// <param name="passedPos"></param>
    /// <returns></returns>
    public CellPos GetCellPos(Vector3 passedPos){
        closestCell.x = -1;
        closestCell.y = -1;

        float xMultiplyier = (int)((passedPos.x - offset.x)/DistanceBetweenCellX());
        float xRemainder = ((passedPos.x - offset.x)%DistanceBetweenCellX());
        if(xRemainder >= DistanceBetweenCellX()/2f){
            xMultiplyier += 1;
        }

        if(xRemainder < DistanceBetweenCellX()/2f && xRemainder >= 0f){
            xMultiplyier += 0;
        }

        if(xRemainder < 0 && xRemainder < -DistanceBetweenCellX()/2f){
            xMultiplyier -= 1;
        }

        float yMultiplyier = (int)((passedPos.y - offset.y)/DistanceBetweenCellY());
        float yRemainder = ((passedPos.y - offset.y)%DistanceBetweenCellY());
        if(yRemainder >= DistanceBetweenCellY()/2f){
            yMultiplyier += 1;
        }

        if(yRemainder < DistanceBetweenCellY()/2f && yRemainder >= 0f){
            yMultiplyier += 0;
        }

        if(yRemainder < 0 && yRemainder < -DistanceBetweenCellY()/2f){
            yMultiplyier -= 1;
        }

        closestCell.x = (int)xMultiplyier;
        closestCell.y = (int)yMultiplyier;

        return closestCell;
    }

    public CellPos GetClosestCellPos(GameObject target){
        Vector3 temp = target.transform.position;
        return GetClosestCellPos(temp);
    }


    #endregion

    #region Grid Vacancy

    class RowVacancy{
        public List<bool> colVacancy;
    }
    class GridVacancy{
        public List<RowVacancy> rowVacancy;
    }

    GridVacancy gridVacancy;

    public bool GetGridVacancy(int x, int y){
        return gridVacancy.rowVacancy[x].colVacancy[y];
    }

    public bool GetGridVacancy(Vector3 pos){
        CellPos temp = GetClosestCellPos(pos);
        return GetGridVacancy(temp.x, temp.y);
    }

    public bool GetGridVacancy(GameObject passedObj){
        CellPos temp = GetClosestCellPos(passedObj.transform.position);
        return GetGridVacancy(temp.x, temp.y);
    }

    public void SetGridVacancy(int x, int y, bool value){
        gridVacancy.rowVacancy[x].colVacancy[y] = value;
    }

    public void SetGridVacancy(Vector3 pos, bool value){
        CellPos temp = GetClosestCellPos(pos);
        SetGridVacancy(temp.x, temp.y, value);
    }

    public void SetGridVacancy(GameObject passedObj, bool value){
        CellPos temp = GetClosestCellPos(passedObj.transform.position);
        SetGridVacancy(temp.x, temp.y, value);
    }


    #endregion

    #region Grid GameObject
    //it is important that this doesn't create a voided GameObject for each grid cell
    //otherwise it needs to mirror or create a secondary grid so it doesn't collect too much garbage

    List<GameObject> objectsInGrid;

    GameObject FindGameObjectAtPos(Vector3 passedPos){
        GameObject temp = null;
        foreach (var item in objectsInGrid)
        {
            if(item.transform.position == passedPos){
                temp = item;
                break;
            }
        }

        return temp;
    }

    GameObject FindGameObjectAtCellPos(CellPos passedPos){
        return FindGameObjectAtPos(grid.row[passedPos.x].col[passedPos.y].transform.position);
    }

    void AddObjectsToGrid(GameObject pObject){
        objectsInGrid.Add(pObject);
        SetGridVacancy(pObject, true);
    }

    void AddObjectsToGrid(List<GameObject> pListObjects){
        foreach (var item in pListObjects)
        {
            AddObjectsToGrid(item);
        }
    }

    void RemoveObjectsToGrid(GameObject pObject){
        SetGridVacancy(pObject, false);
        objectsInGrid.Remove(pObject);
    }

    #endregion

    #region GridMovement
        #region GetPosition 
        Vector3 GetPositionVertical(GameObject target, float pValue){
            return target.transform.position + (pValue * new Vector3(0,1,0) * DistanceBetweenCellX());
        }

        Vector3 GetPositionHorizontal(GameObject target, float pValue){
            return target.transform.position + (pValue * new Vector3(1,0,0) * DistanceBetweenCellY());
        }

        /// <summary>
        /// Bottom Left To Upper Right
        /// </summary>
        Vector3 GetPositionDiagonalRight(GameObject target, float pValue){
            return target.transform.position + (pValue * new Vector3(1,1,0) * DistanceBetweenCellXY());
        }

        /// <summary>
        /// Bottom right To Upper left
        /// </summary>
        Vector3 GetPositionDiagonalLeft(GameObject target, float pValue){
            return target.transform.position + (pValue * new Vector3(1,-1,0) * DistanceBetweenCellXY());
        }
        #endregion

        #region Move
        public void Vertical(GameObject target, float pValue){
            target.transform.position = GetPositionVertical(target, pValue);
        }

        public void Horizontal(GameObject target, float pValue){
            target.transform.position = GetPositionHorizontal(target, pValue);
        }

        /// <summary>
        /// Bottom Left To Upper Right
        /// </summary>
        void DiagonalRight(GameObject target, float pValue){
            target.transform.position = GetPositionDiagonalRight(target, pValue);
        }

        /// <summary>
        /// Bottom right To Upper left
        /// </summary>
        void DiagonalLeft(GameObject target, float pValue){
            target.transform.position = GetPositionDiagonalLeft(target, pValue);
        }
        #endregion

    #endregion

}
