using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField]
    float damage;

    public List<string> invalidTargetTags;

    void OnCollisionEnter(Collision coll) {
        if(coll.transform.GetComponent<Health>() != null){
            foreach (var item in invalidTargetTags)
            {
                if(coll.transform.CompareTag(item) != true){
                    coll.transform.GetComponent<Health>().Damage(damage);
                }
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
