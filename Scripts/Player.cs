using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement3D playerMovement;
    public Shooting gun;
    public Melee gunMelee;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement3D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.CameraAndInputControls();

        if(Input.GetAxis("Fire1") == 1){
            gun.Fire();
        }

        if(Input.GetKeyUp(KeyCode.F)){
            gunMelee.StartMelee();
        }
    }
}
