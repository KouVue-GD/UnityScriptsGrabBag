using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingIsometric : MonoBehaviour
{
    public bool isRaycastShoot;
    public bool doesShotApplyForce;
    public float force;
    public float damage;
    float baseDamage;
    [Tooltip("Only used if not shooting raycast")]
    public GameObject bullet;

    public float fireRatePerSecond;
    public float recoilStrength;
    public float recoilStablityRate;
    public float maxRecoil;
    float currRecoil;
    float timer;

    public Animator fireAnim;
    public List<ParticleSystem> firePS;

    public GameObject firePoint;

    void Start(){
        timer = fireRatePerSecond;
        baseDamage = damage;
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
    }

    public RaycastHit Fire(){
        RaycastHit hit = new RaycastHit();
        if(timer >= fireRatePerSecond){
            //fire shot
            if(isRaycastShoot){
                hit = FireRayCast();
            }else{
                hit = FireBullet();
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

    public RaycastHit CheckFire(){
        RaycastHit hit;
        Physics.Raycast(firePoint.transform.position, firePoint.transform.forward + new Vector3(Random.Range(-currRecoil, currRecoil), Random.Range(-currRecoil, currRecoil), 0), out hit, 1000f);
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
        if(firePS.Count > 0){
            foreach (var item in firePS)
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
        if(firePS.Count > 0){
            foreach (var item in firePS)
            {
                item.transform.LookAt(hit.point);
                item.Play();
            }
        }

        return hit;
    }

}
