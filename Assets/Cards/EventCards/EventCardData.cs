using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New EventCard", menuName = "Cards/Event/Normal")]
public class EventCardData : ScriptableObject
{
    public string eName;
    //public UnityEvent e;
    [TextArea(3,10)]
    public string eventText;
    //public string eventButtonText;

    public List<EventCardMenuItem> options;

    public BattleCardDataContainer ReplaceFrom;
    public BattleCardDataContainer ReplaceTo;
    public void takeDamage(){
        Deck.Instance.takeDamage(1);
    }
    public void HealingFountain()
    {
        Deck.Instance.Hp += 5;


    }
    public void replaceCardWrapper(){
        Deck.Instance.ReplaceAllCardsOfType(Deck.Instance.BattleDeck,ReplaceFrom,ReplaceTo);

    }
}
