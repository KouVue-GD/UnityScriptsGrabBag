using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision2D : MonoBehaviour
{
    public List<string> invalidTargetTags;

    // Start is called before the first frame update
    void Start()
    {
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
        
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
