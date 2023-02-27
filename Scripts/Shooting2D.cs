using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting2D : MonoBehaviour
{
    [SerializeField, Tooltip("Only used if not shooting raycast")]GameObject bullet;
    [SerializeField]Animator fireAnim;
    [SerializeField]List<ParticleSystem> firePS;
    [SerializeField]GameObject firePoint;

    [SerializeField, Header("Checks")]bool isRaycastShoot;
    [SerializeField]bool doesShotApplyForce;
    
    [SerializeField, Header("Values")]float force;
    [SerializeField]float damage;
    float baseDamage;
    [SerializeField]float fireRatePerSecond;
    float baseFireRatePerSecond;
    [SerializeField]float recoilStrength;
    [SerializeField]float recoilStablityRate;
    [SerializeField]float maxRecoil;
    [SerializeField]float range;
    float currRecoil;
    float timer;

    void Start(){
        timer = fireRatePerSecond;
        baseDamage = damage;
        baseFireRatePerSecond = fireRatePerSecond;
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

    public RaycastHit2D Shoot2D(){
        RaycastHit2D hit = new RaycastHit2D();
        if(timer >= 1/fireRatePerSecond){
            //fire shot
            if(isRaycastShoot){
                hit = FireRayCast2D();
            }else{
                hit = FireBullet2D();
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

    public RaycastHit2D CheckFire2D(){
        RaycastHit2D hit;
        hit = Physics2D.Raycast(firePoint.transform.position, firePoint.transform.right + new Vector3(Random.Range(-currRecoil, currRecoil), Random.Range(-currRecoil, currRecoil), 0), range);
        // print(hit.collider);
        Debug.DrawRay(firePoint.transform.position, firePoint.transform.right * range, Color.red, 10f);
        return hit;
    }

    RaycastHit2D FireRayCast2D(){
        RaycastHit2D hit = CheckFire2D();
        //apply force
        if(hit.transform != null && doesShotApplyForce == true){
            if(hit.transform.GetComponent<Rigidbody>() != null){
                hit.transform.GetComponent<Rigidbody>().AddForce((hit.point - new Vector2(hit.transform.position.x, hit.transform.position.y)) * -force);
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
                item.Play();
            }
        }

        return hit;
    }

    RaycastHit2D FireBullet2D(){
        RaycastHit2D hit = CheckFire2D();
        
        //create bullet
        GameObject temp = Instantiate(bullet);
        temp.transform.position = firePoint.transform.position;
        temp.transform.rotation = gameObject.transform.parent.rotation;

        //set damage of bullet
        if(temp.GetComponent<DamageOnCollision2D>() != null){
            temp.GetComponent<DamageOnCollision2D>().SetDamage(damage);
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

    public void SetDamage(float newDamage){
        damage = newDamage;
    }

    public void SetfireRatePerSecond(float newFireRate){
        fireRatePerSecond = newFireRate;
        timer = fireRatePerSecond;
    }

    public float GetBaseFireRate(){
        return baseFireRatePerSecond;
    }

}
