using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnButtonHider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endTurnButton;
    public GameObject DrawEventCard;
    public Image deckImage;
    public Image EventdeckImage;
    public TMPro.TextMeshProUGUI deckText;
    public GameObject discardPileImage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //hide inactive UI Elements
        if (Deck.Instance.inBattle)
        {
            if(Deck.Instance.enemyTurn){
                endTurnButton.SetActive(true);
            }else{
                endTurnButton.SetActive(false);
            }
            deckImage.gameObject.SetActive(true);
            EventdeckImage.gameObject.SetActive(false);
            discardPileImage.SetActive(true);
            deckText.text="";
        }
        else {
            deckImage.gameObject.SetActive(false);
            EventdeckImage.gameObject.SetActive(true);
            discardPileImage.SetActive(false);
            endTurnButton.SetActive(false);
            deckText.text="Cards left in event deck "+Deck.Instance.EventDeck.Count;
        }

        if (!Deck.Instance.eventVisible&& !Deck.Instance.inBattle)
        {
            DrawEventCard.SetActive(true);
        }
        else
        {
            DrawEventCard.SetActive(false);
        }

    }
}
