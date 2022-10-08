using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouseIsometric : MonoBehaviour
{
    //raycast into the screen
    //call back first touch or until length ends
    //look at the point

    //looks straight if it doesn't connect with any target

    Camera mainCamera;

    void Start(){
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(TargetToLookAt(), transform.up);
    }

    public Vector3 offset;

    Vector3 TargetToLookAt(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast (ray, out hit, float.PositiveInfinity);
        //draw invisible ray cast/vector
        Debug.DrawLine (ray.origin, hit.point);
        if(hit.transform != null){
            if(hit.transform.GetComponent<Health>() != null){
                return hit.transform.position;
            }
        }
        return hit.point;
    }
}
