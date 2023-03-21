using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIDragDropEvent : MonoBehaviour
{
    RectTransform rectTransform;
    Vector3 offset;
    void Start(){
        rectTransform = GetComponent<RectTransform>();
    }

    public void GetOffset(){
        offset = rectTransform.position - Input.mousePosition;
    }

    public void MoveObject(){
        rectTransform.position = Input.mousePosition + offset;
    }
}
