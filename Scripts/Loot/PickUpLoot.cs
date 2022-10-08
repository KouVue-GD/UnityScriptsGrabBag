using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLoot : MonoBehaviour
{
    #region Pick ups
    [HideInInspector]
    public float totalValue = 0;
    public AudioSource clip;
    void OnTriggerEnter(Collider other) {
        print(other.tag);
        if(other.CompareTag("PickUps") == true){
            //pick up item
            Loot temp = other.GetComponent<Loot>();
            totalValue += temp.value;

            //play sound
            clip.Play();

            //destroy item
            Destroy(other.gameObject);
        }
    }
    #endregion
}
