using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [SerializeField]
    GridSystem gridSystem;
    [SerializeField]
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGhostBuilding(); //FIXME: only used for testing
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Build(){
        currBuildingToBuild.transform.position = gridSystem.GetCellPos(GetMousePos());
        if(gridSystem.CheckVacancy(currBuildingToBuild) == true){
            gridSystem.AddGameObjectToGrid(GameObject.Instantiate(currBuildingToBuild));
        }
    }

    public void Destroy(){
        if(gridSystem.CheckVacancy(gridSystem.GetCellPos(GetMousePos())) == false){
            GameObject temp = gridSystem.GetGameObjectInGrid(gridSystem.GetCellPos(GetMousePos()));
            gridSystem.RemoveGameObjectsFromGrid(temp);
            GameObject.Destroy(temp);
        }
    }

    public void SetCurrBuildingToBuild(GameObject building){
        DestroyGhostBuilding();
        currBuildingToBuild = building;
        InitializeGhostBuilding();
    }

    GameObject ghostBuilding;
    [SerializeField]
    GameObject currBuildingToBuild;

    [SerializeField]
    Color ghostColor;
    public void SetGhostBuildingToMousePos(){
        ghostBuilding.transform.position = (Vector3)gridSystem.GetCellPos(GetMousePos()) + new Vector3(0,0,-1);
    }

    void InitializeGhostBuilding(){
        ghostBuilding = GameObject.Instantiate(currBuildingToBuild);
        ghostBuilding.GetComponent<SpriteRenderer>().color = ghostColor;
        ghostBuilding.GetComponent<Collider2D>().enabled = false;
    }

    void DestroyGhostBuilding(){
        Destroy(ghostBuilding);
        ghostBuilding = null;
    }
    Vector2 GetMousePos(){
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
