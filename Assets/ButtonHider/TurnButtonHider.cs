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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //hide inactive UI Elements
        if (Deck.Instance.inBattle)
        {
            if (!ContentsOfDeck.Instance.gameObject.activeSelf)
            {
                if (Deck.Instance.enemyTurn)
                {
                    endTurnButton.SetActive(true);
                }
                else
                {
                    endTurnButton.SetActive(false);
                }
                deckImage.SetActive(true);
                discardPileImage.SetActive(true);
                EventdeckImage.gameObject.SetActive(false);
                deckText.text = "";
            }
            else
            {
                HideEverything();
            }
        }
        else
        {
            if (!ContentsOfDeck.Instance.gameObject.activeSelf)
            {
                EventdeckImage.gameObject.SetActive(true);
                deckImage.SetActive(false);
                discardPileImage.SetActive(false);
                endTurnButton.SetActive(false);
                deckText.text = "Cards left in event deck: " + (16-Deck.Instance.bossCounter-Deck.Instance.bosses);
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
    }

    private void HideEverything()
    {
        EventdeckImage.gameObject.SetActive(false);
        deckImage.SetActive(false);
        discardPileImage.SetActive(false);
        endTurnButton.SetActive(false);
        deckText.text = "";
    }
}
