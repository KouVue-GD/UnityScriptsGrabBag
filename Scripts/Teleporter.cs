using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportLocation;
    public List<GameObject> validObjectsToTeleport;
    bool isTeleporterReady = false;
    [HideInInspector]
    public bool teleportedRecently = false;
    float timer;
    void Update(){
        if(teleportedRecently == true){
            if(timer <= 1f){
                timer += Time.deltaTime;
            }else{
                teleportedRecently = false;
                timer = 0f;
            }
        }
    }

    public bool GetIsTeleporterReady(){
        return isTeleporterReady;
    }

    public void SetIsTeleporterReady(bool value){
        isTeleporterReady = value;
    }

    void OnTriggerEnter(Collider coll) {
        if(validObjectsToTeleport != null){
            foreach (var item in validObjectsToTeleport)
            {
                if(item == coll.gameObject){
                    if(isTeleporterReady == true){
                        coll.transform.position = teleportLocation.transform.position;
                        coll.transform.rotation = teleportLocation.transform.rotation;
                        isTeleporterReady = false;
                        teleportedRecently = true;
                    }
                }
            }
        }else{
            if(isTeleporterReady == true){
                coll.transform.position = teleportLocation.transform.position;
                coll.transform.rotation = teleportLocation.transform.rotation;
                isTeleporterReady = false;
                teleportedRecently = true;
            }
        }
    }
}
