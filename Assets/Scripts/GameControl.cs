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
    }

    public void HideOptionsPanel()
    {
        optionsPanelObj.SetActive(false);
    }


}
