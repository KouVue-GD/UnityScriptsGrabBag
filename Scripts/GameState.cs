using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
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

    
}
