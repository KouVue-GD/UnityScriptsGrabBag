using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    float currHealth;
    public float minHealth;
    public bool destroyOnDeath;


    public Animator animator;
    public AnimationClip hurt;
    public ParticleSystem damageParticles;
    public ParticleSystem healParticles;

    void Awake()
    {
        currHealth = MaxHealth;
    }

    public AudioSource deathSound;

    void Update()
    {

        if(destroyOnDeath == true){
            if(currHealth <= minHealth){
                if(deathSound != null){
                    deathSound.Play();
                }
                
                Destroy(gameObject);
            }
        }

        if(destroyOnDeath != true){
            if(currHealth <= minHealth){
                if(deathSound != null){
                    deathSound.Play();
                }
                
                Disable(gameObject);
            }
        }
    }

    void Disable(GameObject target){
        
    }

    public void SetCurrHealth(float passedValue){
        currHealth = passedValue;
    }

    public float GetCurrHealth(){
        return currHealth;
    }

    /// <summary>
    /// Deals Damage to currHealth based on passed value.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Damage(float damage){
        if(damage >= 0){
            currHealth -= damage;
            if(damageParticles != null){
                damageParticles.Play();
            }

            if(animator != null){
                if(hurt != null){
                    animator.Play(hurt.name, -1, 0);
                }
            }
        }
    }

    public void Heal(float amount){
        if(amount >= 0){
            currHealth += amount;
            if(healParticles != null){
                healParticles.Play();
            }
        }
    }

    public override string ToString(){
        return currHealth + "/" + MaxHealth;
    }
}
