using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButtonHider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endTurnButton;
    public GameObject DrawEventCard;
    public GameObject deckImage;
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
            endTurnButton.SetActive(true);
            deckImage.SetActive(true);
            discardPileImage.SetActive(true);
        }
        else {
            deckImage.SetActive(false);
            discardPileImage.SetActive(false);
            endTurnButton.SetActive(false);
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
