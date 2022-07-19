using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerMovement : MonoBehaviour
{
    float currMoveSpeed;
    public float movementSpeed;
    public float sprintSpeed;
    public float airMovementSpeed;
    public float jumpStrength;
    public float jumpingFloatingForce;
    Camera mainCam;

    Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        mainCam = Camera.main;
    }

     bool hasJumped;
    public void InputControls(){
        CheckGround();

        //sprint
        if(Input.GetKeyDown(KeyCode.LeftShift) == true){
            currMoveSpeed = sprintSpeed;
        }else if(Input.GetKeyDown(KeyCode.LeftShift) == false){
            currMoveSpeed = movementSpeed;
        }

        //basic movement, if in the air move less
        if(isGrounded == true){
            movement.XMovement(Input.GetAxisRaw("Horizontal"), currMoveSpeed);
        }else if(isGrounded == false){
            movement.XMovement(Input.GetAxisRaw("Horizontal"), airMovementSpeed);
        }

        if(isGrounded == true){
            movement.ZMovement(Input.GetAxisRaw("Vertical"), currMoveSpeed);
        }else if(isGrounded == false){
            movement.ZMovement(Input.GetAxisRaw("Vertical"), airMovementSpeed);
        }

        //make it less slippery
        if(Input.GetAxisRaw("Horizontal") == 0){
            movement.rb.velocity = new Vector3(movement.rb.velocity.x/2,
                                                 movement.rb.velocity.y,
                                                 movement.rb.velocity.z
                                                );
        }

        if(Input.GetAxisRaw("Vertical") == 0){
            movement.rb.velocity = new Vector3(movement.rb.velocity.x,
                                                 movement.rb.velocity.y,
                                                 movement.rb.velocity.z/2
                                                );
        }

        //jump
        if(Input.GetAxisRaw("Jump") == 1 && hasJumped == false && isGrounded == true){
            //if player is on the ground and hasn't jumped and presses jump
            movement.Jump(jumpStrength);
            hasJumped = true;
        }else if(Input.GetAxisRaw("Jump") == 1 && hasJumped == true && isGrounded == false){
            //if player is in the air while pressing jump
            //float
            movement.AddJumpForce(jumpingFloatingForce);
            
        }else if(Input.GetAxisRaw("Jump") != 1 && hasJumped == true && isGrounded == true){
            //if player has landed after jumping and has let go of the jump button reset jump
            hasJumped = false;
        }else{
            movement.Down(50f);
        }
    }

    bool isGrounded;
    void CheckGround(){
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, 1.1f);

        if(hit.transform != null){
            isGrounded = true;
        }else{
            isGrounded = false;
        }
    }
}
