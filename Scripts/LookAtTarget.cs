using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Look at target based on rotateSpeed
/// </summary>
public class LookAtTarget : MonoBehaviour
{
    public GameObject target;

    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        //look at target based on rotation speed
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - gameObject.transform.position), rotateSpeed * Time.deltaTime);
    }
}
