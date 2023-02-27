using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    float currMoveSpeed;
    [SerializeField, Header("Movement")]
    float moveAcceleration;
    [SerializeField]
    float sprintAcceleration;
    [SerializeField]
    float airMovementSpeed;
    float currMaxSpeed;
    [SerializeField]
    float maxMoveSpeed;
    [SerializeField]
    float maxSprintSprint;
    [SerializeField]
    float jumpStrength;
    [SerializeField]
    float jumpingFloatingForce;
    [SerializeField]
    float extraGravity;
    [SerializeField]
    float stickyFootFactor;
    [SerializeField]
    float turnSpeed;
    [SerializeField, Header("Camera")]
    Camera cam;
    [Range(0,1), SerializeField]
    float mouseSensitivity;
    [SerializeField]
    bool isLookAtCamera;

    Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();

        if(cam == null){
            cam = Camera.main;
        }
    }

    public void CameraAndInputControls(){
        CameraMouseMovement();
        InputControls();
        LookAtCamera();
    }

    RaycastHit hit;
    void LookAtCamera(){
        if(isLookAtCamera == true){
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, float.PositiveInfinity);
            TurnTo(hit.point);
        }
    }

    void LookAtTarget(){
        if(isLookAtCamera == false){
            TurnTo(hit.point);
        }
    }

    //for later use for looking at target
    void TurnTo(Vector3 target){
        transform.rotation = Quaternion.LookRotation(target, transform.up);
    }

    void CameraMouseMovement(){
        //limiting y
        float mouseY = cam.transform.eulerAngles.x - (Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        if(mouseY < 270 && mouseY >= 180){
            mouseY = 270;
        }else if(mouseY > 90 && mouseY < 180){
            mouseY = 90;
        }

        //x mouse movement controls y camera rotation //y mouse controls x camera
        cam.transform.eulerAngles = new Vector3(
            mouseY,
            cam.transform.eulerAngles.y,
            cam.transform.eulerAngles.z
        );

        gameObject.transform.eulerAngles = new Vector3(
            gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y + (Input.GetAxisRaw("Mouse X") * mouseSensitivity),
            gameObject.transform.eulerAngles.z
        );
    }

     bool hasJumped;
    void InputControls(){
        CheckGround();

        Vector3 currentVelocity = transform.InverseTransformDirection(movement.rb.velocity);

        //sprint
        if(Input.GetKey(KeyCode.LeftShift) == true){
            currMoveSpeed = sprintAcceleration;
            currMaxSpeed = maxSprintSprint;
        }else if(Input.GetKey(KeyCode.LeftShift) == false){
            currMoveSpeed = moveAcceleration;
            currMaxSpeed = maxMoveSpeed;
        }

        //basic movement, if in the air move less
        if(isGrounded == true){
            if(currentVelocity.x > 0 && Input.GetAxisRaw("Horizontal") < 0){
                //if movement is the going left way but you're still going right, turn faster
                movement.XMovement(Input.GetAxisRaw("Horizontal"), currMoveSpeed * turnSpeed);
            }else if(currentVelocity.x < 0 && Input.GetAxisRaw("Horizontal") > 0){
                //if movement is the going right way but you're still going left, turn faster
                movement.XMovement(Input.GetAxisRaw("Horizontal"), currMoveSpeed * turnSpeed);
            }else{
                movement.XMovement(Input.GetAxisRaw("Horizontal"), currMoveSpeed);
            }
        }else if(isGrounded == false){
            movement.XMovement(Input.GetAxisRaw("Horizontal"), airMovementSpeed);
        }

        if(isGrounded == true){
            if(currentVelocity.z > 0 && Input.GetAxisRaw("Vertical") < 0){
                //if movement is the going bacwards but you're still going forwards
                movement.ZMovement(Input.GetAxisRaw("Vertical"), currMoveSpeed * turnSpeed);
            }else if(currentVelocity.z < 0 && Input.GetAxisRaw("Vertical") > 0){
                //if movement is the going forward but you're still going backwards
                movement.ZMovement(Input.GetAxisRaw("Vertical"), currMoveSpeed * turnSpeed);
            }else{
                movement.ZMovement(Input.GetAxisRaw("Vertical"), currMoveSpeed);
            }
            movement.ZMovement(Input.GetAxisRaw("Vertical"), currMoveSpeed);
        }else if(isGrounded == false){
            movement.ZMovement(Input.GetAxisRaw("Vertical"), airMovementSpeed);
        }

        //make it less slippery
        if(Input.GetAxisRaw("Horizontal") == 0){

            currentVelocity = new Vector3(  currentVelocity.x - (currentVelocity.x * stickyFootFactor * Time.deltaTime),
                                            currentVelocity.y,
                                            currentVelocity.z
                                         );

            movement.rb.velocity = transform.TransformDirection(currentVelocity);
            
        }

        if(Input.GetAxisRaw("Vertical") == 0){

            currentVelocity = new Vector3(  currentVelocity.x,
                                            currentVelocity.y,
                                            currentVelocity.z - (currentVelocity.z * stickyFootFactor * Time.deltaTime)
                                         );

            movement.rb.velocity = transform.TransformDirection(currentVelocity);
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
            movement.Down(extraGravity);
        }

        //Limit Velocity, magnitude is sqrt(x*x+y*y+z*z)
        if( isGrounded == true && Mathf.Sqrt(movement.rb.velocity.x * movement.rb.velocity.x + movement.rb.velocity.z * movement.rb.velocity.z) > currMaxSpeed){
            movement.rb.velocity = new Vector3( movement.rb.velocity.normalized.x * currMaxSpeed, 
                                                movement.rb.velocity.y,
                                                movement.rb.velocity.normalized.z * currMaxSpeed );
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
