using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

      static public void LoadGame(){
        SceneManager.LoadScene("Game");
    }
        static public void LoadMainMenu(){
            SceneManager.LoadScene("MainMenu");
        }
        static public void LoadGameOver(){
            SceneManager.LoadScene("GameOver");
        }
        
}
