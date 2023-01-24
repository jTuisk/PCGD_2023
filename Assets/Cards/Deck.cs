using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }
    public List<Card> BattleDeck;
    public List<Card> BattleDiscardPile;
    public List<EventCardData> EventDeck;
    public GameObject CardBasePrefab;
    public List<BattleCardDataContainer> cardPrefabs;
    public List<int> deckList;
    public List<string> flags;
    public int money = 0;
    public int mana = 0;
    private int bossCounter=0;
    public int Hp = 0;
    public int block = 0;
    public int MaxactionPoints = 3;
    public int actionPoints = 3;
    public bool inBattle=false;
    public bool eventVisible = false;
    public EnemyCard enemy;
    public GameObject Hand;
    public GameObject eventBase;
    public List<EventCardData> BossBattles=new List<EventCardData>();
    
    public int CardsDrawnAtStartOfTurn=5;
    private void Awake()
    {
        //singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    public void DrawEventCard()
    {
        
        if(bossCounter<=20){
            Instantiate(eventBase).GetComponent<EventCard>().CreateEventCard(EventDeck[Random.Range(0, EventDeck.Count - 1)]);
            //the below line is only for testing
            //Instantiate(eventBase).GetComponent<EventCard>().CreateEventCard(EventDeck[5]); 

        }else{
            Instantiate(eventBase).GetComponent<EventCard>().CreateEventCard(BossBattles[Random.Range(0, BossBattles.Count)]);
            bossCounter=0;
        }
        bossCounter+=1;
        eventVisible = true;

    }
public void takeDamage(int amount){

            Hp -= Mathf.Max(0,amount-block);
            if(Hp<=0){
                SceneLoader.LoadGameOver();
            }            
}
    public void inBattleEndTurn()
    {
        PutHandInDiscardPile();
        //run at the end of the turn
        if (enemy != null)
        {
            if(enemy.cards.Count!=0){
                enemy.Playcard();
                
            }else{
            takeDamage(enemy.damage);
            enemy.damage = Random.Range(enemy.MinDamageRange, enemy.MaxDamageRange);
            
            }
            inBattleStartTurn();
            Debug.Log("HP: " + Hp);
            
        }
        block = 0;
        
    }
    public void PutHandInDiscardPile(){
        while(Hand.transform.childCount>0){
            discard(0);
        }

    }
    public void discard(int CardIndex){
        if(Hand.transform.childCount>0){
            BattleDiscardPile.Add(Hand.transform.GetChild(CardIndex).GetComponent<Card>());
            Hand.transform.GetChild(CardIndex).position=new Vector2(10000000,100000000);
            Hand.transform.GetChild(CardIndex).transform.parent=null;
        }
    }
    public void discardRandom(){
        
        discard(Random.Range(0,Hand.transform.childCount));
    }
    public void Shuffle<T>(List<T> list) {
        for(int i=0; i < list.Count; i++)
        {
            swap(i, Random.Range(0, list.Count), list);

        }
    
    }
    public void swap<T>(int a,int b ,List<T> list)
    {
        var temp = list[a];
        list[a] = list[b];
        list[b] = temp;
    }
    public void shuffleDiscardPileBackInDeck()
    {


            foreach (var i in BattleDiscardPile)
            {
                BattleDeck.Add(i);
            }
            Shuffle(BattleDeck);
            Debug.Log("Deck shuffled");
            BattleDiscardPile = new List<Card>();

    }
    public Card DrawCard()
    {
        //pops card from deck
        if (BattleDeck.Count == 0)
        {
            shuffleDiscardPileBackInDeck();
        }
            var temp = BattleDeck[0];
            BattleDeck.Remove(temp);
        
        return temp;
    }
    public void DrawCardInHand(int amount)
    {
        //draws specified amount of cards to hand
        for(int i=0; i<amount; i++)
        {
            if (BattleDeck.Count > 0||BattleDiscardPile.Count>0)
            {
                //DrawCard().gameObject.transform.parent = Hand.transform;

                var card = DrawCard();
                
                var deckPos = Deck.Instance.getPosition();
                card.transform.position = deckPos;
                card.gameObject.transform.parent = Hand.transform;
                
                var startPos = new Vector3(deckPos.x, deckPos.y, transform.position.z);
                var endPos = new Vector3(-amount * 10/2  + i * 10, -10, 0);
                card.triggerMove(startPos, endPos);
            }

        }

    }
    public void inBattleStartTurn() {
        //run at the start of the turn
        DrawCardInHand(CardsDrawnAtStartOfTurn);
        actionPoints = MaxactionPoints;

    }
    void Start()
    {
        int index = 0;
        //inits all cards specified by their index
        foreach(var i in deckList)
        {
            var j = Instantiate(CardBasePrefab).GetComponent<Card>();
            j.name += index; index++;
            j.createCard(cardPrefabs[i]);
            j.transform.position =new Vector2(1000000,100000);
            BattleDeck.Add(j);

        }
    }
    public void ResetDeck()
    {
        //moves all cards back to deck 
        shuffleDiscardPileBackInDeck();
        while (Hand.transform.childCount > 0)
        {
            foreach (Transform i in Hand.transform)
            {
                i.parent = null;
                BattleDeck.Add(i.gameObject.GetComponent<Card>());
                i.position = new Vector2(1000000, 100000);
                continue;
            }
        }
        actionPoints = MaxactionPoints;
    }
    public void battleStart()
    {
        //run when battle starts
        Shuffle(BattleDeck);
       DrawCardInHand(CardsDrawnAtStartOfTurn);
    }
    public void BattleDeckAddCard(int index) {
        BattleDeckAddCardFromCardData(cardPrefabs[index]);
    }
    public void BattleDeckAddCardFromCardData(BattleCardDataContainer cardData) {
        var j = Instantiate(CardBasePrefab).GetComponent<Card>();
        j.createCard(cardData);
        j.transform.position = new Vector2(1000000, 100000);
        BattleDeck.Add(j);
    }
    
    // Return the deck position under world coordination
    public Vector3 getPosition()
    {
        var camPos = Camera.main.ScreenToWorldPoint(this.transform.position);
        return new Vector3(camPos.x, camPos.y, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
