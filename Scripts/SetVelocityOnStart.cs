using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocityOnStart : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 minVelocity;
    public Vector3 maxVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(Mathf.Random.Range(minVelocity.x, maxVelocity.x),
                                  Mathf.Random.Range(minVelocity.y, maxVelocity.y),
                                  Mathf.Random.Range(minVelocity.z, maxVelocity.z));
    }
}
