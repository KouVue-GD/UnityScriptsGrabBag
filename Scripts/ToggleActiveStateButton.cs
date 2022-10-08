using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveStateButton : MonoBehaviour
{
    public GameObject target;

    public void ToggleActive(){
        target.SetActive(!target.activeSelf);
    }
}
