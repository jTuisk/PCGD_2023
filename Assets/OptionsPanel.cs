using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    [SerializeField] private TMPro.TextMeshProUGUI masterSliderText;
    [SerializeField] private TMPro.TextMeshProUGUI musicSliderText;
    [SerializeField] private TMPro.TextMeshProUGUI sfxSliderText;
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Button mainMenuButton; 

    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            AudioManager.Instance.PlayOneShot(AudioManager.AudioEffects.flipCard);
        }
    }

    void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
        SetDefaultFullScreenOption();    
        SetDefaultVolumeValue();
    }

    void Start()
    {
    }

    void OnEnable()
    {
        if(SceneLoader.IsGameSceneActive())
        {
            mainMenuButton.gameObject.SetActive(true);
        }
        else
        {
            mainMenuButton.gameObject.SetActive(false);
        }
    }

    public void HideOptionsPanel()
    {
        this.gameObject.SetActive(false);
        
        TipTool[] tiptools = Object.FindObjectsOfType<TipTool>();
        for(int i = 0; i< tiptools.Length; i++)
        {
            tiptools[i].canShow = true;
        }
    }

    # region Audio
    public void OnMasterSliderValueChange()
    {
        masterSliderText.text = Mathf.RoundToInt(masterSlider.value).ToString();
        var mixerVolValue = masterSlider.value - 80;
        mainMixer.SetFloat("MasterVol", AudioManager.Instance.LinearToDecibel((mixerVolValue + 80) / 100));
        PlayerPrefs.SetFloat("MasterVol", mixerVolValue);
    }

    public void OnMusicSliderValueChange()
    {
        musicSliderText.text = Mathf.RoundToInt(musicSlider.value).ToString();
        var mixerVolValue = musicSlider.value - 80;
        mainMixer.SetFloat("MusicVol", AudioManager.Instance.LinearToDecibel((mixerVolValue+80)/100));
        PlayerPrefs.SetFloat("MusicVol", mixerVolValue);
    }

    public void OnSFXSliderValueChange()
    {
        sfxSliderText.text = Mathf.RoundToInt(sfxSlider.value).ToString();
        var mixerVolValue = sfxSlider.value - 80;
        mainMixer.SetFloat("SFXVol", AudioManager.Instance.LinearToDecibel((mixerVolValue + 80) / 100));
        PlayerPrefs.SetFloat("SFXVol", mixerVolValue);
    }

    private void SetDefaultVolumeValue()
    {
        if(PlayerPrefs.HasKey("MasterVol"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVol") + 80;
        }
        if(PlayerPrefs.HasKey("MusicVol"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol") + 80;
        }
        if(PlayerPrefs.HasKey("SFXVol"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVol") + 80;
        }
    }

    #endregion

    # region FullScreen
    public void OnFullScreenToggleValueChange()
    {
        # if UNITY_EDITOR
            var windows = (UnityEditor.EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(UnityEditor.EditorWindow));
            foreach(var window in windows)
            {
                if(window != null && window.GetType().FullName == "UnityEditor.GameView")
                {
                    window.maximized = fullScreenToggle.isOn;
                    break;
                }
            }
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        #else
            Screen.fullScreen = fullScreenToggle.isOn;
        # endif
        PlayerPrefs.SetInt("FullScreen", (fullScreenToggle.isOn)? 1: 0);
    }

    public void SetDefaultFullScreenOption()
    {
        if(PlayerPrefs.HasKey("FullScreen"))
        {
            bool isOn = (PlayerPrefs.GetInt("FullScreen") == 1)? true: false;
            fullScreenToggle.isOn = isOn;
        }
        OnFullScreenToggleValueChange();       
    }

    #endregion


    public void BackToMainMenu()
    {
        SceneLoader.LoadMainMenu();
    }
}
