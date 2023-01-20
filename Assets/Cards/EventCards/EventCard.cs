using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCard : MonoBehaviour
{
    public UnityEvent e;
    // Start is called before the first frame update
    public void Activate()
    {
        e.Invoke();
        Deck.Instance.eventVisible = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
