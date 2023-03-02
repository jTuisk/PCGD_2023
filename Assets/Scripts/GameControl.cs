using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public GameObject optionsPanelPrefab;
    private GameObject optionsPanel;

    void Awake()
    {
        optionsPanel = Instantiate(optionsPanelPrefab);
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
        optionsPanel.SetActive(true);
    }

    public void HideOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }


}
