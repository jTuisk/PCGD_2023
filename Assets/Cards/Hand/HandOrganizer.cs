using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOrganizer : MonoBehaviour
{
    private bool[] cardInPlace = new bool[20];
    

    // Called when the player draw a card from the deck, the card will then move from deck to hand
    public GameObject tpMessage;
    void DrawACard()
    {
        //pick a card randomly from the deck (deck.BattleDeck)
        var card = Deck.Instance.DrawCard();
        //card.transform.gameObject.name = "new";

        //move the card from deck to hand
        var deckPos = Deck.Instance.getPosition();
        var startPos = new Vector3(deckPos.x, deckPos.y, transform.position.z);
        var endPos = transform.position;
        card.triggerMove(startPos, endPos);
        // TODO: should not use the position of hand, instead use the calculated position of card.

        //after moving, set the card as a child of hand gameObject.
    }

    // Start is called before the first frame update
    public float cardDistanceScalar=0.5f;
    void Start()
    {
    }

    // Update is called once per frame
    public float HitboxSize;
    private float heldCard=-1;
    void Update()
    {

        
        int i = 0;
        foreach(Transform child in transform)
        {
            var targetPos = new Vector3(-transform.childCount *cardDistanceScalar*10/2  + cardDistanceScalar*i * 10, -10-15*(1-child.localScale.x), 0);
            var card = child.GetComponent<Card>();
            
            //move card to mouse position if player drags mouse and card distance of the cursor is lower than hitboxsize
            var campos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousepos = new Vector3(campos.x, campos.y, 0);
            if ((mousepos - child.position).magnitude < HitboxSize&& Input.GetMouseButton(0)||Input.GetMouseButton(0)&&heldCard==i)
            {
                card.terminateMove(); // when the card is grabbed, card moving animation should be turn off

                if(heldCard == -1) // haven't held a card
                {
                    heldCard=i;
                }
                if(heldCard == i) // only one card can be held at a time
                {
                    child.position = mousepos;
                }
            }
            else
            {
                if(heldCard==i&&!Input.GetMouseButton(0)){
                    //child.gameObject.GetComponentInChildren<Canvas>().sortingOrder=0;
                    heldCard=-1;
                }

                //when player releases card and its y position is above 700 and the player can afford it play the card
                if (child.position.y > 5 && child.position.x < 700 && card.inHand == true)
                {
                    Debug.Log("play Card");
                    if (Deck.Instance.mana + card.magic >= 0)
                    {
                        
                        if (Deck.Instance.actionPoints >= card.actionCost)
                        {
                            Deck.Instance.actionPoints -= card.actionCost;
                            card.Playcard();
                            Deck.Instance.BattleDiscardPile.Add(card);
                            card.status = Card.BelongTo.DiscardPile;
                            child.parent = null;
                            child.position = new Vector2(10000, 100000);
                            break;
                        }else{
                            Instantiate(tpMessage).GetComponent<FailText>().changeTextString("Not enough toolpoints");
                        }
                    }else{
                                                    Instantiate(tpMessage).GetComponent<FailText>().changeTextString("Not enough mana");
                    }
                }
                
                if(child.position != targetPos && card.inHand == true)
                {
                    card.triggerMove(child.position, targetPos);
                    card.inHand = false;
                }


                //move cards to correct position
                // child.position = new Vector3(-transform.childCount * 10/2  + i * 10, -10, 0);
                // cardInPlace[i] = false;
            }
            i++;
        }
    }

    
}
