using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnOnStart : MonoBehaviour
{
    [Serializable]
    public class ObjectToSpawn{
        public GameObject objectToSpawn;
        [Tooltip("Minimum amount of possible drop if it drops")]
        public int minAmount;
        [Tooltip("Maximum amount of possible drop if it drops")]
        public int maxAmount;
        [Range(0f, 100f), Tooltip("By Percent ie 50 = 50% chance to drop")]
        public float dropRate;
    }
    float timer = 0;
    bool hasPlayed = false;
    public float delayBySeconds = 0;
    [Tooltip("Spawn using its own transform, overrides other options")]
    public bool spawnUsingDefaultTransform;
    [Tooltip("Spawn using its own rotation")]
    public bool spawnUsingDefaultRotation;
    [Tooltip("Spawn nearby or using this transform values(corpse)")]
    public bool spawnNearby;
    [Tooltip("Min value for Random.Range for spawning nearby")]
    public Vector3 minPositionNearby;
    [Tooltip("Max value for Random.Range for spawning nearby")]
    public Vector3 maxPositionNearby;
    /*
        for min and max pos, if min and max = 0 then it will use 0 for y or in other words it won't have any height
    */

    public List<ObjectToSpawn> listOfObjectsToSpawn;
    void Update() {
        if(hasPlayed == false){
            timer += Time.deltaTime;
        }

        if(timer >= delayBySeconds && hasPlayed == false){
            foreach (var item in listOfObjectsToSpawn)
            {
                if(item.objectToSpawn != null){
                    //roll to see if it drops
                    if(UnityEngine.Random.Range(0f, 100f) <= item.dropRate){
                        //roll to see how many drops
                        int amountToDrop = UnityEngine.Random.Range(item.minAmount, item.maxAmount + 1);
                        //drop the item
                        for (int i = 0; i < amountToDrop; i++)
                        {
                            GameObject temp = GameObject.Instantiate(item.objectToSpawn);
                            //Spawn using its own transform //note its default behaviour
                            if(spawnUsingDefaultTransform == false){
                                //spawn nearby 
                                if(spawnNearby){
                                    temp.transform.position = gameObject.transform.position + new Vector3(  UnityEngine.Random.Range(minPositionNearby.x, maxPositionNearby.x), 
                                                                                                            UnityEngine.Random.Range(minPositionNearby.y, maxPositionNearby.y), 
                                                                                                            UnityEngine.Random.Range(minPositionNearby.z, maxPositionNearby.z));
                                    if(spawnUsingDefaultRotation == false){
                                        temp.transform.rotation = gameObject.transform.rotation;
                                    }
                                //spawn on gameObject
                                }else{
                                    temp.transform.position = gameObject.transform.position;
                                    if(spawnUsingDefaultRotation == false){
                                        temp.transform.rotation = gameObject.transform.rotation;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            hasPlayed = true;
        }
    }
}