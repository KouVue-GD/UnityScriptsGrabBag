using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    [SerializeField]
    List<GameObject> listOfGameObjectsSelection;
    [SerializeField, Tooltip("x = row, y = col")]
    Vector2 sizeOfUIGrid;
    [SerializeField]
    GameObject selection;

    float currentlySelected = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool hasPressed = false;
    [SerializeField]
    float delay = 1;
    float timer = 0;
    // Update is called once per frame
    void Update()
    {   
        if(Input.GetAxis("Horizontal") > 0 && hasPressed == false){
            Right();
            hasPressed = true;
            timer = 0;
        }

        if(Input.GetAxis("Horizontal") < 0 && hasPressed == false){
            Left();
            hasPressed = true;
            timer = 0;
        }

        if(Input.GetAxis("Vertical") > 0 && hasPressed == false){
            Up();
            hasPressed = true;
            timer = 0;
        }

        if(Input.GetAxis("Vertical") < 0 && hasPressed == false){
            Down();
            hasPressed = true;
            timer = 0;
        }

        if(timer >= delay){
            hasPressed = false;
            timer = 0;
        }

        // if(Input.GetAxis("Horizontal") == 0){
        //     hasPressed = false;
        //     timer = 0;
        // }

        // if(Input.GetAxis("Vertical") == 0){
        //     hasPressed = false;
        //     timer = 0;
        // }

        timer += Time.deltaTime;

        selection.transform.position = listOfGameObjectsSelection[(int)currentlySelected].transform.position;
        
    }

    void Right(){
        if(currentlySelected % sizeOfUIGrid.x == sizeOfUIGrid.y - 1){
            //hit the end of how far it can go right
        }
        else{
            currentlySelected += 1;
        }
    }

    void Left(){
        if(currentlySelected % sizeOfUIGrid.x == 0){
            //hit the end of how far it can go left
        }
        else{
            currentlySelected -= 1;
        }
    }

    void Up(){
        if((int)(currentlySelected/sizeOfUIGrid.x) == sizeOfUIGrid.y - 1){
            //hit the end of how far it can go up
        }
        else{
            currentlySelected += sizeOfUIGrid.x;
        }
    }

    void Down(){
        if((int)(currentlySelected/sizeOfUIGrid.x) == 0){
            //hit the end of how far it can go down
        }
        else{
            currentlySelected -= sizeOfUIGrid.x;
        }
    }
}
