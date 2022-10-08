using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    Health health;
    public Animator animator;
    public AnimationClip deathAnim;
    public ParticleSystem deathParticles;
    public bool destroyOnDeath;
    public bool disableSelfOnDeath;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyOnDeath == true){
            DestroyOnDeath();
        }

        if(disableSelfOnDeath == true){
            DisableSelfOnDeath();
        }
    }

    void DestroyOnDeath(){
        
        if(health.GetCurrHealth() <= health.minHealth){
            Destroy(gameObject);
        }
        
    }

    bool hasDisabled = false;
    void DisableSelfOnDeath(){
        if(health.GetCurrHealth() <= health.minHealth){
            if(hasDisabled == false){
                MonoBehaviour[] mb = GetComponentsInChildren<MonoBehaviour>();

                for (int i = 0; i < mb.Length; i++)
                {
                    // print(mb[i]);
                    //mb[i].enabled = false;
                    Destroy(mb[i]);
                }

                if(GetComponent<Rigidbody>() == true){
                    GetComponent<Rigidbody>().useGravity = true;
                }
                hasDisabled = true;

                if(deathParticles != null){
                    deathParticles.Play();
                }

                if(animator != null){
                    animator.Play(deathAnim.name);
                }
            }
        }
    }
}
