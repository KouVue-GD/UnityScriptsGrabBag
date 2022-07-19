    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Goes on Player
/// </summary>
public class FPSCamera : MonoBehaviour
{
    public GameObject cam;
    void Start()
    {
        if(cam == null){
            cam = Camera.main.gameObject;
        }
    }

    void Update() {
        CameraMouseMovement();    
    }

    void CameraMouseMovement(){
        float mouseY = cam.transform.eulerAngles.x - Input.GetAxisRaw("Mouse Y");

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
            gameObject.transform.eulerAngles.y + Input.GetAxisRaw("Mouse X"),
            gameObject.transform.eulerAngles.z
        );
    }
}