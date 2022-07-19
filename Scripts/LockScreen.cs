using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uses Cancel axis to lock cursor to middle of screen
/// </summary>
public class LockScreen : MonoBehaviour
{
    bool isCancelDown;
    void Update() {
        if(Input.GetAxisRaw("Cancel") == 1 && isCancelDown == false){
            if(Cursor.lockState == CursorLockMode.Locked){
                Cursor.lockState = CursorLockMode.None;
            }else if(Cursor.lockState == CursorLockMode.None){
                Cursor.lockState = CursorLockMode.Locked;
            }
            isCancelDown = true;
        }else{
            isCancelDown = false;
        }
    }
}
