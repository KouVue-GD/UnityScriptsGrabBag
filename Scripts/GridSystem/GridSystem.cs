using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Grid System that creates two grids, a vector3 grid and a bool grid. Contains a list of GameObjects in the grid. Contains movement based on transforms.
/// </summary>
public class GridSystem : MonoBehaviour
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

    class Row{
        //public List<Vector3> col;
        public List<GameObject> col;
    }

    class Grid{
        public List<Row> row;
    }

    Grid grid;

    #region Grid Initialization
    Vector3 offset;

    void Start()
    {   
        offset = gameObject.transform.position;
        grid = new Grid();
        grid.row = new List<Row>();
        //float sizeSquared = Mathf.Sqrt(size.x + size.y);


        //FIXME: faulty logic for creating row and cols atm it needs to be the same value to work properly
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
                grid.row[i].col[j].transform.position = transform.position + new Vector3((i * (cellSize.x + padding.x))/2 * defaultCell.transform.localScale.x,
                                                                                        ( j * (cellSize.y + padding.y))/2 * defaultCell.transform.localScale.y,
                                                                                          0);
            }
        }

        gridVacancy = new GridVacancy();
        gridVacancy.rowVacancy = new List<RowVacancy>();

        //height
        for (int i = 0; i <= size.x; i++)
        {
            gridVacancy.rowVacancy.Add(new RowVacancy());
            gridVacancy.rowVacancy[i].colVacancy = new List<bool>();
            //width
            for (int j = 0; j <= size.y; j++)
            {
                gridVacancy.rowVacancy[i].colVacancy.Add(new bool());
                gridVacancy.rowVacancy[i].colVacancy[j] = true;
            }
        }

        objectsInGrid = new List<GameObject>();
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
        return Vector3.Distance(grid.row[0].col[0].transform.position, grid.row[0].col[1].transform.position); //or (cellSize.x + padding.x))/2 * defaultCell.transform.localScale.x
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

    Vector2 tempCellPos;
    public void SnapGameObjectToGrid(GameObject target){
        tempCellPos = GetClosestCellPos(target.transform.position);
        if(tempCellPos.x != -1 && tempCellPos.y != -1){
            target.transform.position = new Vector2(grid.row[(int)tempCellPos.x].col[(int)tempCellPos.y].transform.position.x,
                                                    grid.row[(int)tempCellPos.x].col[(int)tempCellPos.y].transform.position.y);
        }
    }

    public Vector3 SnapVector3toGrid(Vector3 passedPos){
        Vector3 temp = Vector3.zero;
        tempCellPos = GetClosestCellPos(passedPos);
        if(tempCellPos.x != -1 && tempCellPos.y != -1){
            temp = grid.row[(int)tempCellPos.x].col[(int)tempCellPos.y].transform.position;
        }

        return temp;
    }

    float closestDistance;
    Vector2 closestCell;
    float tempDistance;

    /// <summary>
    /// Get the closest valid cell 
    /// </summary>
    /// <param name="passedPos"></param>
    /// <returns></returns>
    public Vector2 GetClosestCellPos(Vector3 passedPos){
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
            return new Vector2(-1, -1);
        }

        return closestCell;
    }

    /// <summary>
    /// Calculate projected cell position 
    /// </summary>
    /// <param name="passedPos"></param>
    /// <returns></returns>
    public Vector2 GetCellPos(Vector2 passedPos){
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


    #endregion

    #region Grid Vacancy
    /// <summary>
    /// true means it is avaiable
    /// false means it isn't valable
    /// </summary>
    class RowVacancy{
        public List<bool> colVacancy;
    }
    class GridVacancy{
        public List<RowVacancy> rowVacancy;
    }

    GridVacancy gridVacancy;

    public bool CheckVacancy(int x, int y){
        print("Grid Vacacy x: " + x + " and y: " + y);
        if(x > size.x || x < 0 || y > size.y || y < 0){
            return false;
        }

        return gridVacancy.rowVacancy[x].colVacancy[y];
    }

    public bool CheckVacancy(Vector2 pos){
        Vector2 temp = GetCellPos(pos);
        return CheckVacancy((int)temp.x, (int)temp.y);
    }

    public bool CheckVacancy(Vector3 pos){
        Vector2 temp = GetCellPos(pos);
        return CheckVacancy((int)temp.x, (int)temp.y);
    }

    public bool CheckVacancy(GameObject passedObj){
        Vector2 temp = GetCellPos(passedObj.transform.position);
        return CheckVacancy((int)temp.x, (int)temp.y);
    }

    public void SetGridVacancy(int x, int y, bool value){
        gridVacancy.rowVacancy[x].colVacancy[y] = value;
    }

    public void SetGridVacancy(Vector3 pos, bool value){
        Vector2 temp = GetCellPos(pos);
        SetGridVacancy((int)temp.x, (int)temp.y, value);
    }

    public void SetGridVacancy(GameObject passedObj, bool value){
        Vector2 temp = GetCellPos(passedObj.transform.position);
        SetGridVacancy((int)temp.x, (int)temp.y, value);
    }


    #endregion

    #region Grid GameObject
    //it is important that this doesn't create a voided GameObject for each grid cell
    //otherwise it needs to mirror or create a secondary grid so it doesn't collect too much garbage

    List<GameObject> objectsInGrid;

    // [SerializeField]
    // float distanceValidForGameObjects;
    // float CheckGameObjectDistanceToNearestCell(GameObject target){
    //     return Vector3.Distance(target.transform.position, GetClosestCellPos(target.transform.position));
    // }

    public GameObject GetGameObjectInGrid(Vector3 passedPos){
        return FindGameObjectAtPos(passedPos);
    }

    public GameObject FindGameObjectAtPos(Vector3 passedPos){
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

    public GameObject FindGameObjectAtCellPos(Vector2 passedPos){
        return FindGameObjectAtPos(grid.row[(int)passedPos.x].col[(int)passedPos.y].transform.position);
    }

    public bool AddGameObjectToGrid(GameObject pObject){
        if(CheckVacancy(pObject) == true){
            // CheckGameObjectDistanceToNearestCell(pObject)
            objectsInGrid.Add(pObject);
            SetGridVacancy(pObject, false);
            SnapGameObjectToGrid(pObject);
        }

        return true;
    }

    public bool AddGameObjectToGrid(List<GameObject> pListObjects){
        bool canAddAllObjects = true;
        foreach (var item in pListObjects)
        {
            if(AddGameObjectToGrid(item) != true){
                canAddAllObjects = false;
            }
        }

        if(canAddAllObjects == false){
            foreach (var item in objectsInGrid)
            {
                foreach (var itemToCheck in pListObjects)
                {
                    if(item == itemToCheck){
                        RemoveGameObjectsFromGrid(itemToCheck);
                    }
                }
            }

            return false;
        }

        return true;
    }

    public void RemoveGameObjectsFromGrid(GameObject pObject){
        SetGridVacancy(pObject, true);
        objectsInGrid.Remove(pObject);
    }

    #endregion

    #region GridMovement
        float SignOfFloat(float pValue){
            if(pValue > 0){
                return 1;
            }

            if(pValue < 0){
                return -1;
            }

            return 0;
        }

        #region GetPosition 
        Vector3 GetPositionVertical(GameObject target, float pValue){
            return target.transform.position + (SignOfFloat(pValue) * new Vector3(0,1,0) * DistanceBetweenCellX());
        }

        Vector3 GetPositionHorizontal(GameObject target, float pValue){
            return target.transform.position + (SignOfFloat(pValue) * new Vector3(1,0,0) * DistanceBetweenCellY());
        }

        /// <summary>
        /// Bottom Left To Upper Right
        /// </summary>
        Vector3 GetPositionDiagonalRight(GameObject target, float pValue){
            return target.transform.position + (SignOfFloat(pValue) * new Vector3(1,1,0) * DistanceBetweenCellXY());
        }

        /// <summary>
        /// Bottom right To Upper left
        /// </summary>
        Vector3 GetPositionDiagonalLeft(GameObject target, float pValue){
            return target.transform.position + (SignOfFloat(pValue) * new Vector3(1,-1,0) * DistanceBetweenCellXY());
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

        #region MoveLerp
        [SerializeField]
        bool useLerpTransform;
        bool isMoving = false;
        Vector3 currentTargetPos;
        Vector3 oldPos;
        float timeToLerp = 1f;
        float timer = 0;
        GameObject targetObject;
        void FixedUpdate(){
            if(useLerpTransform){
                //print(isMoving);
                if(isMoving == true && Vector3.Distance(targetObject.transform.position, currentTargetPos) <= 0.01f){
                    isMoving = false;
                    currentTargetPos = Vector3.zero;
                    timeToLerp = 1;
                    timer = 0;
                    oldPos = Vector3.zero;
                    targetObject = null;
                }

                if(isMoving == true){
                    if(timer <= timeToLerp){
                        timer += Time.deltaTime;
                    }else{
                        timer = timeToLerp;
                    }

                    targetObject.transform.position = oldPos + ((currentTargetPos - oldPos) * (timer/timeToLerp));//Vector3.Lerp(oldPos, currentTargetPos, timer);

                }
            }
        }

        public bool LerpVertical(GameObject target, float pValue, float pTime = 1){
            if(isMoving != true){
                if(CheckVacancy(GetPositionVertical(target, pValue)) == true){
                    isMoving = true;
                    currentTargetPos = GetPositionVertical(target, pValue);
                    timeToLerp = pTime;
                    oldPos = target.transform.position;
                    targetObject = target;
                    return true;
                }
            }

            return false;
        }

        public bool LerpHorizontal(GameObject target, float pValue, float pTime = 1){
            if(isMoving != true){
                if(CheckVacancy(GetPositionHorizontal(target, pValue)) == true){
                    isMoving = true;
                    currentTargetPos = GetPositionHorizontal(target, pValue);
                    timeToLerp = pTime;
                    oldPos = target.transform.position;
                    targetObject = target;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Bottom Left To Upper Right
        /// </summary>
        public bool LerpDiagonalRight(GameObject target, float pValue, float pTime = 1){
            if(isMoving != true){
                if(CheckVacancy(GetPositionDiagonalRight(target, pValue)) == true){
                    isMoving = true;
                    currentTargetPos = GetPositionDiagonalRight(target, pValue);
                    timeToLerp = pTime;
                    oldPos = transform.position;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Bottom right To Upper left
        /// </summary>
        public bool LerpDiagonalLeft(GameObject target, float pValue, float pTime = 1){
            if(isMoving != true){
                if(CheckVacancy(GetPositionDiagonalRight(target, pValue)) == true){
                    isMoving = true;
                    currentTargetPos = GetPositionDiagonalLeft(target, pValue);
                    timeToLerp = pTime;
                    oldPos = transform.position;
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region  MoveToPosition
        // void MoveToGridPosition(GameObject target, Vector3 newPos){
            

        // }

        void TeleportToGridPosition(GameObject target, Vector3 newPos){
            //new pos should be mouse pos

            RemoveGameObjectsFromGrid(target);
            target.transform.position = new Vector2(newPos.x, newPos.y);
            AddGameObjectToGrid(target);
        }

        #endregion
    
    #endregion

}
