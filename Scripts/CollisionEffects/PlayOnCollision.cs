using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnCollision : MonoBehaviour
{
    public AudioSource clip;
    public bool canCollideSameTag = true;
    void OnTriggerEnter(Collision coll) {
        if(canCollideSameTag == false){
            if(gameObject.CompareTag(coll.transform.tag) != true){
                clip.Play();
            }
        }
    }
}
