using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropSystem : MonoBehaviour
{
    [SerializeField]Camera cam;

    [SerializeField]float distanceToCheck = 20f;
    GameObject objectToDrag;
    Vector3 offset;

    Vector3 mousePos;

    [SerializeField]bool is3D;
    [SerializeField]bool is2D;

    [SerializeField]List<string> draggableTags;

     void StartDrag() {
        //check if it is using gui or 2d or 3d
        if(is3D == true){
            //shoot a raycast from mouse in the same direction as camera direction
            RaycastHit hit;
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Physics.Raycast(mousePos, cam.transform.forward, out hit, distanceToCheck);
            
            //set dragging object
            if(hit.collider != null){
                if(draggableTags.Count > 0){
                    foreach (var item in draggableTags)
                    {
                        if(hit.collider.transform.CompareTag(item) == true){
                            objectToDrag = hit.collider.gameObject;
                            offset = hit.collider.transform.position - mousePos;
                            break;
                        }
                    }
                }else{
                    objectToDrag = hit.collider.gameObject;
                    offset = hit.collider.transform.position - mousePos;
                }
            }
        }

        //check if it is using gui or 2d or 3d
        if(is2D == true){
            //shoot a raycast from mouse in the same direction as camera direction
            RaycastHit2D hit;
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(mousePos, cam.transform.forward, distanceToCheck);
            
            //set dragging object
            if(hit.collider != null){
                if(draggableTags.Count > 0){
                    foreach (var item in draggableTags)
                    {
                        if(hit.collider.transform.CompareTag(item) == true){
                            objectToDrag = hit.collider.gameObject;
                            offset = hit.collider.transform.position - mousePos;
                            break;
                        }
                    }
                }else{
                    objectToDrag = hit.collider.gameObject;
                    offset = hit.collider.transform.position - mousePos;
                }
            }
        }
    }

    void KeepDrag() {
        if(objectToDrag != null){
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            objectToDrag.transform.position = mousePos + offset;
        }
    }

    void EndDrag(){
        objectToDrag = null;
    }

    void Update(){
        
        if(Input.GetMouseButton(0) == true){
            if(Input.GetMouseButtonDown(0) == true){
                StartDrag();
            }

            KeepDrag();
        }else{
            EndDrag();
        }
    }
}
