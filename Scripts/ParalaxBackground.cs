using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    public Material material;
    public float parallaxFactor;
    Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        offset = material.mainTextureOffset;
        offset.x = transform.position.x / transform.localScale.x / parallaxFactor;
        offset.y = transform.position.y / transform.localScale.y / parallaxFactor;
        
        material.mainTextureOffset = offset;
    }
}
