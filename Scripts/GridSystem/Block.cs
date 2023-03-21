using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
public enum ColorType{
        white, grey, tan, orange, redOrange, purple, blue, green, red
    }
    
    [SerializeField] ColorType colorType;

    public ColorType GetColorType(){
        return colorType;
    }

}
