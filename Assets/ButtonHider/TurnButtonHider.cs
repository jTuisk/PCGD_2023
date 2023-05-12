using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnButtonHider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endTurnButton;
    public GameObject DrawEventCard;
    public GameObject deckImage;
    public GameObject discardPileImage;
    public Image EventdeckImage;
    public TMPro.TextMeshProUGUI deckText;
    public GameObject AP;
    public GameObject armor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Deck.Instance.block > 0)
        {
            armor.SetActive(true);
        }
        else
        {
            armor.SetActive(false);
        }

        //hide inactive UI Elements
        if (Deck.Instance.inBattle)
        {
            EventdeckImage.gameObject.SetActive(false);
            AP.gameObject.SetActive(true);
            if (!ContentsOfDeck.Instance.gameObject.activeSelf)
            {
                if (!Deck.Instance.stunned&&Deck.Instance.enemyTurn && Deck.Instance.enemy.HP>0)
                {
                    endTurnButton.SetActive(true);
                }
                else
                {
                    endTurnButton.SetActive(false);
                }
                deckImage.SetActive(true);
                discardPileImage.SetActive(true);
                //EventdeckImage.gameObject.SetActive(false);
                deckText.text = "";
            }
            else
            {
                HideEverything();
            }
        }
        else
        {
            AP.gameObject.SetActive(false);
            EventdeckImage.gameObject.SetActive(true);
            if (!ContentsOfDeck.Instance.gameObject.activeSelf)
            {
                EventdeckImage.gameObject.SetActive(true);
                //deckImage.SetActive(false);
                discardPileImage.SetActive(false);
                endTurnButton.SetActive(false);
                deckText.text = "Cards left in event deck: " + (Deck.Instance.isbit1Final? $"{Deck.Instance.Bit1Final.Count}" : (16-Deck.Instance.bossCounter-Deck.Instance.bosses));
            }
            else
            {
                HideEverything();
            }

        }

        if(Deck.Instance.inReward)
        {
            deckText.text="";
        }

        if (!Deck.Instance.eventVisible && !Deck.Instance.inBattle && !Deck.Instance.inReward)
        {
            DrawEventCard.SetActive(true);
        }
        else
        {
            DrawEventCard.SetActive(false);
        }
        if(ContentsOfDeck.Instance.gameObject.activeInHierarchy){
            deckImage.SetActive(false);}else{
            deckImage.SetActive(true);
        }
    }

    private void HideEverything()
    {
        EventdeckImage.gameObject.SetActive(false);
        discardPileImage.SetActive(false);
        endTurnButton.SetActive(false);
        deckText.text = "";
    }

}
