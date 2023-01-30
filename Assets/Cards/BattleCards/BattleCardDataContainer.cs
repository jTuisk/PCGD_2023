using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (fileName ="New BattleCard", menuName ="Cards/Battle")]
public class BattleCardDataContainer : ScriptableObject
{
    public string effectDescriptor = "";
    public string cardName="";
    public int Damage = 0;
    public int block = 0;
    public int magic = 0;
    public int money = 0;
    public int actionCost = 0;
    public UnityEvent effect;




    public void addCard()
    {
        Deck.Instance.BattleDeckAddCardFromCardData(this);

    }
    public void drawCard(){
        Deck.Instance.DrawCardInHand(1);
    }
    public void discardRandom()
    {
        Deck.Instance.discardRandom();

    }
    public void removeRandomCardFromDeck()
    {
        if (Deck.Instance.BattleDeck.Count > 0)
        {
            var c = Deck.Instance.BattleDeck[Random.Range(0, Deck.Instance.BattleDeck.Count)];
            Deck.Instance.BattleDeck.Remove(c);
            Destroy(c.gameObject);
        }
    }

}
