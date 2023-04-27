using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonFlicker : ButtonFlicker
{
    // Start is called before the first frame update


    // Update is called once per frame
    public bool HasValidMoves()
    {
        //if performance issues cache this
        
        foreach(Transform  g in Deck.Instance.Hand.transform)
        {
            var card = g.gameObject.GetComponent<Card>();
           // Debug.Log(card.actionCost * card.actionCostMultiplier + " < " + Deck.Instance.actionPoints+ "   " + card.magic + " < " + Deck.Instance.mana);
            if(card.actionCost * card.actionCostMultiplier <= Deck.Instance.actionPoints && -card.magic <= Deck.Instance.mana)
            {
                return true;
            }
            
        }
        return false;


    }

    protected override void Update()
    {
    
            Flickering = !HasValidMoves();
        
        base.Update();
    }
}
