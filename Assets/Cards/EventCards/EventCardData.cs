using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New EventCard", menuName = "Cards/Event/Normal")]
public class EventCardData : ScriptableObject
{
    //public UnityEvent e;
    [TextArea(3,10)]
    public string eventText;
    //public string eventButtonText;

    public List<EventCardMenuItem> options;

    public void takeDamage(){
        Deck.Instance.takeDamage(1);
    }
    public void HealingFountain()
    {
        Deck.Instance.Hp += 5;


    }
}
