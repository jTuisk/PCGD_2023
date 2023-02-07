using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class EventCard : MonoBehaviour
{
   // public UnityEvent e;
   public string eName;
    public TMPro.TextMeshProUGUI text;
    public TMPro.TextMeshProUGUI buttonText;
    public GameObject Menu;
    public GameObject ButtonPrefab;
    public List<EventCardMenuItem> options;
    // Start is called before the first frame update
    public GameObject CreatureBase;
    public CreatureDataContainer Creature;


    public void CreateEventCard(EventCardData data) {
        name=data.name;
        //e = data.e;
        text.text = data.eventText;
        //buttonText.text = data.eventButtonText;
        options = data.options;
        foreach (EventCardMenuItem option in options) {
            bool visible = true;
            foreach (Condition con in option.conditions) {
                if (!con.Evaluate()) {
                    visible = false;
                }
            
            }
            if (visible)
            {
                var button = Instantiate(ButtonPrefab);
                button.GetComponent<Button>().onClick.AddListener(delegate { Activate(option.effect); });
                //button.GetComponent<Button>().onClick.AddListener(delegate { Activate(option.effects); });

                button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = option.description;
                button.transform.parent = Menu.transform;
            }
        }
        if (Menu.transform.childCount == 0) {
            var button = Instantiate(ButtonPrefab);
            button.GetComponent<Button>().onClick.AddListener(delegate { Activate(); });
            button.transform.parent = Menu.transform;
        }
    }
    
    public void Activate(List<Effect> effects)
    {
        foreach(var effect in effects)
        {
            if(effect.possibility == 0)
                continue;

            var v = Random.value;
            var p = effect.possibility;
            if(v <= p)
            {
                effect.targetEvent.Invoke();
                Debug.Log("'" + effect.name +"' takes place cuz a " + p * 100 + "% chance of happening is fulfilled by random value " + v);
            }
            else
            {
                Debug.Log("'" + effect.name +"' won't take place cuz a " + p * 100 + "% chance of happening is not fulfilled by random value " + v);
            }
        }
        Deck.Instance.eventVisible = false;
        Destroy(gameObject);
    }

    public void Activate(UnityEvent e)
    {
        //activate event effect and change gamestate back to drawing event cards.
        
        Deck.Instance.eventVisible = false;
        e.Invoke();
        Destroy(gameObject);
    }
    public void Activate()
    {
        //activate event effect and change gamestate back to drawing event cards.
        
        Deck.Instance.eventVisible = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
