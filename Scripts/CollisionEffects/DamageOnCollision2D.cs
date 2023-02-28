using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision2D : MonoBehaviour
{
    [SerializeField]
    float damage;

    public List<string> invalidTargetTags;
    bool canDamage = false;
    void OnCollisionEnter2D(Collision2D coll) {
        canDamage = true;
        if(coll.transform.GetComponent<Health>() != null){
            foreach (var item in invalidTargetTags)
            {
                if(coll.transform.CompareTag(item) == true){
                    canDamage = false;
                }
            }

            if(canDamage == true){
                coll.transform.GetComponent<Health>().Damage(damage);
            }  
        }
    }

    public void SetDamage(float pDamage){
        damage = pDamage;
    }

    public float GetDamage(){
        return damage;
    }
}
