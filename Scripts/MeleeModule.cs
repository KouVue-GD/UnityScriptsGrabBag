using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeModule : MonoBehaviour
{
    [SerializeField] List<string> validTargetTags;
    [SerializeField] float damage;
    [SerializeField] float force;
    [SerializeField] Animator anim;
    [SerializeField] float nextAttackDelay;
    int index = 0;
    float timeBetweenAttacks = 0;
    float timer;
    bool canAttack;
    bool canHit;


    [SerializeField] List<AnimationClip> animationMelee;

    void Start(){
        if(anim != null){
            timeBetweenAttacks = anim.GetCurrentAnimatorStateInfo(0).length;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //reset attack if no input
        if(timer >= timeBetweenAttacks){
            index = 0;
            timer = 0;
            canHit = false;

            //only delays the start of next attack
            canAttack = false;
        }

        //delay start of next attack
        if(timer > nextAttackDelay && canAttack == false){
            canAttack = true;
        }

        timer += Time.deltaTime;
    }

    public void Melee(){

        //if the next attack has started
        if(canAttack == true){
            MeleeCheck();
        }
    }

    void MeleeCheck(){
        //play first animation
        if(index == 0){
            PlayAnimation(index, animationMelee);
            timeBetweenAttacks = anim.GetCurrentAnimatorStateInfo(0).length;
            timer = 0;
            canHit = true;
        }

        //play next animation if it exists
        if( anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && index < animationMelee.Count && anim.GetCurrentAnimatorStateInfo(0).IsName(animationMelee[index].name)){
            index += 1;
            if(index < animationMelee.Count){
                PlayAnimation(index, animationMelee);
                timeBetweenAttacks = anim.GetCurrentAnimatorStateInfo(0).length;
                timer = 0;
                canHit = true;
            }
        }
    }

    void PlayAnimation(int pIndex, List<AnimationClip> pAnim){
        anim.Play(pAnim[pIndex].name, 0);
    }

    void OnTriggerEnter(Collider coll){
         //during animation let the enemy get hurt
        if( canHit == true){
            // print(coll);
            //if it is a valid target
            foreach (var item in validTargetTags)
            {
                if(coll.transform.CompareTag(item)){
                    coll.transform.GetComponent<LifeModule>().Damage(damage);
                    coll.transform.GetComponent<Rigidbody>().AddForce((gameObject.transform.position - coll.ClosestPoint(gameObject.transform.position)).normalized * force);
                }

            }
        }
    }
}
