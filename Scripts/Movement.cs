using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
#region Helper functions
    #region 3D
        public Rigidbody rb;
        void Start(){
            if(rb == null){
                rb = GetComponent<Rigidbody>();    
            }

            if(rb2d == null){
                rb2d = GetComponent<Rigidbody2D>();    
            }

            if(rb != null && rb2d != null){
                print("A Movement script has both rigidbody and a rigidbody2D");
            }

            if(rb == null && rb2d == null){
                print("A Movement script has neither a rigidbody or a rigidbody2D");
            }
        }

        //simple movement
        public void Forward(float speed){
            rb.AddForce(transform.forward * speed);
        }

        public void Backward(float speed){
            rb.AddForce(-transform.forward * speed);
        }

        public void Right(float speed){
            rb.AddForce(transform.right * speed);
        }

        public void Left(float speed){
            rb.AddForce(-transform.right * speed);
        }

        public void Up(float speed){
            rb.AddForce(transform.up * speed);
        }

        public void Down(float speed){
            rb.AddForce(-transform.up * speed);
        }


        //direct input
        public void ZMovement(float input, float speed){
            if(rb != null){
                rb.AddForce(transform.forward * input * speed);
            }

            if(rb2d != null){
                rb2d.AddForce(transform.forward * input * speed);
            }
        }

        public void XMovement(float input, float speed){
            if(rb != null){
                rb.AddForce(transform.right * input * speed);
            }

            if(rb2d != null){
                rb2d.AddForce(transform.right * input * speed);
            }
        }

        public void YMovement(float input, float speed){
            if(rb != null){
                rb.AddForce(transform.up * input * speed);
            }
            
            if(rb2d != null){
                rb2d.AddForce(transform.up * input * speed);
            }
        }

        //renamed direct input
        public void Vertical(float input, float speed){
            if(rb != null){
                rb.AddForce(transform.forward * input * speed);
            }

            if(rb2d != null){
                rb2d.AddForce(transform.forward * input * speed);
            }
        }

        public void Horizontal(float input, float speed){
            if(rb != null){
                rb.AddForce(transform.right * input * speed);
            }

            if(rb2d != null){
                rb2d.AddForce(transform.right * input * speed);
            }
        }

        //Jump
        public void Jump(float force){
            rb.velocity = new Vector3(rb.velocity.x, force, rb.velocity.z);
        }

        public void AddJumpForce(float force){
            rb.AddForce(transform.up * force);
        }
    #endregion
    #region 2D
        public Rigidbody2D rb2d;

        public void Left2D(float speed){
            rb2d.AddForce(Vector2.left * speed * Time.deltaTime);
        }

        public void Right2D(float speed){
            rb2d.AddForce(Vector2.right * speed * Time.deltaTime);
        }

        public void Up2D(float speed){
            rb2d.AddForce(Vector2.up * speed * Time.deltaTime);
        }

        public void Down2D(float speed){
            rb2d.AddForce(Vector2.down * speed * Time.deltaTime);
        }

        public void Left2DRelative(float speed){
            rb2d.AddForce(Vector2.left * speed * Time.deltaTime);
        }

        public void Right2DRelative(float speed){
            rb2d.AddForce(Vector2.right * speed * Time.deltaTime);
        }

        public void Up2DRelative(float speed){
            rb2d.AddForce(Vector2.up * speed * Time.deltaTime);
        }

        public void Down2DRelative(float speed){
            rb2d.AddForce(Vector2.down * speed * Time.deltaTime);
        }

        public void Vertical2D (float input, float speed){
            rb2d.AddForce(Vector2.up * input * speed * Time.deltaTime);
        }

        public void Horizontal2D(float input, float speed){
            rb2d.AddForce(Vector2.right * input * speed * Time.deltaTime);
        }

        //TOPDOWN
        /// <summary>
        /// Used for Topdown. Input needs Direction
        /// </summary>
        /// <param name="input"></param>
        /// <param name="speed"></param>
        public void Move2D(Vector2 input, float speed){
            rb2d.AddForce(input * speed);
        }

        public void Move2DVelocity(Vector2 input, float speed){
            rb2d.velocity = (input * speed);
        }
        
        //SIDESCROLLER
        /// <summary>
        /// Used for Sidescrollers. Input needs Direction
        /// </summary>
        /// <param name="input"></param>
        /// <param name="speed"></param>
        public void Move2D(float input, float speed){
            rb2d.AddForce(new Vector2(input * speed, 0));
        }

        public void Jump2D(float force){
            rb2d.velocity = new Vector2(rb2d.velocity.x, force);
        }

        public void Jump2D(float forceX, float forceY){
            rb2d.velocity = new Vector2(forceX, forceY);
        }

        public void AddJumpForce2D(float force){
            rb2d.AddForce(new Vector2(0, force));
        }
    #endregion
