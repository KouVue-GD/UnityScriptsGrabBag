using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Look at target based on rotateSpeed
/// </summary>
public class LookAtTarget : MonoBehaviour
{
    [System.Serializable]
    enum Dimension{
        two, three
    }

    [SerializeField] Dimension type;
    
    [SerializeField] bool isLookAtMouse;
    [SerializeField] Vector3 offset;

    [SerializeField] GameObject target;
    [SerializeField] float rotateSpeed;
    Camera mainCam;
    void Start(){
        mainCam = Camera.main;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(type == Dimension.three){
            if(isLookAtMouse == false){
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - gameObject.transform.position), rotateSpeed * Time.deltaTime);
            }

            if(isLookAtMouse == true){
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(mainCam.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position), rotateSpeed * Time.deltaTime);
            }
        }
        
        if(type == Dimension.two){
            if(isLookAtMouse == false){
                transform.rotation = Quaternion.Slerp(transform.rotation,  LookAtTarget2D(target), rotateSpeed * Time.deltaTime);
            }
            
            if(isLookAtMouse == true){
                transform.rotation = Quaternion.Slerp(transform.rotation,  LookAtMouse2D(), rotateSpeed * Time.deltaTime);
            }
        }
    }

    Quaternion LookAtTarget2D(GameObject target){
        Vector3 dir = target.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion quaternionToReturn = Quaternion.AngleAxis(angle, Vector3.forward);
        quaternionToReturn.eulerAngles += offset;
        return quaternionToReturn;
    }
    Quaternion LookAtMouse2D(){
        Vector3 dir = Input.mousePosition - mainCam.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion quaternionToReturn = Quaternion.AngleAxis(angle, Vector3.forward);
        quaternionToReturn.eulerAngles += offset;
        return quaternionToReturn;
    }
}
