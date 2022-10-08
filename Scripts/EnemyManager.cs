using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //gets a list of enemies, controls all enemies movement

    public List<GameObject> listOfEnemyPrefabs;
    List<GameObject> listOfBasicEnemies;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        listOfBasicEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        LevelManagement();
        EnemyMovement();
        UpdateEnemyList();
    }

    void UpdateEnemyList(){
        foreach (var item in listOfBasicEnemies)
        {
            if(item == null){
                listOfBasicEnemies.Remove(item);
                break;
            }
        }
    }

    #region Level Management

    int level = 0;
    float levelTimerWait = 0;
    [SerializeField]
    float timeBetweenLevels = 3;
    bool isLevelReady = false;
    bool isLevelOver = true;
    bool isGameStarted = false;

    [SerializeField]
    int enemiesToSpawnPerLevel;
    [SerializeField]
    int increasedEnemiesPerLevel;
    void LevelManagement(){
        if(isLevelReady == true && isLevelOver == true){
            SpawnEnemyWave(enemiesToSpawnPerLevel + (level * increasedEnemiesPerLevel));
            isLevelReady = false;
            isLevelOver = false;
            levelTimerWait = 0f;
            level += 1;
        }

        if(isLevelOver == true){
            levelTimerWait += Time.deltaTime;
        }

        if(levelTimerWait >= timeBetweenLevels){
            isLevelReady = true;
        }

        if(listOfBasicEnemies.Count <= 0f && isGameStarted == true){
            isLevelOver = true;
        }
    }

    public void StartGame(){
        isGameStarted = !isGameStarted; //TODO: put game start into game state
    }

    #endregion

    #region Spawn Enemy and wave management
    public List<Transform> basicEnemySpawnPositions;
    void SpawnEnemy(){
        #region  basic enemies
        if(listOfBasicEnemies != null && basicEnemySpawnPositions != null){
            // this is latest enemy added
            listOfBasicEnemies.Add(GameObject.Instantiate(listOfEnemyPrefabs[0]));
            //get the latest enemies's added positon
            listOfBasicEnemies[listOfBasicEnemies.Count-1].transform.position = basicEnemySpawnPositions[Random.Range(0, basicEnemySpawnPositions.Count)].position;
        }
        #endregion
        //SHOOTER ENEMIES
        //Boss enemies
    }

    void SpawnEnemyWave(int numOfEnemies){
        for (int i = 0; i < numOfEnemies; i++)
        {
            SpawnEnemy();
        }
    }


    #endregion

    void EnemyMovement(){
        #region  basic enemies
        if(listOfBasicEnemies != null || listOfBasicEnemies.Count != 0){
            foreach (var item in listOfBasicEnemies)
            {
                if(item != null){
                    GameObject temp = GetClosestTarget(item);
                    if(temp != null){
                        //get is null or towers or players
                        GameObject target = GetClosestTarget(item);
                        if(target != null){
                            item.transform.LookAt(target.transform);
                        }
                    }
                    //move until item gets too close
                    if(Vector3.Distance(item.transform.position, temp.transform.position)>= 3f){
                        item.GetComponent<Movement>().Forward(item.GetComponent<Enemy>().speed);
                    }
                    //limit speed
                    // if(item.GetComponent<Movement>().rb.velocity.sqrMagnitude > item.GetComponent<Enemy>().maxSpeed){
                    //     item.GetComponent<Movement>().rb.velocity = Vector3.ClampMagnitude(item.GetComponent<Movement>().rb.velocity, item.GetComponent<Enemy>().maxSpeed);
                    // }
                }
            }
        }
        #endregion
    }

    GameObject GetClosestTarget(GameObject currGameObject){
        float closestDistance = float.PositiveInfinity;
        GameObject closestTarget = null;
        
        //check player
        if(player != null){
            if(Vector3.Distance(player.transform.position, currGameObject.transform.position) < closestDistance){
                closestDistance = Vector3.Distance(player.transform.position, currGameObject.transform.position);
                closestTarget = player;
            }
        }
        
        return closestTarget;
    }
}
