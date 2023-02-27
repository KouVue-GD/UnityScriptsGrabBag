using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb2d;

    public List<string> invalidTargetTags;

    [SerializeField] float secondsBeforeDestruction;
    float timer;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        foreach (var item in invalidTargetTags)
        {
            GameObject temp = GameObject.FindGameObjectWithTag(item);
            if(temp != null){
                Physics2D.IgnoreCollision(temp.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = transform.right * speed;

        timer += Time.deltaTime;
        if(timer >= secondsBeforeDestruction){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {

        Destroy(gameObject);
    }
    [SerializeField] GameObject explosion;
    void OnDestroy(){
        if(Application.isPlaying){
            GameObject temp = Instantiate(explosion, null);
            temp.transform.position = gameObject.transform.position;
            // Destroy(temp, 3f);
        }
    }
}
