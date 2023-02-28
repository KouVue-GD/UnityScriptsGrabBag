using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnOnDestroy : MonoBehaviour
{

#region Helper Classes
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

    [Serializable]
    public class Range{
        public Vector3 min;
        public Vector3 max;
    }

#endregion

#region Variables
    public enum SpawnType{
        spawnOnSelf, spawnAtLocation, spawnNearby
    }

    [Header("Spawn choices")]
    [SerializeField] SpawnType spawnType;


    [Header("Options for Spawntype")]
    [SerializeField] Transform spawnLocation;
    [SerializeField] Range nearbySpawnRange;
    

    [Header("Optional")]
    [SerializeField] bool usePrefabOffset;


    [Header("Objects to spawn")]
    public List<ObjectToSpawn> listOfObjectsToSpawn;


    bool isQuitting = false;

#endregion

#region SpawnOnDestory(Main function)
    //This is to prevent left over objects during testing (function is called by unity)
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
                            Transform offset = temp.transform;
                            Transform targetTransform = SpawnHelper(temp);
                            temp.transform.position = targetTransform.position;
                            temp.transform.rotation = targetTransform.rotation;

                            if(usePrefabOffset == true){
                                temp.transform.position = offset.position;
                                temp.transform.rotation = offset.rotation;
                            }
                        }
                    }
                }
            }
        }
    }

    Transform SpawnHelper(GameObject passedObject){
        Transform transformToReturn = passedObject.transform;
        
        if(spawnType == SpawnType.spawnNearby){
            transformToReturn.transform.position = GetRandomNearbyPosition();
            transformToReturn.transform.rotation = gameObject.transform.rotation;
        }
        if(spawnType == SpawnType.spawnAtLocation){
            transformToReturn.transform.position = spawnLocation.transform.position;
            transformToReturn.transform.rotation = spawnLocation.transform.rotation;
        }    
        if(spawnType == SpawnType.spawnOnSelf){
            transformToReturn.transform.position = gameObject.transform.position;
            transformToReturn.transform.rotation = gameObject.transform.rotation;
        }    
        
        return transformToReturn;
    }

    Vector3 GetRandomNearbyPosition(){
        return gameObject.transform.position + new Vector3( UnityEngine.Random.Range(nearbySpawnRange.min.x, nearbySpawnRange.min.x), 
                                                            UnityEngine.Random.Range(nearbySpawnRange.min.y, nearbySpawnRange.min.y), 
                                                            UnityEngine.Random.Range(nearbySpawnRange.min.z, nearbySpawnRange.min.z));
    }
#endregion
}