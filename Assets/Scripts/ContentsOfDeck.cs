using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsOfDeck : MonoBehaviour
{
    public static ContentsOfDeck Instance { get; private set; }
    /*TODO
     *  Later I found out that we also can use Card.Status to get List of cards and if we edit GetListOfCards to it, we can get rid of GetHandsCards function.
     */

    [SerializeField] GameObject _playerHand;
    [SerializeField] Deck _playerDeck;


    [Header("Game cards")]
    [SerializeField] bool _updateCardsInGame = true;
    [SerializeField] GameObject[] _cardsInGame;
    [SerializeField] List<GameObject> _discardPile;
    [SerializeField] List<GameObject> _battleDeck;
    [SerializeField] List<GameObject> _handCards;

    [Header("Card positions")]
    /*[SerializeField] Transform _discardPileParent;
    [SerializeField] Transform _battleDeckParent;
    [SerializeField] Transform _handCardsParent;*/
    [SerializeField] Vector2 _discardPilePosition;
    [SerializeField] Vector2 _battleDeckPosition;
    [SerializeField] Vector2 _handCardsPosition;

    [Header("Others")]
    [SerializeField] bool _customOverlay = true;
    [SerializeField] Color _overlayColor = Color.blue;

    [SerializeField] List<Card> _selectedCards; // to private later

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

        DisplayDiscardedDeck();
        DisplayBattleDeck();
        DisplayHandCards();
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
        Debug.Log($"discard deck pile count: {_playerDeck.BattleDiscardPile.Count} -> {_discardPile.Count}"); //Used this to make sure everything is correct.

        if(_discardPile.Count > 0)
        {
            DisplayListOfCards(_discardPile, _discardPilePosition);
        }
    }

    private void DisplayBattleDeck()
    {
        Debug.Log($"battle deck pile count: {_playerDeck.BattleDeck.Count} -> {_battleDeck.Count}"); //Used this to make sure everything is correct.

        if(_battleDeck.Count > 0)
        {
            DisplayListOfCards(_battleDeck, _battleDeckPosition);
        }
    }
    
    private void DisplayHandCards()
    {
        Debug.Log($"hand cards count: {_playerHand.transform.childCount} -> {_handCards.Count}"); //Used this to make sure everything is correct.

        if (_handCards.Count > 0)
        {
            DisplayListOfCards(_handCards, _handCardsPosition);
        }
    }

    private void DisplayListOfCards(List<GameObject> listOfCards, Vector2 parent)
    {
        float cardDistanceScalar = 0.5f;
        float cardZvalue = 0f;

        for (int i = 0; i < listOfCards.Count; i++)
        {
            GameObject cardGO = listOfCards[i];
            cardGO.transform.position = CalculateCardPosition(parent, listOfCards.Count, i, cardGO.transform.localScale.x, cardDistanceScalar, cardZvalue);
        }

    }
    /**
     *  edited from HandOrganizer.CalculateCardPos(cardCount, cardIndex, cardScale)
     */
    private Vector3 CalculateCardPosition(Vector2 pos, int cardCount, int cardIndex, float cardScale, float distanceScalar, float z)
    {
        float x = -cardCount * distanceScalar * 10f / 2f + distanceScalar * cardIndex * 10f + pos.x; // need to do something with this, Maybe position card in multiple line?
        float y = pos.y;

        return new Vector3(x, y, z);
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

    private void AddOutLineToSelectedCards()
    {
        if (!_customOverlay)
            return;

        foreach(Card card in _selectedCards)
        {
            
        }
    }


    private void DisableEnemyGO(bool disable)
    {
        if(disable == true) //This is because GameObject.Find does not find disabled gameobjects
            enemyGO = GameObject.Find("CreatureBase(Clone)");

        if (enemyGO != null)
        {
            enemyGO.SetActive(!disable);
        }
    }

    private void OnEnable()
    {
        _playerHand.GetComponent<HandOrganizer>().enabled = false;
        DisableEnemyGO(true);
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

        foreach(GameObject cardGO in _handCards)
        {
            //Move cards back to hand position
        }

        _playerHand.GetComponent<HandOrganizer>().enabled = true;
        DisableEnemyGO(false);

    }
}
