using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField, Tooltip("Only used if not shooting raycast")]
    GameObject bullet;
    [SerializeField, Tooltip("Calculations done with camera")]
    Camera cam;
    [SerializeField, Tooltip("Mostly for visuals")]
    GameObject firePoint;

    [SerializeField, Header("Settings")]
    bool uselaserSight;
    [SerializeField]
    LineRenderer laserSight;
    [SerializeField]
    bool isRaycastShoot = true;
    [SerializeField]
    bool doesShotApplyForce;

    [SerializeField, Header("Values")]
    float force;
    [SerializeField]
    float damage;
    float baseDamage;

    [SerializeField]
    float fireRatePerSecond;
    [SerializeField]
    float recoilStrength;
    [SerializeField]
    float recoilStablityRate;
    [SerializeField]
    float maxRecoil;
    float currRecoil;
    float timer;

    [SerializeField, Header("Visuals")]
    Animator fireAnim;
    [SerializeField]
    List<ParticleSystem> fireParticleSystem;
    [SerializeField]
    AudioSource fireSound;

    void Start(){
        timer = fireRatePerSecond;
        baseDamage = damage;

        //use this gameobject if there is no firepoint set
        if(firePoint == null){
            firePoint = gameObject;
        }
    }

    public float GetBaseDamage(){
        return baseDamage;
    }

    void Update(){
        timer += Time.deltaTime;

        //reduce recoil over time
        if(currRecoil >= 0){
            currRecoil -= recoilStablityRate * Time.deltaTime;
            //currRecoil can't go below min
            if(currRecoil < 0){
                currRecoil = 0;
            }
        }

        LaserSightFire();
    }
    public RaycastHit Fire(){
        RaycastHit hit = new RaycastHit();
        if(timer >= fireRatePerSecond/60){
            //fire shot
            if(isRaycastShoot){ //fire raycast if it is a raycast shot
                hit = FireRayCast();
            }else{//fire bullet if it is a bullet shot
                //if bullet prefab doesn't exist then switch to raycast shooting
                if(bullet != null){
                    hit = FireBullet();
                }else{
                    hit = FireRayCast();
                    isRaycastShoot = true;
                }
            }

            if(fireSound != null){
               fireSound.Play(); 
            }
            
            //recoil per shot
            if(currRecoil < maxRecoil){
                currRecoil += recoilStrength;
                //limit recoil;
                if(currRecoil > recoilStrength){
                    currRecoil = maxRecoil;
                }
            }

            timer = 0f;
        }
        return hit;
    }

    //good for telling the player what they're going to hit
    void LaserSightFire(){
        if(laserSight != null){
            if(uselaserSight == true){
                laserSight.enabled = true;
                RaycastHit hit;
                Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(Random.Range(-currRecoil, currRecoil), Random.Range(-currRecoil, currRecoil), 0), out hit, float.PositiveInfinity);
                laserSight.SetPosition(0, firePoint.transform.position);
                laserSight.SetPosition(laserSight.positionCount - 1, hit.point);
            }else{
                laserSight.enabled = false;
            }
        }
        
    }

    public RaycastHit CheckFire(){
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(Random.Range(-currRecoil, currRecoil), Random.Range(-currRecoil, currRecoil), 0), out hit, float.PositiveInfinity);
        return hit;
    }

    RaycastHit FireRayCast(){
        RaycastHit hit = CheckFire();
        //apply force
        if(hit.transform != null && doesShotApplyForce == true){
            if(hit.transform.GetComponent<Rigidbody>() != null){
                hit.transform.GetComponent<Rigidbody>().AddForce((hit.point - hit.transform.position) * -force);
            }
        }

        //apply damage
        if(hit.transform != null){
            if(hit.transform.gameObject.GetComponent<Health>() != null){
                hit.transform.gameObject.GetComponent<Health>().Damage(damage);
            }
        }

        //animation
        if(fireAnim != null){
            fireAnim.Play("FireAnim");
        }

        //muzzleFlash, bullet effects
        if(fireParticleSystem.Count > 0){
            foreach (var item in fireParticleSystem)
            {
                item.transform.LookAt(hit.point);
                item.Play();
            }
        }

        return hit;
    }

    RaycastHit FireBullet(){
        RaycastHit hit = CheckFire();
        
        GameObject temp = Instantiate(bullet);
        temp.transform.position = firePoint.transform.position;
        temp.transform.LookAt(hit.point);
        if(temp.activeSelf == false){
            temp.SetActive(true);
        }
        if(temp.GetComponent<DamageOnCollision>() != null){
            temp.GetComponent<DamageOnCollision>().SetDamage(damage);
        }
        
        //animation
        if(fireAnim != null){
            fireAnim.Play("FireAnim");
        }

        //muzzleFlash, bullet effects
        if(fireParticleSystem.Count > 0){
            foreach (var item in fireParticleSystem)
            {
                item.transform.LookAt(hit.point);
                item.Play();
            }
        }

        return hit;
    }


}
