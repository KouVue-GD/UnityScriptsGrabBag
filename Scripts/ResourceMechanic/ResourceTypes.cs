using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable][ExecuteInEditMode]
public class ResourceTypes : MonoBehaviour
{
    [SerializeField] public List<string> listOfTypes;

    static ResourceTypes instance;
    public static ResourceTypes Instance{
        get {
            if (instance == null) {
                instance = FindObjectOfType<ResourceTypes>();
            }
            return instance;
        }
    }

    public void Update(){
        if(Instance != this){
            DestroyImmediate(this);
        }else{
            instance = this;
        }
    }
}
