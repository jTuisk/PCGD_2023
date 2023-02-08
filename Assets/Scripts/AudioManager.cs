using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    public AudioSource audioSource;
    public AudioClip exploreCardAudioEffect;
    public AudioClip flipCardAudioEffect;
    public AudioClip playCardAudioEffect;
    public AudioClip dealDamageAudioEffect;
    public AudioClip healSelfAudioEffect;
    public AudioClip getManaAudioEffect;
    public AudioClip loseManaAudioEffect;
    public AudioClip shuffleDeckAudioEffect;

    public AudioClip battleBGM; 

    [HideInInspector]
    public enum AudioEffects {exploreCard, flipCard, playCard, 
        healSelf, getMana, loseMana, dealDamage, 
        shuffleDeck};

    public void PlayOneShot(AudioEffects audio)
    {
        switch(audio)
        {
            case(AudioEffects.exploreCard):
            {
                audioSource.PlayOneShot(exploreCardAudioEffect);
                break;
            }
            case(AudioEffects.flipCard):
            {
                audioSource.PlayOneShot(flipCardAudioEffect);
                break;
            }
            case(AudioEffects.playCard):
            {
                audioSource.PlayOneShot(playCardAudioEffect);
                break;
            }
            case(AudioEffects.dealDamage):
            {
                audioSource.PlayOneShot(dealDamageAudioEffect);
                break;
            }      
            case(AudioEffects.healSelf):
            {
                audioSource.PlayOneShot(healSelfAudioEffect);
                break;
            }            
            case(AudioEffects.getMana):
            {
                audioSource.PlayOneShot(getManaAudioEffect);
                break;
            }
            case(AudioEffects.loseMana):
            {
                audioSource.PlayOneShot(loseManaAudioEffect);
                break;
            }
            case(AudioEffects.shuffleDeck):
            {
                audioSource.PlayOneShot(shuffleDeckAudioEffect);
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

    void Awake()
    {
        Instance = this;

        // check audio source is available

    }

}
