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
    public TMPro.TextMeshProUGUI title;
    public TMPro.TextMeshProUGUI buttonText;
    public GameObject Menu;
    public GameObject ButtonPrefab;
    public List<EventCardMenuItem> options;
    // Start is called before the first frame update
    public GameObject CreatureBase;
    public CreatureDataContainer Creature;


    public void CreateEventCard(EventCardData data) {
        eName=data.eName;
        if(eName.Equals("")){
            eName=data.name;
        }
        //e = data.e;
        text.text = data.eventText;
        //buttonText.text = data.eventButtonText;
        options = data.options;
        title.text=eName;
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
                //button.GetComponent<Button>().onClick.AddListener(delegate { Activate(option.effect); });
                button.GetComponent<Button>().onClick.AddListener(delegate { Activate(option.effects,option.independentRNG); });

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
    
    public void Activate(List<Effect> effects,bool Independent)
    {
        var sumOfprob=0.0f;
        Deck.Instance.eventVisible = false;
        var v = Random.value;
        foreach(var effect in effects)
        {
            if(effect.possibility == 0)
                continue;


            var p = effect.possibility;

            if(sumOfprob+p >= v && v >= sumOfprob)
            {
                effect.targetEvent.Invoke();
                if(Independent){
                Debug.Log("'" + effect.name +"' takes place cuz a " + p * 100 + "% chance of happening is fulfilled by random value " + v);
            
            }else{
                Debug.Log("'" + effect.name +"' takes place cuz rolled " + v + "is between " + p+sumOfprob+" and "+sumOfprob);
            
            }
            }
            else
            {
                if(!Independent){
                    Debug.Log("'" + effect.name +"'wont take take place cuz rolled " + v + "is not between " + p+sumOfprob+" and "+sumOfprob);
                }
                Debug.Log("'" + effect.name +"' won't take place cuz a " + p * 100 + "% chance of happening is not fulfilled by random value " + v);
            }
            if(!Independent){
                sumOfprob+=effect.possibility;
                if(sumOfprob>1){
                    Debug.Log("Warning sum of propabilities is greater than 1 this might make some events unable to trigger. if this was intended change the mode to IndependentRNG");
                }
            }
        }

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
