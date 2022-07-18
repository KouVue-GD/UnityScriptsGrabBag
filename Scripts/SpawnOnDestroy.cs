using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnOnDestroy : MonoBehaviour
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

    [Tooltip("Spawn using its own transform, overrides other options")]
    public bool spawnUsingDefaultTransform;
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

    bool isQuitting = false;
    //This is to prevent left over objects during testing
    void OnApplicationQuit(){isQuitting = true;}

    void OnDestroy() {
        if (!isQuitting)
        {
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
                                    temp.transform.rotation = gameObject.transform.rotation;
                                //spawn on gameObject
                                }else{
                                    temp.transform.position = gameObject.transform.position;
                                    temp.transform.rotation = gameObject.transform.rotation;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}