using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool is3D = true;
    #region 3D
    public Rigidbody rb;
    void Start(){
        if(rb == null && is3D == true){
            rb = GetComponent<Rigidbody>();    
        }

        if(rb2d == null && is3D == false){
            rb2d = GetComponent<Rigidbody2D>();    
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

        public void MoveLeft2D(float speed){
                rb2d.AddForce(Vector2.left * speed * Time.deltaTime);
            }

            public void MoveRight2D(float speed){
                rb2d.AddForce(Vector2.right * speed * Time.deltaTime);
            }

            public void MoveUp2D(float speed){
                rb2d.AddForce(Vector2.up * speed * Time.deltaTime);
            }

            public void MoveDown2D(float speed){
                rb2d.AddForce(Vector2.down * speed * Time.deltaTime);
            }

            public void MoveRelativeLeft2D(float speed){
                rb2d.AddForce(Vector2.left * speed * Time.deltaTime);
            }

            public void MoveRelativeRight2D(float speed){
                rb2d.AddForce(Vector2.right * speed * Time.deltaTime);
            }

            public void MoveRelativeUp2D(float speed){
                rb2d.AddForce(Vector2.up * speed * Time.deltaTime);
            }

            public void MoveRelativeDown2D(float speed){
                rb2d.AddForce(Vector2.down * speed * Time.deltaTime);
            }

            //TOPDOWN
            /// <summary>
            /// Used for Topdown
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
            /// Used for Sidescrollers
            /// </summary>
            /// <param name="input"></param>
            /// <param name="speed"></param>
            public void Move2D(float input, float speed){
                rb2d.AddForce(new Vector2(input * speed, 0));
            }

            public void Jump2D(float force){
                rb2d.velocity = new Vector2(rb2d.velocity.x, force);
            }

            public void AddJumpForce2D(float force){
                rb2d.AddForce(new Vector2(0, force));
            }


    #endregion
}
