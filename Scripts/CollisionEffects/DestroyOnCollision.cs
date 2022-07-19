using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public float delay;

    void OnCollisionEnter(Collision coll){
        Destroy(gameObject, delay);
    }
}
