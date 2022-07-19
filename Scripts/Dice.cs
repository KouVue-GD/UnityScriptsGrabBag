using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script for dice, counts pips(dots), rolls for physical randomization
/// </summary>
public class Dice : MonoBehaviour
{
    //value of pips should be put into the list in order
    public List<GameObject> pips;
    int highestPip;
    float highestPipValue;

    Rigidbody rb;
    public float launchForce;
    public float rollForce;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Launch(){
        rb.velocity = Vector3.up * launchForce;
    }
    public void Roll(){
        rb.AddTorque(Random.Range(0, rollForce), Random.Range(0, rollForce), Random.Range(0, rollForce));
    }

    public int CheckPipFacingUp(){
        CheckGround();

        if(isGrounded == true){
            highestPip = 0;
            highestPipValue = pips[0].transform.position.y;
            for (int i = 0; i < pips.Count; i++)
            {
                if(pips[i].transform.position.y > highestPipValue){
                    highestPipValue = pips[i].transform.position.y;
                    highestPip = i;
                }
            }

            return (highestPip + 1);
        }

        return -1;
    }

    bool isGrounded;
    void CheckGround(){
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);

        if(hit.transform != null){
            isGrounded = true;
        }else{
            isGrounded = false;
        }
    }
}
