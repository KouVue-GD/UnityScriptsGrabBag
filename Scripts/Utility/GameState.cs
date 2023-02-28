using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
#region  LockScreen
    [Header("LockScreen")]
    [Header("ProjectSettings->InputManager")]
    [SerializeField] string input;
    bool isInputDown = false;
    void Update() {
        //once per button press
        if(Input.GetAxisRaw(input) == 1 && isInputDown == false){
            LockScreen();
            isInputDown = true;
        }

        if(Input.GetAxisRaw(input) == 0){
            isInputDown = false;
        }
    }

    public void LockScreen(){
        if(Cursor.lockState == CursorLockMode.Locked){
            Cursor.lockState = CursorLockMode.None;
        }else if(Cursor.lockState == CursorLockMode.None){
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
#endregion

#region  Pause
    bool pause = false;
    public void Pause(){
        if(pause == false){
            Time.timeScale = 0f;
            pause = true;
        }else{
            Time.timeScale = 1f;
            pause = false;
        }
    }
#endregion

#region  Reset
    public void Reset(){
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }
#endregion

#region Game Over
public void GameOver(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
#endregion

    
}
