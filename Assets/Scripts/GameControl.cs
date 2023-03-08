using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public GameObject optionsPanelPrefab;
    private GameObject optionsPanelObj;

    void Awake()
    {
        var optionPanel = Object.FindObjectOfType<OptionsPanel>();
        if(optionPanel == null)
        {
            optionsPanelObj = Instantiate(optionsPanelPrefab);
        }
        HideOptionsPanel();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneLoader.IsGameSceneActive())
            {
                if(!optionsPanelObj.activeInHierarchy)
                {
                    ShowOptionsPanel();
                }
                else
                {
                    HideOptionsPanel();
                }
            }
        }
    }

    public void QuitGame()
    {
        # if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        # else
            Application.Quit();
        # endif
    }

    public void ShowOptionsPanel()
    {
        optionsPanelObj.SetActive(true);

        // disable all tip tools
        TipTool[] tiptools = Object.FindObjectsOfType<TipTool>();
        for(int i = 0; i< tiptools.Length; i++)
        {
            tiptools[i].canShow = false;
        }
    }

    public void HideOptionsPanel()
    {
        optionsPanelObj.SetActive(false);
        
        TipTool[] tiptools = Object.FindObjectsOfType<TipTool>();
        for(int i = 0; i< tiptools.Length; i++)
        {
            tiptools[i].canShow = true;
        }
    }

    public bool IsOptionPanelActive()
    {
        return optionsPanelObj.activeInHierarchy;
    }

}
