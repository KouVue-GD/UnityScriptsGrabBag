using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter(Collision coll) {
        if(coll.transform.GetComponent<Health>() != null){
            coll.transform.GetComponent<Health>().Damage(damage);
        }
    }
}
