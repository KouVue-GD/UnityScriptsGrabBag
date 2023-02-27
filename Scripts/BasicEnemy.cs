using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BasicEnemy : MonoBehaviour
{
    //looks around, if it detects player, shoot player
    //once it detects the player, double the range it can detect the player and chase
    //once it stops detecting player move back to original position and decrease range

    [SerializeField] Shooting2D gun;
    [ReadOnly] bool hasTurnedToPlayer = false;

    [SerializeField] GameObject detection; 
    float detectionRange;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    Rigidbody2D rb2d;

    Vector3 startingPos;
    Quaternion startingRot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        detectionRange = detection.transform.localScale.x;
        rb2d = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        startingRot = transform.rotation;
    }

    [SerializeField] bool canPatrol = false;

    // Update is called once per frame
    void Update()
    {
        if(hasDetectedPlayer == true){
            hasTurnedToPlayer = CheckRotation();

            if(hasTurnedToPlayer == true){
                gun.Shoot();
                // print("Enemy Shoot");
            }else{
                TurnToPlayer();
            }

            detection.transform.localScale = Vector3.one * detectionRange * 2f;
        }else{
            detection.transform.localScale = Vector3.one * detectionRange * 1f;

            if(canPatrol){
                Patrol();
            }else{
                ResetToStartingPos();
            }
        }

        //min distance
        if(canMove == true && hasDetectedPlayer == true && Vector2.Distance(transform.position, player.transform.position) > maxDistance){
            //move towards player
            rb2d.velocity = (player.transform.position - transform.position) * moveSpeed;
        }

        if(canMove == true && hasDetectedPlayer == true &&
        Vector2.Distance(transform.position, player.transform.position) > minDistance && 
        Vector2.Distance(transform.position, player.transform.position) < maxDistance){
            rb2d.velocity = Vector2.zero;
        }

        //max distance
        if(canMove == true && hasDetectedPlayer == true && Vector2.Distance(transform.position, player.transform.position) < minDistance){
            //move away from player
            rb2d.velocity = (transform.position - player.transform.position) * moveSpeed;
        }
    }

    void ResetToStartingPos(){
        //move back to starting pos
        if(Vector3.Distance(transform.position, startingPos) > 0.1f){
            rb2d.velocity = (startingPos - transform.position) * moveSpeed;
        }else{
            rb2d.velocity = Vector2.zero;
        }

        //resetRotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, startingRot, rotateSpeed);
    }

    [SerializeField] float waypointRadius = 1f;
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject[] waypoints;
    int currentWaypoint = 0;

    void Patrol()
    {
        // Get the current waypoint
        Transform waypoint = waypoints[currentWaypoint].transform;

        // Check the distance to the waypoint
        float distance = Vector3.Distance(transform.position, waypoint.position);

        // If the AI is close enough to the waypoint
        if (distance < waypointRadius)
        {
            // Move to the next waypoint
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        // Move towards the current waypoint
        rb2d.velocity = (waypoint.position - transform.position) * moveSpeed;
    }

    GameObject player;
    [SerializeField] float rotateSpeed;
    [SerializeField] float angleOffset;
    [SerializeField] float minAngleToCheck;
    void TurnToPlayer(){
        Vector3 dir = player.transform.position - gameObject.transform.position;
        
        float angle = (Mathf.Atan2(dir.y, dir.x) + angleOffset) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);
    }

    bool CheckRotation(){
        Vector3 dir = player.transform.position - gameObject.transform.position;
        
        float angle = (Mathf.Atan2(dir.y, dir.x) + angleOffset) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));

        return Quaternion.Angle(gameObject.transform.rotation, targetRotation) < minAngleToCheck;
    }

    bool hasDetectedPlayer = false;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") == true){
            hasDetectedPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player") == true){
            hasDetectedPlayer = false;
        }
    }
}
