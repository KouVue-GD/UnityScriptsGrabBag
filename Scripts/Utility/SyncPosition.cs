using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosition : MonoBehaviour
{   
    public Transform target;
    [Tooltip("If checked uses both starting pos and offset")]
    public bool useStartingPosAsOffset;
    public Vector3 offset;

    public bool followX = true;
    public bool followY = true;
    public bool followZ = true;

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){
            Vector3 pos = Vector3.zero;
            if(useStartingPosAsOffset == true){
                if(followX == true){
                    pos.x = target.transform.position.x;
                }

                if(followY == true){
                    pos.y = target.transform.position.y;
                }

                if(followZ == true){
                    pos.z = target.transform.position.z;
                }

                transform.position = pos + startingPos + offset;
            }

            if(useStartingPosAsOffset == false){
                if(followX == true){
                    pos.x = target.transform.position.x;
                }

                if(followY == true){
                    pos.y = target.transform.position.y;
                }

                if(followZ == true){
                    pos.z = target.transform.position.z;
                }

                transform.position = pos + offset;
            }
        }
    }

    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.1f;

    public void Shake()
    {
        InvokeRepeating("StartShake", 0, 0.01f);
        Invoke("StopShake", shakeDuration);
    }

    public void StartShake(){
        if(shakeAmount > 0)
        {
            float shakeX = Random.value * shakeAmount * 2 - shakeAmount;
            float shakeY = Random.value * shakeAmount * 2 - shakeAmount;
            Vector3 shakePos = new Vector3(shakeX, shakeY, 0);
            if(target != null){
                transform.position = shakePos + target.position + startingPos;
            }
        }
    }

    void StopShake(){
        CancelInvoke("StartShake");
        if(target != null){
            transform.position = target.position + startingPos;
        }
    }
}
