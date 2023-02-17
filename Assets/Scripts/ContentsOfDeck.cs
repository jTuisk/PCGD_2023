using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentsOfDeck : MonoBehaviour
{
    public static ContentsOfDeck Instance { get; private set; }

    /*TODO
     *  Later I found out that we also can use Card.Status to get List of cards and if we edit GetListOfCards to it, we can get rid of GetHandsCards function.
     */

    public enum DeckTask { view, selectStartDeck, removeCardFromDeck }

    [SerializeField] DeckTask _currentTask = DeckTask.view;
    [SerializeField] int _maximumNumberOfSelectedCards = 2;

    [SerializeField] GameObject _playerHand;
    [SerializeField] Deck _playerDeck;

    [Header("Game cards")]
    [SerializeField] bool _updateCardsInGame = true;
    [SerializeField] GameObject[] _cardsInGame;
    [SerializeField] List<GameObject> _discardPile;
    [SerializeField] List<GameObject> _battleDeck;
    [SerializeField] List<GameObject> _handCards;

    [SerializeField] List<Card> _selectedCards; 

    [Header("Show deck")] // delete later?
    [SerializeField] bool _showDiscardPile = true;
    [SerializeField] bool _showBattleDeck = true;
    [SerializeField] bool _showHandCards = true;

    [Header("Card positions")]
    [SerializeField] Vector2 _gridStartPosition = Vector2.zero;
    [SerializeField, Range(1, 7)] int _numbersOfCardsInRow = 5;
    [SerializeField] float _rowHeight = 10f;
    [SerializeField] float _columnWidth = 10f;

    [Header("Others")]
    [SerializeField] GameObject enemyGO;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        GetCardsInGame();

        DisplayListOfCards(new List<GameObject>(_cardsInGame));
        //DisplayDiscardedDeck();
        //DisplayBattleDeck();
        //DisplayHandCards();

        HandlePlayerInput();
    }

    private void GetCardsInGame()
    {
        if (!_updateCardsInGame)
            return;

        _cardsInGame = GameObject.FindGameObjectsWithTag("Card");

        _discardPile = GetListOfCards(_playerDeck.BattleDiscardPile);
        _battleDeck = GetListOfCards(_playerDeck.BattleDeck);
        _handCards = GetHandCards();
    }

    private void DisplayDiscardedDeck()
    {
        if (!_showDiscardPile)
            return;

        Debug.Log($"discard deck pile count: {_playerDeck.BattleDiscardPile.Count} -> {_discardPile.Count}"); //Used this to make sure everything is correct.

        if(_discardPile.Count > 0)
        {
            DisplayListOfCards(_discardPile);
        }
    }

    private void DisplayBattleDeck()
    {
        if (!_showBattleDeck)
            return;

        Debug.Log($"battle deck pile count: {_playerDeck.BattleDeck.Count} -> {_battleDeck.Count}"); //Used this to make sure everything is correct.

        if(_battleDeck.Count > 0)
        {
            DisplayListOfCards(_battleDeck);
        }
    }
    
    private void DisplayHandCards()
    {
        if (!_showHandCards)
            return;

        Debug.Log($"hand cards count: {_playerHand.transform.childCount} -> {_handCards.Count}"); //Used this to make sure everything is correct.

        if (_handCards.Count > 0)
        {
            DisplayListOfCards(_handCards);
        }
    }

    private void DisplayListOfCards(List<GameObject> listOfCards)
    {
        float cardZvalue = 0f;

        Vector2 gridStartPosition = _gridStartPosition;

        int totalRows = listOfCards.Count / _numbersOfCardsInRow + 1;

        for (int i = 0; i < totalRows; i++)
        {
            float currentRowPosition = gridStartPosition.y - (i* _rowHeight);

            for (int j = 0; j < _numbersOfCardsInRow; j++)
            {
                int cardIndex = i * _numbersOfCardsInRow + j;

                if (cardIndex >= listOfCards.Count)
                    break;

                GameObject cardGO = listOfCards[cardIndex];
                Vector3 cardFinalPosition = new Vector3(gridStartPosition.x + (j * _columnWidth), currentRowPosition, cardZvalue);
                cardGO.transform.position = cardFinalPosition;
            }
        }
    }

    private List<GameObject> GetHandCards()
    {
        List<GameObject> handCards = new List<GameObject>();

        /**
         * Get all gameobject which has Card component and Card.inHand value is True .
         * Another way to get hand cards is to get childrens from _playerHand gameobject.
         * 
         */
        foreach (GameObject cardGO in _cardsInGame)
        {
            Card card;

            if (cardGO.TryGetComponent<Card>(out card))
            {
                if (card.inHand)
                {
                    handCards.Add(cardGO);
                }
            }
        }

        return handCards;
    }

    private List<GameObject> GetListOfCards(List<Card> compareToList)
    {
        List<GameObject> finalList = new List<GameObject>();

        /**
         *  Get all gameobject which has Card component and are in compareToList.
         */
        foreach (GameObject cardGO in _cardsInGame)
        {
            Card card;
            if (cardGO.TryGetComponent<Card>(out card))
            {
                if (compareToList.Contains(card))
                {
                    finalList.Add(cardGO);
                }
            }
        }
        return finalList;
    }

    private void DisplayEnemyGO(bool disable)
    {
        if(!disable == true) //This is because GameObject.Find does not find disabled gameobjects
            enemyGO = GameObject.Find("CreatureBase(Clone)");

        if (enemyGO != null)
        {
            enemyGO.SetActive(disable);
        }
    }

    private void HandlePlayerInput()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && _currentTask != DeckTask.view)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);

            if (raysastResults.Count > 0)
            {
                if (raysastResults[0].gameObject != null && raysastResults[0].gameObject.transform.root.tag == "Card")
                {
                    GameObject cardGO = raysastResults[0].gameObject.transform.root.gameObject;
                    Card card;

                    if (cardGO.TryGetComponent<Card>(out card))
                    {
                        AddOrRemoveCardFromSelectedList(card);
                    }
                    Debug.Log($"{cardGO.name}");
                }
            }
            /*foreach(RaycastResult result in raysastResults)
            {
                if(result.gameObject != null && result.gameObject.transform.root.tag == "Card")
                {
                    GameObject cardGO = result.gameObject.transform.root.gameObject;
                    Card card;

                    if (cardGO.TryGetComponent<Card>(out card))
                    {
                        AddOrRemoveCardFromSelectedList(card);
                    }
                    Debug.Log($"{cardGO.name}");
                }
            }*/
        }
    }
    public void ChangeCurrentTask(DeckTask newTask = DeckTask.view, int maxSelected = 0)
    {
        _currentTask = newTask;
        //remove outline.
        _selectedCards.Clear();
        _maximumNumberOfSelectedCards = maxSelected;
    }

    public void FinishDeckTask()
    {
        switch (_currentTask)
        {
            case DeckTask.selectStartDeck:
                foreach (Card card in _selectedCards)
                {
                    //Start game with selected cards.
                }
                ChangeCurrentTask();
                gameObject.SetActive(false);
                break;

            case DeckTask.removeCardFromDeck:
                foreach(Card card in _selectedCards)
                {
                    //Remove game object from every list and then destroy gameobject
                }
                ChangeCurrentTask();
                gameObject.SetActive(false);
                break;


            default:
                ChangeCurrentTask();
                gameObject.SetActive(false);
                break;
        }
    }


    private void AddOrRemoveCardFromSelectedList(Card card)
    {
        if (_selectedCards.Contains(card))
        {
            _selectedCards.Remove(card);
            //remove outline 
        }
        else
        {
            if (_selectedCards.Count < _maximumNumberOfSelectedCards)
            {
                _selectedCards.Add(card);
                //Add outline
            }
            else
            {
                Debug.Log($"Too many cards selected! ({_selectedCards.Count}/{_maximumNumberOfSelectedCards})");
                //Display error message.
            }
        }
    }

    private void OnEnable()
    {
        _playerHand.GetComponent<HandOrganizer>().enabled = false;
        DisplayEnemyGO(false);
    }

    private void OnDisable()
    {
        foreach (GameObject cardGO in _discardPile)
        {
            cardGO.transform.position = new Vector2(1000000, 100000);
        }

        foreach(GameObject cardGO in _battleDeck)
        {
            cardGO.transform.position = new Vector2(1000000, 100000);
        }

        /*foreach(GameObject cardGO in _handCards)
        {
            //Move cards back to hand position -> enabling HandOrganizer does it.
        }*/

        _playerHand.GetComponent<HandOrganizer>().enabled = true;
        DisplayEnemyGO(true);
    }
}
