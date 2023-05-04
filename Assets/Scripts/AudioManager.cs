using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioSource musicAudioSource; 
    [SerializeField] private AudioSource sfxAudioSource; //sound effects

    [SerializeField] private AudioMixerGroup musicAudioMixer;
    [SerializeField] private AudioMixerGroup sfxAudioMixer;
 

    public AudioClip exploreCardAudioEffect;
    public AudioClip flipCardAudioEffect;
    public AudioClip playCardAudioEffect;
    public AudioClip dealDamageAudioEffect;
    public AudioClip healSelfAudioEffect;
    public AudioClip getManaAudioEffect;
    public AudioClip loseManaAudioEffect;
    public AudioClip shuffleDeckAudioEffect;
    public AudioClip stunEffect;
    public AudioClip shieldEffect;

    public AudioClip battleBGM; 

    public AudioClip eventCardDraw;
    public AudioClip eventCardSlide;
    public AudioClip eventCardFlip;
    public AudioClip eventCardBGM;

    public AudioClip mainMenuBGM;
    public AudioClip GameOverAudioClip;
    public AudioClip winGameAudioClip;

    public AudioClip bossLaughter;

    [HideInInspector]
    public enum AudioEffects {exploreCard, flipCard, playCard, 
        healSelf, getMana, loseMana, dealDamage, 
        shuffleDeck,stun,shield};

    public void PlayOneShot(AudioEffects audio)
    {
        switch(audio)
        {
            case(AudioEffects.exploreCard):
            {
                sfxAudioSource.PlayOneShot(exploreCardAudioEffect);
                break;
            }
            case(AudioEffects.flipCard):
            {
                sfxAudioSource.PlayOneShot(flipCardAudioEffect);
                break;
            }
            case(AudioEffects.playCard):
            {
                sfxAudioSource.PlayOneShot(playCardAudioEffect);
                break;
            }
            case(AudioEffects.dealDamage):
            {
                sfxAudioSource.PlayOneShot(dealDamageAudioEffect);
                break;
            }      
            case(AudioEffects.healSelf):
            {
                sfxAudioSource.PlayOneShot(healSelfAudioEffect);
                break;
            }            
            case(AudioEffects.getMana):
            {
                sfxAudioSource.PlayOneShot(getManaAudioEffect);
                break;
            }
            case(AudioEffects.loseMana):
            {
                sfxAudioSource.PlayOneShot(loseManaAudioEffect);
                break;
            }
            case(AudioEffects.shuffleDeck):
            {
                sfxAudioSource.PlayOneShot(shuffleDeckAudioEffect);
                break;
            }
            case (AudioEffects.stun):
                {
                    sfxAudioSource.PlayOneShot(stunEffect);
                    break;
                }
            case (AudioEffects.shield):
                {
                    sfxAudioSource.PlayOneShot(shieldEffect);
                    break;
                }
            default: break;
        }
    }

    public void PlayCardEffectsWhenCardPlayed(Card card)
    {
        if(card.Damage > 0)
        {
            PlayOneShot(AudioEffects.dealDamage);
        }
        else if(card.magic > 0)
        {
            PlayOneShot(AudioEffects.getMana);
        }
        else if(card.magic < 0)
        {
            PlayOneShot(AudioEffects.loseMana);
        }
        // TODO heal
    }

    public void PlayEventCardDrawSound()
    {
        sfxAudioSource.PlayOneShot(eventCardDraw);
    }

    public void PlayEventCardSlideSound()
    {
        sfxAudioSource.PlayOneShot(eventCardSlide);
    }

    public void PlayEventCardFlipSound()
    {
        sfxAudioSource.PlayOneShot(eventCardFlip);
    }

    public void PlayDrawEventCardBGM()
    {
        musicAudioSource.clip = eventCardBGM;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayWinGameBGM()
    {
        musicAudioSource.Stop();
        musicAudioSource.PlayOneShot(winGameAudioClip);
    }

    public void PlayMainMenuBGM()
    {
        musicAudioSource.clip = mainMenuBGM;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayGameOverBGM()
    {
        musicAudioSource.Stop();
        musicAudioSource.PlayOneShot(GameOverAudioClip);
        musicAudioSource.PlayOneShot(bossLaughter);
    }


    public void playBattleBGM(){
        musicAudioSource.loop=true;
        musicAudioSource.clip=battleBGM;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    void Awake()
    {
        Instance = this;
        
        //check if there are more than one audioManager in the scene
        var audioManagerObjs = GameObject.FindObjectsOfType<AudioManager>();
        if(audioManagerObjs.Length > 1)
        {
            Destroy(this);
            Destroy(gameObject);
        }

        // check audio source is available
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        musicAudioSource.outputAudioMixerGroup = musicAudioMixer;
        sfxAudioSource.outputAudioMixerGroup = sfxAudioMixer;

        SetDefaultVolumeValue();
    }

    private void SetDefaultVolumeValue()
    {
        SetDefaultVolumeValue("MasterVol");
        SetDefaultVolumeValue("MusicVol");
        SetDefaultVolumeValue("SFXVol");
    }
    public float DecibelToLinear(float decibel)
    {
        return Mathf.Pow(10f, decibel / 20f);
    }
    public float LinearToDecibel(float linear)
    {
        return  20f*Mathf.Log10(linear);
    }
    private void SetDefaultVolumeValue(string name)
    {
        if(PlayerPrefs.HasKey(name))
        {
            mainMixer.SetFloat(name, AudioManager.Instance.LinearToDecibel((PlayerPrefs.GetFloat(name) + 80) / 100));
        }
        else
        {
            var mixerVolValue = -10f;
            mainMixer.SetFloat(name, AudioManager.Instance.LinearToDecibel((mixerVolValue + 80) / 100));
            PlayerPrefs.SetFloat(name, mixerVolValue);
        }
    }


    void Update(){
        if(Deck.Instance != null)
        {
            if(Deck.Instance.enemy==null && musicAudioSource.clip==battleBGM){
                musicAudioSource.Stop();
                musicAudioSource.clip=null;
            }
        }
    }

}
