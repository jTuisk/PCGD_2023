using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// general handler for cards' animation and display
public class CardHandler : MonoBehaviour
{
    public static CardHandler Instance { get; private set; }
    private static int UILayer;
    public string CardTag = "Card";
    private Card currentFocusedCard; // Hovered by mouse
    private GameObject currentFocusedCardObj;
    public float cardScaleInEnermyDeck = 0.3f;

    public float cardScaleInEnermyDeckWhenHovering = 1.5f;
    public float cardScaleInPlayerHandWhenHovering = 1.2f;

    public float cardOriginalScale {get; private set;} = 1.0f;

    void Awake()
    {
        //singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        cardOriginalScale = Deck.Instance.CardBasePrefab.transform.localScale.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsPointerOverCard())
            return;

        //DisplayCardsWhenMouseHovered(GetCardsUnderMousePointer(GetEventSystemRaycastResults()));
        DisplayCurrentFocusedCard();
    }

    private bool IsPointerOverCard()
    {
        // if the current mouse position is not on top of any gameObjects
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        return IsPointerOverCard(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverCard(List<RaycastResult> raycasts)
    {
        if(raycasts.Count <= 0)
            return false;

        //Now, only check the first element hit by ray, modify here when needed
        RaycastResult topRaycastResult = raycasts[0];
        return IsPointerOverCard(topRaycastResult.gameObject);
    }

    // The Raycast does not hit Canvas (why?), thus backtrack parent hierarchy is necessary
    private bool IsPointerOverCard(GameObject obj)
    {
        // Elements inside Card all belong to UILayer
        if (obj.layer != UILayer)
            return false;

        if(obj.CompareTag(CardTag))
            return true;
        if(FindParentWithTag(obj, CardTag))
            return true;
        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        
        return raysastResults;
    }

    // return the GameObject with certain tag in obj's parental hierarchy
    GameObject FindParentWithTag(GameObject obj, string tag)
    {
        // return null if current object has no parent
        if(!obj.transform.parent)
            return null;
        
        GameObject parent = obj.transform.parent.gameObject;
        if(parent.CompareTag(tag))
        {
            return parent;
        }
        return FindParentWithTag(parent, tag);
    }

    List<Card> GetCardsUnderMousePointer(List<RaycastResult> raycasts)
    {
        List<Card> cards = new List<Card>();
        for (int index = 0; index < raycasts.Count; index++)
        {
            GameObject cardObj = FindParentWithTag(raycasts[index].gameObject, CardTag);
            if(cardObj)
            {
                cards.Add(cardObj.GetComponent<Card>());
            }
        }
        return cards;
    }

    void DisplayCardsWhenMouseHovered(List<Card> cards)
    {
        for(int index = 0; index < cards.Count; index ++)
        {
        }
    }
    public GameObject IntentTipPanel;
    public void SetCurrentFocusedCard(Card card)
    {
        if(!card)
            return;

        currentFocusedCard = card;

        // currentFocusedCardObj is now only used to store card prefab instance
        if(card && card.status == Card.BelongTo.Enermy)
        {
            currentFocusedCardObj = Instantiate(IntentTipPanel);
            currentFocusedCardObj.GetComponent<IntentLarge>().createCard(card.BattleCardData);
            currentFocusedCardObj.name = "tipPanel";
            //Destroy(currentFocusedCardObj.gameObject.GetComponent<Card>());
            var scale = CardHandler.Instance.cardScaleInEnermyDeckWhenHovering;
            currentFocusedCardObj.transform.localScale = new Vector3(scale, scale, scale);
            currentFocusedCardObj.GetComponentInChildren<Canvas>().sortingOrder = 20;
        }
    }

    public void RemoveCurrentFocusedCard()
    {
        currentFocusedCard = null;
        //Destroy current gameObject
        Destroy(currentFocusedCardObj);
        currentFocusedCardObj = null;
    }

    void DisplayCurrentFocusedCard()
    {
        if(!currentFocusedCard)
            return;

        switch (currentFocusedCard.status)
        {
            case Card.BelongTo.Enermy:
            {
                if(currentFocusedCardObj)
                {
                    // display a tip panel
                    var campos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    var mousepos = new Vector3(campos.x + 9f, campos.y - 9f, 0);
                    currentFocusedCardObj.transform.position = mousepos;                    
                }
                break;
            }
            case Card.BelongTo.PlayerHand:
            {
                break;
            }
            default: break;
        }
    }
}
