using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void SwitchActiveState(GameObject objectToSwitch){
        objectToSwitch.SetActive(!objectToSwitch.activeSelf);
    }
}
