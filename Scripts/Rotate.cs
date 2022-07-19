using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    Vector3 newAngle;
    public bool rotateX;
    public bool rotateY;
    void Start(){
        newAngle = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotateX == true){
            newAngle.x += speed * Time.deltaTime;
        }
        
        if(rotateY == true){
            newAngle.y += speed * Time.deltaTime;
        }
        
        gameObject.transform.localEulerAngles = newAngle;
    }
}
