using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;
public class EnemyCard : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP = 10;
    public int MaxDamageRange = 6;
    public int MinDamageRange = 1;
    public int damage = 0;

    public List<Card> cards;
    public TMPro.TextMeshProUGUI text; 

    public GameObject cardPrefab;
    public GameObject enemyDeck;
    public EventCardData postBattleEvent;
    public bool confused=false;
    void Start()
    {
        Deck.Instance.battleStart();
    }
public void reorganize(){
    for(int j=0; j<enemyDeck.transform.childCount; j++){
    enemyDeck.transform.GetChild(j).GetComponent<Card>().status=Card.BelongTo.Enermy;
    enemyDeck.transform.GetChild(j).transform.position=new Vector2(enemyDeck.transform.position.x  + j * 2,enemyDeck.transform.position.y);
    }
}
    public void Create(CreatureDataContainer data)
    {
        HP = data.HP;
        MaxDamageRange = data.MaxDamageRange;
        MinDamageRange = data.MinDamageRange;
        postBattleEvent = data.postBattleEvent;
        int j=0;
        foreach(BattleCardDataContainer i in data.deck){
            var card=Instantiate(cardPrefab).GetComponent<Card>();
            card.createCard(i);
            var scale = CardHandler.Instance.cardScaleInEnermyDeck;
            card.transform.localScale=new Vector3(scale, scale, scale);
            cards.Add(card);
            //card.transform.position=new Vector2(enemyDeck.transform.position.x-transform.childCount * 10/2  + j * 10,enemyDeck.transform.position.y);
            card.transform.parent=enemyDeck.transform;
            card.status = Card.BelongTo.Enermy;
           
            j++;
        }
        reorganize();
        
        Deck.Instance.Shuffle(cards);
        
    }
    public void Playcard(){
        if(enemyDeck.transform.childCount==0){
            Deck.Instance.Shuffle(cards);
            int j=0;
            foreach(Card card in cards){
                
                //card.transform.position=new Vector2(enemyDeck.transform.position.x-transform.childCount * 10/2  + j * 10,enemyDeck.transform.position.y);
                card.transform.parent=enemyDeck.transform;
                
                j++;
            }
            reorganize();
        }else{
            enemyDeck.transform.GetChild(0).GetComponent<Card>().EnemyPlayCard(this);
            enemyDeck.transform.GetChild(0).transform.position=new Vector2(10000,100000);
            enemyDeck.transform.GetChild(0).transform.parent=this.transform;
            reorganize();

            
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Cange gamestate back to drawing event cards if player wins battle
        text.text = damage + "\n" + HP;
        if (HP <= 0)
        {
            Deck.Instance.enemy = null;
            Deck.Instance.inBattle = false;
            Deck.Instance.ResetDeck();
            if (postBattleEvent != null)
            {
                Instantiate(Deck.Instance.eventBase).GetComponent<EventCard>().CreateEventCard(postBattleEvent);
                Deck.Instance.eventVisible = true;
            }
            Destroy(gameObject);
        }
        
    }
}