#endregion

#region Variables
    [Header("Stats")]
    [SerializeField] float speed;
    [SerializeField] float airSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float jumpForce;

    [SerializeField] float maxSpeed;
    [SerializeField] float maxAirspeed;

    [SerializeField] float characterHeight;
    [SerializeField] float characterWidth;

    [SerializeField, Range(0,1)] float friction;

    [SerializeField] float floatForce;

    [SerializeField] float jumpDelay;

    float jumpTimerDelay;

    [SerializeField] float clampDelay;

    float clampTimerDelay;
    [SerializeField] float jumpingHorizontalMultiplier;

    bool isGrounded = false;
    bool canLedgeGrab = true;
    bool isLedgeGrabbing = false;
    [SerializeField] float ledgeGrabDelay;
    float ledgeGrabTimer;
    Vector3 ledgeHangingPosition;
    [SerializeField] LayerMask ledgeMask;

#endregion
#region Sidescroller

    public void MovementSideScroller(){
        float horizontal = SignOfFloat(Input.GetAxis("Horizontal"));

        //movement
        if(isGrounded == true){
            Horizontal2D(horizontal, speed);
        }else{
            Horizontal2D(horizontal, airSpeed);
        }

        if(isGrounded && SignOfFloat(rb2d.velocity.x) != horizontal){
            Horizontal2D(horizontal, turnSpeed);
        }
        
        //jumping
        if(Input.GetAxis("Jump") > 0 && isGrounded == true){
            if(Input.GetAxis("Horizontal") != 0){
                Jump2D(horizontal * (speed * jumpingHorizontalMultiplier), jumpForce);
            }else{
                Jump2D(jumpForce);
            }

            isGrounded = false;
        }

        if(Input.GetAxis("Jump") > 0 && isGrounded == false && jumpTimerDelay <= jumpDelay){
            Jump2D(jumpForce);
            isGrounded = false;
        }

        //better jump
        if(isGrounded == false && Input.GetAxis("Jump") > 0){
          AddJumpForce2D(floatForce);
        }

        //jump forgiveness 
        if(isGrounded == false && jumpTimerDelay <= jumpDelay){
            jumpTimerDelay += Time.deltaTime;
        }

        if(isGrounded == true){
            jumpTimerDelay = 0;
        }
        //TODO: okay so raycast checks the right spot for grabbing the ledge fix it so it works
        //wall check, can ledge grab from block if one block empty above that block
        
        if(canLedgeGrab == true && Physics2D.Raycast(gameObject.transform.position, gameObject.transform.right, characterWidth, ledgeMask).collider != null){
            
            Debug.DrawLine(gameObject.transform.position + new Vector3(characterWidth, characterHeight, 0), gameObject.transform.position + new Vector3(characterWidth, characterHeight, 0) + -gameObject.transform.up * characterHeight, Color.red);
            print(Physics2D.Raycast(gameObject.transform.position + new Vector3(characterWidth, characterHeight, 0), -gameObject.transform.up, characterHeight, ledgeMask).collider.name);
            //check head level
            if(canLedgeGrab == true && Physics2D.Raycast(gameObject.transform.position + new Vector3(characterWidth, characterHeight, 0), -gameObject.transform.up, characterHeight, ledgeMask).collider == null){
                //wallHold
                isLedgeGrabbing = true;
                ledgeHangingPosition = transform.position;
                canLedgeGrab = false;
            }
        }

        if(isLedgeGrabbing == true){
            gameObject.transform.position = ledgeHangingPosition;
        }

        if(Input.GetAxis("Jump") > 0 && isLedgeGrabbing == true){
            Jump2D(jumpForce);
            //canLedgeGrab = true;
            isLedgeGrabbing = false;
            ledgeGrabTimer = 0f;
        }

        if(Input.GetAxis("Jump") > 0 && isLedgeGrabbing == false && ledgeGrabTimer < ledgeGrabDelay){
            ledgeGrabTimer += Time.deltaTime;
        }

        if(ledgeGrabTimer >= ledgeGrabDelay){
            canLedgeGrab = true;
        }

        //ground check
        if(Physics2D.Raycast(gameObject.transform.position, Vector2.down, characterHeight).collider != null){
            isGrounded = true;
        }else{
            isGrounded = false;
        }

        //friction
        if(isGrounded && Input.GetAxis("Horizontal") == 0){
            rb2d.velocity -= rb2d.velocity * friction;
        }

        //Clamp for max speed
        if(isGrounded){
            if(clampTimerDelay <= clampDelay){
                clampTimerDelay += Time.deltaTime;
            }
        }else{
            clampTimerDelay = 0f;
        }

        if(isGrounded == true && clampTimerDelay > clampDelay){
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
        }

        if(isGrounded != true){
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxAirspeed);
        }
    }
    
