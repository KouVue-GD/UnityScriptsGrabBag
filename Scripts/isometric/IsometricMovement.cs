using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricMovement : MonoBehaviour
{
    Movement movement;
    public float speed;
    public float sprintSpeed;
    float currSpeed;
    bool isSprinting;
    public float friction;


    //TODO: set movement based on camera angle

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        currSpeed = speed;
    }

    public float maxSpeed;
    float currMaxSpeed;
    public void InputMovement(){

        //sprint
        if(Input.GetKey(KeyCode.LeftShift) == true){
            isSprinting = true;
        }else{
            isSprinting = false;
        }

        if(isSprinting == true){
            currSpeed = sprintSpeed;
            currMaxSpeed = sprintSpeed;
        }else if(isSprinting == false){
            currSpeed = speed;
            currMaxSpeed = maxSpeed;
        }

        //slowdown
        if(Input.GetAxis("Horizontal") == 0){
            movement.rb.velocity = new Vector3(movement.rb.velocity.x * friction, movement.rb.velocity.y, movement.rb.velocity.z);
        }

        if(Input.GetAxis("Vertical") == 0){
            movement.rb.velocity = new Vector3(movement.rb.velocity.x, movement.rb.velocity.y, movement.rb.velocity.z * friction);
        }

        //movement
        movement.rb.velocity = new Vector3(Input.GetAxis("Horizontal") * currSpeed, movement.rb.velocity.y, Input.GetAxis("Vertical") * currSpeed);
        
        //limit speed
        if(movement.rb.velocity.sqrMagnitude > currMaxSpeed){
            movement.rb.velocity = Vector3.ClampMagnitude(movement.rb.velocity, currMaxSpeed);
        }
        
    }
}

