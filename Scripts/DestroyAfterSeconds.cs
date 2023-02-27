using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float secondsBeforeDestruction;
    float timer;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= secondsBeforeDestruction){
            Destroy(gameObject);
        }
    }
}