#endregion
    #region  TopDown
        public void TopDown(){
            //to move
            Move2D(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), speed);

            //friction
            if(Input.GetAxis("Horizontal") == 0){
                rb2d.velocity = new Vector2(rb2d.velocity.x * (1-friction), rb2d.velocity.y);
            }

            if(Input.GetAxis("Vertical") == 0){
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * (1-friction));
            }

            //constrain to topspeed
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);

        }

    #endregion
#region 3D
    #region  local 3d movement
        public void Local3DMovement(){
            //ground check
            if(Physics.Raycast(gameObject.transform.position, Vector3.down, characterHeight)){
                isGrounded = true;
            }else{
                isGrounded = false;
            }

            //movement
            if(isGrounded  == true){
                Horizontal( Input.GetAxis("Horizontal"), speed);
                Vertical(   Input.GetAxis("Vertical")  , speed);
            }

            if(isGrounded == false){
                Horizontal( Input.GetAxis("Horizontal"), airSpeed);
                Vertical(   Input.GetAxis("Vertical")  , airSpeed);
            }

            //jumping
                //works only when grounded
            if(Input.GetAxis("Jump") > 0 && isGrounded == true){
                
                Jump(jumpForce);
                isGrounded = false;
            }

                //works only when jump is forgiven
            if(Input.GetAxis("Jump") > 0 && isGrounded == false && jumpTimerDelay <= jumpDelay){
                Jump(jumpForce);
                isGrounded = false;
            }

                //floating jump
            if(isGrounded == false && Input.GetAxis("Jump") > 0){
                AddJumpForce(floatForce);
            }

                //jump forgiveness 
            if(isGrounded == false && jumpTimerDelay <= jumpDelay){
                jumpTimerDelay += Time.deltaTime;
            }
            
                //reset  jump
            if(isGrounded == true){
                jumpTimerDelay = 0;
            }

            //friction
            if(Input.GetAxis("Horizontal") == 0){
                rb.velocity = new Vector3(rb.velocity.x * (1-friction), rb.velocity.y, rb.velocity.z);
            }

            if(Input.GetAxis("Vertical") == 0){
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * (1-friction));
            }

            //constrain to topspeed
            if(isGrounded == true){
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }

            if(isGrounded == false){
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
        }
    #endregion

#endregion

    float SignOfFloat(float pValue){
        if(pValue > 0){
            return 1;
        }

        if(pValue < 0){
            return -1;
        }

        return 0;
    }
}