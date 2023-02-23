using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;
public class EnemyCard : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP = 10;
    public int MaxDamageRange = 6;
    public int block=0;
    public int MinDamageRange = 1;
    public int damage = 0;
    public GameObject DamageText;
    public List<Card> cards;
    public TMPro.TextMeshProUGUI text; 

    public TMPro.TextMeshProUGUI DeckSize; 

    public GameObject cardPrefab;
    public GameObject enemyDeck;
    public GameObject sprite;
    public EventCardData postBattleEvent;

    public GameObject combatReward;

    [HideInInspector]
    public CreatureDataContainer creatureDataContainer;

    public bool confused=false;
    public bool stunned = false;
    public float EnemyDamageModifier = 1;
    public Image img;
    int decks;
    void Start()
    {

        img=sprite.GetComponent<Image>();
        Debug.Log(img);
        Deck.Instance.battleStart();
    }
public void reorganize(){
    for(int j=0; j<enemyDeck.transform.childCount; j++){
    enemyDeck.transform.GetChild(j).GetComponent<Card>().status=Card.BelongTo.Enermy;
    if(j==0){
        enemyDeck.transform.GetChild(j).transform.position=new Vector2(enemyDeck.transform.position.x  + (j-1) * 5,enemyDeck.transform.position.y);
        enemyDeck.transform.GetChild(j).transform.localScale=new Vector3(0.6f,0.6f,0.52f);
    }else{
    enemyDeck.transform.GetChild(j).transform.localScale=new Vector3(0.5f,0.5f,0.5f);
    enemyDeck.transform.GetChild(j).transform.position=new Vector2(enemyDeck.transform.position.x  + j * 5,enemyDeck.transform.position.y);
    }}
    DeckSize.text=""+(decks-enemyDeck.transform.childCount);
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
            Instantiate(DamageText).GetComponent<DamageText>().changeTextString(""+(amount*EnemyDamageModifier-block));
        }
        if(!Deck.Instance.reversed){
        HP = Mathf.Min((int) (HP +block- amount * EnemyDamageModifier),HP);
        }else{
            HP = (int) (HP + amount * EnemyDamageModifier);
        }
    }
    public void Create(CreatureDataContainer data)
    {
        creatureDataContainer = data;
        // check if the battle card is banned by a previous battle reward
        int affectedBattleCardIndex = (Deck.Instance.enemyAffectedByCombatRewards.ContainsKey(data))?
            Deck.Instance.enemyAffectedByCombatRewards[data] : -1;

        HP = data.HP;
        MaxDamageRange = data.MaxDamageRange;
        MinDamageRange = data.MinDamageRange;
        postBattleEvent = data.postBattleEvent;
        int j=0;
        if(data.Picture!=null){
            img.sprite=data.Picture;
        }
        foreach(BattleCardDataContainer i in data.deck){
            if(!( Deck.Instance.cardsToRemoveFromEnemies.Contains(i)) ){
            
            // Skip instantiating if the card is banned
            if(affectedBattleCardIndex == j)
            {
                Deck.Instance.enemyAffectedByCombatRewards.Remove(data);
                j++;
                continue;
            }

            var card=Instantiate(cardPrefab).GetComponent<Intent>();
            card.createCard(i);
            var scale = CardHandler.Instance.cardScaleInEnermyDeck;
            card.transform.localScale=new Vector3(scale, scale, scale);
            cards.Add(card);
            //card.transform.position=new Vector2(enemyDeck.transform.position.x-transform.childCount * 10/2  + j * 10,enemyDeck.transform.position.y);
            card.transform.parent=enemyDeck.transform;
            card.status = Card.BelongTo.Enermy;
            
            j++;
        }
        }
        
        decks=enemyDeck.transform.childCount;
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
            if(temp.exaust){
                cards.Remove(temp);
                Destroy(temp.gameObject);
            }
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

            if(combatReward != null)
            {
                var combatRewardObj = Instantiate(combatReward);
                combatRewardObj.GetComponent<CombatReward>().OnCreate(creatureDataContainer);

                Deck.Instance.inReward = true;
                Deck.Instance.eventVisible = true;
            }
            else if (postBattleEvent != null)
            {
                Instantiate(Deck.Instance.eventBase).GetComponent<EventCard>().CreateEventCard(postBattleEvent);
                Deck.Instance.eventVisible = true;
            }
            Destroy(gameObject);
        }
        
    }
}
