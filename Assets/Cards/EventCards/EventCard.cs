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
    public GameObject CreatureBase;
    public CreatureDataContainer Creature;
    public GameObject eventCardFront;
    public GameObject eventCardBack;
    public GameObject deckAnchor;

    #region Animation Parameters
    private Vector3 slideStartRotation = new Vector3(0f, 0f, 18.45f);
    private Vector3 slideEndRotation = Vector3.zero;
    private float cardMinimalSize = 0.15f;
    private float cardNormalSize = 1.0f;
    private Vector3 flipUpAngle = new Vector3(90f, 0f, 0f);
    private Vector3 flipFlatAngle = Vector3.zero;

    private float slideAnimDuration = 0.5f;
    private float flipAnimDuration = 0.5f;
    #endregion

    private List<BattleCardTipTool> cardTipTools = new List<BattleCardTipTool>();


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
        foreach (EventCardMenuItem option in options) 
        {
            bool visible = true;
            foreach (Condition con in option.conditions) 
            {
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

                CreateCardTipTools(option, button);
            }
        }
        if (Menu.transform.childCount == 0) {
            var button = Instantiate(ButtonPrefab);
            button.GetComponent<Button>().onClick.AddListener(delegate { Activate(); });
            button.transform.parent = Menu.transform;
        }
    }

    private void CreateCardTipTools(EventCardMenuItem option, GameObject button)
    {
        foreach(var effect in option.effects)
        {
            // Check if registered event contains AddBattleCard function
            var eventCount = effect.targetEvent.GetPersistentEventCount();
            for(int i = 0; i < eventCount; i++)
            {
                var funcName = effect.targetEvent.GetPersistentMethodName(i);
                var tarObj = effect.targetEvent.GetPersistentTarget(i);

                string addBattleCardClassName = "BattleCardDataContainer";
                string addBattleCardFuncName = "addCard";
                if(tarObj.ToString().Contains(addBattleCardClassName) && funcName == addBattleCardFuncName)
                {
                    BattleCardDataContainer battleCardDataContainer = null;

                    // Get corresponding ScriptableObject
                    foreach(var battleCard in Deck.Instance.cardPrefabs)
                    {
                        if(tarObj.ToString().Contains(battleCard.name))
                        {
                            battleCardDataContainer = battleCard;
                            break;
                        }
                    }
                    if(battleCardDataContainer != null)
                    {
                        var cardTipTool = button.AddComponent<BattleCardTipTool>();
                        cardTipTool.battleCardData = battleCardDataContainer;
                        cardTipTool.canShow = false;
                        cardTipTools.Add(cardTipTool);
                    }
                }
            }
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

    void Start()
    {
        CardBornFromDeck();

        // Set correct Camera so that it won't cover other components
        var canvas = this.GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;

        var canvasScaler = canvas.gameObject.GetComponent<CanvasScaler>();
        canvasScaler.scaleFactor = 2.0f;
    }


    #region Animation
    
    private void CardBornFromDeck()
    {
        AudioManager.Instance.PlayEventCardDrawSound();

        // Calculate the deck_image's local position under cardBack's parent
        // First, get deck_image localPosition under its parent Canvas
        Vector2 deckImageLocalPos;
        var eventDeckImage = GameObject.Find("Canvas/EventDeck/EventDeckImage");
        if(eventDeckImage != null)
        {
            deckImageLocalPos = eventDeckImage.transform.localPosition;
        }
        else
        {
            deckImageLocalPos = deckAnchor.transform.position;
        }
        /*
        // Second, covert localPosition to worldPosition according to EventCard's canvas
        // Which is different from deck_image's world position
        var canvasObj = this.gameObject.transform.GetComponentInChildren<Canvas>().gameObject;
        var deckImageWorldPos = canvasObj.transform.TransformPoint(deckImageLocalPos);
        
        // Finally, convert worldPosition to localPosition according to cardBack's parent Rect
        Vector2 deckRelativeLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            eventCardBack.transform.parent.GetComponent<RectTransform>(),
            Camera.main.WorldToScreenPoint(deckImageWorldPos),
            Camera.main,
            out deckRelativeLocalPos
        );
        */
        eventCardBack.transform.position = eventDeckImage.transform.position;
        eventCardBack.transform.localScale = new Vector3(cardMinimalSize, cardMinimalSize, cardMinimalSize);
        eventCardBack.transform.eulerAngles = slideStartRotation;
        
        eventCardFront.SetActive(false);
        eventCardBack.SetActive(true);

        StartCoroutine("CardSlideToCenter");
    }

    IEnumerator CardSlideToCenter() 
    {
        //play audio sound
        AudioManager.Instance.PlayEventCardSlideSound();

        var slideStartTime = Time.time;
        var slideStartPos = eventCardBack.transform.position;

        do {
            eventCardBack.transform.position = Vector3.Lerp(
                slideStartPos, 
                eventCardFront.transform.position,
                (Time.time - slideStartTime)/slideAnimDuration);

            eventCardBack.transform.eulerAngles = Vector3.Lerp(
                slideStartRotation, slideEndRotation,
                (Time.time - slideStartTime)/slideAnimDuration);

            yield return new WaitForEndOfFrame();
        } while ((Time.time - slideStartTime) <= slideAnimDuration);
        
        StartCoroutine("CardFlip");
    }


    IEnumerator CardFlip()
    {
        yield return new WaitForSeconds(0.3f);

        //play audio sound
        AudioManager.Instance.PlayEventCardFlipSound();

        var flipStartTime = Time.time;

        Vector3 startScale = new Vector3(cardMinimalSize, cardMinimalSize, cardMinimalSize);
        Vector3 endScale = new Vector3(cardNormalSize, cardNormalSize, cardNormalSize);
        eventCardFront.transform.localScale = startScale;
        eventCardFront.transform.eulerAngles = flipUpAngle;
        eventCardFront.SetActive(false);

        do {
            eventCardBack.transform.localScale = Vector3.Lerp(
                startScale, endScale,
                (Time.time - flipStartTime)/flipAnimDuration);
            eventCardFront.transform.localScale = Vector3.Lerp(
                startScale, endScale,
                (Time.time - flipStartTime)/flipAnimDuration);

            if((Time.time - flipStartTime) < (flipAnimDuration*0.5f))
            {
                eventCardBack.transform.eulerAngles = Vector3.Lerp(
                flipFlatAngle, flipUpAngle,
                (Time.time - flipStartTime)/(flipAnimDuration * 0.5f));
            }
            else
            {
                eventCardFront.SetActive(true);
                eventCardBack.SetActive(false);
                eventCardFront.transform.eulerAngles = Vector3.Lerp(
                    flipUpAngle, flipFlatAngle,
                    (Time.time - flipStartTime - flipAnimDuration * 0.5f)/(flipAnimDuration * 0.5f));
            }
            yield return null;

            if((Time.time - flipStartTime) >= flipAnimDuration)
            {
                // Make sure interpolation outcome
                eventCardFront.transform.localScale = endScale;
                eventCardFront.transform.eulerAngles = flipFlatAngle;
                foreach(var cardTipTool in cardTipTools)
                {
                    cardTipTool.canShow = true;
                }
            }
        } while ((Time.time - flipStartTime) <= flipAnimDuration);


    }

    #endregion

}
