using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButtonHider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endTurnButton;
    public GameObject DrawEventCard;
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
        }
        else {
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