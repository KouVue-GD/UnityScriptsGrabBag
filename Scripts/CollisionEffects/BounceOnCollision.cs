using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnCollision : MonoBehaviour
{
    Rigidbody rb;
    public float force;
    public List<string> validTags = new List<string>(){"Player"};
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision coll){
        if(rb != null){
            foreach (var item in validTags)
            {
                if(coll.transform.CompareTag(item) == true){
                    rb.AddForce((gameObject.transform.position - coll.contacts[0].point).normalized * force);
                }
            }
        }
    }
}
