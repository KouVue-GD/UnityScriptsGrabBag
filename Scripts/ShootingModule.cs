using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingModule : MonoBehaviour
{
    public enum Dimension{
        two, three
    }

    [Header("Only used if not shooting raycast")]
    [SerializeField]GameObject bullet;

    [Header("Required")]
    [SerializeField] Camera cam;
    [SerializeField] Dimension dimension = Dimension.three;

    [Header("Optional")]
    [SerializeField] GameObject firePoint;
    [SerializeField] LineRenderer laserSight;

    [Header("Settings")]
    [SerializeField] bool usingLaserSight;
    [SerializeField] bool isRaycastShoot = true;
    [SerializeField] bool doesShotApplyForce;

    [Header("Values")]
    [SerializeField] float force = 5;
    [SerializeField] float damage = 1;
    float baseDamage;

    [SerializeField] float fireRatePerSecond = 3;
    float baseFireRatePerSecond;

    [Header("Recoil")]
    [SerializeField] float recoilStrength = 0;
    [SerializeField] float recoilStablityRate = 1;
    [SerializeField] float maxRecoil = 0;
    float currRecoil;
    float timer;

    [Header("Effects")]
    [SerializeField] List<ParticleSystem> firePS;
    [SerializeField] Animator fireAnim;
    [SerializeField] AudioSource fireSound;

    void Start(){
        timer = fireRatePerSecond;
        baseFireRatePerSecond = fireRatePerSecond;

        baseDamage = damage;

        //use this gameobject if there is no firepoint set
        if(firePoint == null){
            firePoint = gameObject;
        }
    }

    void Update(){
        timer += Time.deltaTime;

        //reduce recoil over time
        if(currRecoil >= 0){
            currRecoil -= recoilStablityRate * Time.deltaTime; // remove recoil based on time since last shot fired

            //currRecoil can't go below min
            if(currRecoil < 0){
                currRecoil = 0;
            }
        }

        //performs checks inside function
        LaserSightFire();
        
    }

    bool CanShoot(){
        if(timer >= 1/fireRatePerSecond){
            timer = 0f;
            return true;
        }
        return false;
    }

    void PlaySound(AudioSource passedSound){
        if(passedSound != null){
            passedSound.Play();
        }
    }

    void AddRecoil(){
        if(currRecoil <= maxRecoil){
            currRecoil += recoilStrength;

            //limit recoil;
            if(currRecoil > recoilStrength){
                currRecoil = maxRecoil;
            }
        }
    }

    #region Combined 3d/2d
    public bool Fire(){
        if(dimension == Dimension.three){
            if(Fire3D().transform != null){
                return true;
            }else return false;
        }

        if(dimension == Dimension.two){
            if(Fire2D().transform != null){
                return true;
            }else return false;
        }

        print("Shooting Module can't fire 2d or 3d");
        return false;
    }

    //good for telling the player what they're going to hit
    void LaserSightFire(){
        if(laserSight != null){
            if(usingLaserSight == true){

                laserSight.enabled = true;

                if(dimension == Dimension.three){
                    RaycastHit hit = CheckFire3D();
                    laserSight.SetPosition(0, firePoint.transform.position);
                    laserSight.SetPosition(laserSight.positionCount - 1, hit.point);
                }

                if(dimension == Dimension.two){
                    RaycastHit2D hit = CheckFire2D();
                    laserSight.SetPosition(0, firePoint.transform.position);
                    laserSight.SetPosition(laserSight.positionCount - 1, hit.point);
                }

            }else{
                laserSight.enabled = false;
            }
        }
    }

    #endregion
    
    #region  3d Shooting
    public RaycastHit Fire3D(){
        RaycastHit hit = new RaycastHit();
        if(CanShoot()){

            //fire shot
            if(isRaycastShoot){ // checks the type of shot
                hit = FireRayCast3D();
            }else{
                //shoot bullet if it exists
                if(bullet != null){
                    hit = FireBullet3D();
                }else{
                    print("Bullet prefab doesn't exist. Please switch to raycast shooting");
                }
            }

            PlaySound(fireSound);

            //adds recoil per shot
            AddRecoil();
        }
        return hit;
    }

    public RaycastHit CheckFire3D(){
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(Random.Range(-currRecoil, currRecoil), Random.Range(-currRecoil, currRecoil), 0), out hit, float.PositiveInfinity);
        return hit;
    }

    RaycastHit FireRayCast3D(){
        RaycastHit hit = CheckFire3D();
        //apply force
        if(hit.transform != null && doesShotApplyForce == true){
            if(hit.transform.GetComponent<Rigidbody>() != null){
                hit.transform.GetComponent<Rigidbody>().AddForce((hit.point - hit.transform.position) * -force);
            }
        }

        //apply damage
        if(hit.transform != null){
            if(hit.transform.gameObject.GetComponent<LifeModule>() != null){
                hit.transform.gameObject.GetComponent<LifeModule>().Damage(damage);
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

    RaycastHit FireBullet3D(){
        RaycastHit hit = CheckFire3D();
        
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

    #endregion

    #region 2d shooting
    public RaycastHit2D Fire2D(){
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
        hit = Physics2D.Raycast(firePoint.transform.position, firePoint.transform.right + new Vector3(Random.Range(-currRecoil, currRecoil), Random.Range(-currRecoil, currRecoil), 0), float.PositiveInfinity);
        // print(hit.collider);
        Debug.DrawRay(firePoint.transform.position, firePoint.transform.right * float.PositiveInfinity, Color.red, 10f);
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
            if(hit.transform.gameObject.GetComponent<LifeModule>() != null){
                hit.transform.gameObject.GetComponent<LifeModule>().Damage(damage);
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

    #endregion

    #region Extra functions
    public float GetBaseDamage(){
        return baseDamage;
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

    #endregion
}
