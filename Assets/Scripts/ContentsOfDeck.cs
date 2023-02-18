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
    [SerializeField] uint _maximumNumberOfSelectedCards = 2;

    [SerializeField] GameObject _playerHand;
    [SerializeField] Deck _playerDeck;

    [Header("Game cards")]
    [SerializeField] bool _updateCardsInGame = true;
    [SerializeField] List<GameObject> _cardsInGame;
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

    [Header("Buttons")]
    [SerializeField] GameObject _closeButton;
    [SerializeField] GameObject _actionButton;

    [Header("Others")]
    [SerializeField] GameObject enemyGO;
    [SerializeField] Color HighlightColor = Color.red;
    [SerializeField] float timer = 1f;

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
        DisplayCards(); //Remove this line after testing

        HandlePlayerInput();
        UpdateUIText();
        HandleActionButton();
    }

    private void GetCardsInGame()
    {
        if (!_updateCardsInGame)
            return;

        _cardsInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("Card"));

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
                Debug.Log($"selected root tag: {raysastResults[0].gameObject.transform.root.tag}"); // Current

                //Currently you cannot select cards that are in your hand. That's because the root tag is no longer "Card". Hand card root tag is = "Untagged")
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

    public void FinishDeckTask()
    {
        Debug.Log("FinishTask: " + _currentTask);
        switch (_currentTask)
        {
            case DeckTask.selectStartDeck:

                if (_selectedCards.Count == _maximumNumberOfSelectedCards)
                {
                    GetCardsInGame();
                    //Removes every card that is not selected.
                    foreach (Card card in _selectedCards)
                    {
                        GameObject cardGO = card.gameObject;

                        if (!_cardsInGame.Contains(cardGO))
                        {
                            _playerDeck.RemoveAndDestroyCard(card);
                        }

                    //Start game with selected cards.
                    }
                    ChangeCurrentTask();
                    gameObject.SetActive(false);
                 }
                break;

            case DeckTask.removeCardFromDeck:
                if(_selectedCards.Count == _maximumNumberOfSelectedCards)
                {
                    foreach (Card card in _selectedCards)
                    {
                        //_cardsInGame.Remove(cardGO); 
                        //_discardPile.Remove(cardGO);
                        //_battleDeck.Remove(cardGO);
                        //_handCards.Remove(cardGO);

                        _playerDeck.RemoveAndDestroyCard(card);
                    }
                    ChangeCurrentTask(_currentTask, _maximumNumberOfSelectedCards);
                    gameObject.SetActive(false);
                }
                break;

            default:
                ChangeCurrentTask();
                gameObject.SetActive(false);
                break;
        }
    }
    public void ChangeCurrentTask(DeckTask newTask = DeckTask.view, uint maxSelected = 0)
    {
        _currentTask = newTask;
        RemoveSelectedCardsHighlight();
        _selectedCards.Clear();
        _maximumNumberOfSelectedCards = maxSelected;
    }

    private void AddOrRemoveCardFromSelectedList(Card card)
    {
        if (_selectedCards.Contains(card))
        {
            _selectedCards.Remove(card);
            card.SetHighlightAlpha(0f);
        }
        else
        {
            if (_selectedCards.Count < _maximumNumberOfSelectedCards)
            {
                _selectedCards.Add(card);
                card.SetHighlightColor(HighlightColor);
                card.SetHighlightAlpha(1f);
            }
            else
            {
                Debug.Log($"Too many cards selected! ({_selectedCards.Count}/{_maximumNumberOfSelectedCards})");
                //Display error message.
            }
        }
    }

    private void RemoveSelectedCardsHighlight()
    {
        foreach(Card card in _selectedCards)
        {
            card.SetHighlightAlpha(0f);
        }
    }

    public void DisplayCards()
    {
        DisplayCards(_currentTask, _maximumNumberOfSelectedCards);
    }

    public void DisplayCards(DeckTask deckTask = DeckTask.view, uint maxSelected = 0)
    {
        GetCardsInGame();
        DisplayCards(_cardsInGame, deckTask, maxSelected);
    }

    public void DisplayCards(List<GameObject> cards, DeckTask deckTask = DeckTask.view, uint maxSelected = 0)
    {
        gameObject.SetActive(true);
        _currentTask = deckTask;
        _maximumNumberOfSelectedCards = maxSelected;
        HandleButtonUI();
        DisplayListOfCards(cards);
    }

    private void HandleButtonUI()
    {
        switch (_currentTask)
        {
            case DeckTask.selectStartDeck:
            case DeckTask.removeCardFromDeck:
                _closeButton.SetActive(false);
                _actionButton.SetActive(true);
                break;

            default:
                _closeButton.SetActive(true);
                _actionButton.SetActive(false);
                break;
        }
    }

    private void HandleActionButton()
    {
        if (_actionButton.activeSelf)
        {
            if(_currentTask != DeckTask.view)
            {
                Button button = _actionButton.GetComponentInChildren<Button>();
                    
                if(button != null)
                {
                    button.interactable = _selectedCards.Count == _maximumNumberOfSelectedCards;
                }
            }
        }
    }

    private void UpdateUIText()
    {
        //Task
        //selected
    }

    private void OnEnable()
    {
        _playerHand.GetComponent<HandOrganizer>().enabled = false;
        DisplayEnemyGO(false);
    }

    private void OnDisable()
    {
        RemoveSelectedCardsHighlight();
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
        HandOrganizer ho = _playerHand.GetComponent<HandOrganizer>();
        ho.SetWaitTime(timer);
        ho.enabled = true;
        DisplayEnemyGO(true);
    }
}
