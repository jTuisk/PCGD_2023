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
    static public void LoadGameVictory()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CreateAudioManager();
    }

    void CreateAudioManager()
    {
        var audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        var audioManager = FindObjectOfType<AudioManager>();
        if(audioSources.Length == 0 || audioManager == null)
        {
            var audioManagerObj = Resources.Load("AudioManager", typeof(GameObject));
            Instantiate(audioManagerObj);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(AudioManager.Instance != null)
        {
            switch(scene.name)
            {
                case "MainMenu":{AudioManager.Instance.PlayMainMenuBGM();break;}
                case "GameOver":{AudioManager.Instance.PlayGameOverBGM();break;}
                case "VictoryScreen":{AudioManager.Instance.PlayWinGameBGM();break;}
                default:break;
            }
        }

    }

}
