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
    public GameObject DamageText;
    public List<Card> cards;
    public TMPro.TextMeshProUGUI text; 

    public GameObject cardPrefab;
    public GameObject enemyDeck;
    public GameObject sprite;
    public EventCardData postBattleEvent;
    public bool confused=false;
    public bool stunned = false;
    public float EnemyDamageModifier = 1;
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


IEnumerator shake(){
    Vector3 startPos=sprite.transform.position;
    Vector3 startScale=sprite.transform.localScale;
    float speed=20;
    for(int i=0;i<speed;i++){
        sprite.transform.position=startPos-new Vector3(0,Mathf.Lerp(0,5,(float)((i*1.0)/speed)),0);
        sprite.transform.localScale=startScale-new Vector3(0,Mathf.Lerp(0,0.2f,(float)((i*1.0)/speed)),0);
        yield return null;
    }
    for(int i=0;i<speed;i++){
        sprite.transform.position=startPos-new Vector3(0,Mathf.Lerp(0,5,1.0f-((float)(i*1.0/speed))),0);
        sprite.transform.localScale=startScale-new Vector3(0,Mathf.Lerp(0,0.2f,1.0f-((float)(i*1.0/speed))),0);
        yield return null;
    }
}
    public void takeDamage(int amount) {
        if(amount>0){
            StartCoroutine("shake");
            Instantiate(DamageText).GetComponent<DamageText>().changeTextString(""+amount*EnemyDamageModifier);
        }
        HP = (int) (HP - amount * EnemyDamageModifier);
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
        if (stunned) {
            stunned = false;
            return;
        }
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
            var temp=enemyDeck.transform.GetChild(0).GetComponent<Card>();
            
            enemyDeck.transform.GetChild(0).transform.position=new Vector2(10000,100000);
            enemyDeck.transform.GetChild(0).transform.parent=this.transform;
            temp.EnemyPlayCard(this);
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
