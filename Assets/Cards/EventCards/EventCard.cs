using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCard : MonoBehaviour
{
    public UnityEvent e;
    public TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    public GameObject CreatureBase;
    public CreatureDataContainer Creature;
    public void CreateEventCard(EventCardData data) {
        e = data.e;
        text.text = data.eventText;
    
    }
    public void Activate()
    {
        //activate event effect and change gamestate back to drawing event cards.
        e.Invoke();
        Deck.Instance.eventVisible = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
