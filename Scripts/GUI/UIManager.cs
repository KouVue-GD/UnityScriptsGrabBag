using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // public GameObject target;

    public void ToggleActive(GameObject target){
        target.SetActive(!target.activeSelf);
    }
}
