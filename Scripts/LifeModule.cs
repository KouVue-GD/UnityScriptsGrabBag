using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeModule : MonoBehaviour
{
#region  Variables
    [Serializable] enum DeathType{
        DestroyOnDeath, DisableSelfOnDeath
    }

    [Header("Choose what happens when you die")]
    [SerializeField] DeathType deathType;

    [Header("Health Amount")]
    [SerializeField] float MaxHealth = 100;
    [SerializeField] float minHealth = 0;
    float currHealth;

    [Header("Animation controller")]
    [SerializeField] Animator animator;

    [Header("Upon Spawning...")]
    [SerializeField] AnimationClip spawnAnim;
    [SerializeField] AudioSource spawnAudio;
    [SerializeField] ParticleSystem spawnParticles;

    [Header("Upon taking damage...")]
    [SerializeField] AnimationClip damageAnim;
    [SerializeField] AudioSource damageSound;
    [SerializeField] ParticleSystem damageParticles;

    // [Header("Upon healing damage...")]
    // [SerializeField] AnimationClip healAnim;
    // [SerializeField] AudioSource healSound;
    // [SerializeField] ParticleSystem healParticles;

    [Header("Upon dying...")]
    [SerializeField] AnimationClip deathAnim;
    [SerializeField] AudioSource deathSound;
    [SerializeField] ParticleSystem deathParticles;

#endregion

#region Unity Functions
        void Start()
        {
            SpawnEffects();
        }

        void Awake()
        {
            //incase it starts and needs to get hurt right away
            currHealth = MaxHealth;
        }

        void Update()
        {
            if(deathType == DeathType.DestroyOnDeath){
                if(currHealth <= minHealth){
                    DestroyOnDeath();
                }
            }

            if(deathType == DeathType.DestroyOnDeath){
                if(currHealth <= minHealth){
                    DisableSelfOnDeath();
                }
            }
        }
#endregion

#region Spawn
    //Un-needed function but done for the sake of consistancy (alternative is to put directly into start)
    void SpawnEffects(){
        PlayEffects(spawnParticles, spawnAnim, spawnAudio);
    }
#endregion

#region  Damage
    public void Damage(float damage){
        if(damage >= 0){
            currHealth -= damage;

            PlayEffects(damageParticles, damageAnim, damageSound);
        }
    }

#endregion

#region  Healing

    // public void Heal(float amount){
    //     if(amount >= 0){
    //         currHealth += amount;
    //         if(healParticles != null){
    //             healParticles.Play();
    //         }
    //     }
    // }

#endregion

#region  Death
    void DestroyOnDeath(){
        
        Destroy(gameObject, 3);
        PlayEffects(deathParticles, deathAnim, deathSound);
        
    }
#pragma warning disable
    bool hasDisabled = false;
#pragma warning restore
    [Header("For DisableSelfOnDeath")]
    [SerializeField] List<Component> listToDisable;
    void DisableSelfOnDeath(){
        if(gameObject.activeSelf == true){
            // MonoBehaviour[] mb = GetComponentsInChildren<MonoBehaviour>();

            // //destroy all components
            // for (int i = 0; i < mb.Length; i++)
            // {
            //     // print(mb[i]);
            //     //mb[i].enabled = false;
            //     Destroy(mb[i]);
            // }

            foreach (var item in listToDisable)
            {
                Destroy(item);
            }

            // gameObject.SetActive(false);

            if(GetComponent<Rigidbody>() != null){
                GetComponent<Rigidbody>().useGravity = true;
            }

            hasDisabled = true;

            PlayEffects(deathParticles, deathAnim, deathSound);
        }
    }

#endregion

#region  Extra functions

    public override string ToString(){
        return currHealth + "/" + MaxHealth;
    }

    public void SetCurrHealth(float passedValue){
        currHealth = passedValue;
    }

    public float GetCurrHealth(){
        return currHealth;
    }

    void PlayEffects(ParticleSystem ps, AnimationClip animationClip, AudioSource audioSource){
        if(ps != null){
            ps.Play();
        }

        if(animator != null){
            animator.Play(animationClip.name);
        }

        if(audioSource != null){
            audioSource.Play();
        }
    }

#endregion


}
