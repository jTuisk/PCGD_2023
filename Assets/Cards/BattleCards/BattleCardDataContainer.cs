using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu (fileName ="New BattleCard", menuName ="Cards/Battle")]
public class BattleCardDataContainer : ScriptableObject
{
    public bool IsEnemySpecialCard;

    public string effectDescriptor = "";
    public string cardName="";
    public int Damage = 0;
    public int block = 0;
    public int magic = 0;
    public int money = 0;
    public int actionCost = 0;
    public UnityEvent effect;
    public List<BattleCardMenuItem> conditionalEffects;    
    public bool exaust=false;

    [Header("Special effect")]
    public bool hasSpecialEffect = false;
    public bool cardsMustBeInHand = false;
    public List<BattleCardDataContainer> requiredCards;
    public BattleCardDataContainer specialEffectEvent;

    public void addCard()
    {
        Deck.Instance.BattleDeckAddCardFromCardData(this);

    }
    public void removeStatusEffects(){
        Deck.Instance.removeAllStatuses();

    }
    public void multiplyHandCardCost(float amount){
        Deck.Instance.multiplyHandCardCost(amount);
    }
    public void multiplyAllCardCost(float amount){
        Deck.Instance.multiplyCardCost(amount);
    }

    public void EnemyDrawCard(){
        Deck.Instance.enemy.Playcard();
    }
    public void putExaustPileBackInDeck(){
        Deck.Instance.putExaustPileBackInDeck();
    }
    public void drawCard(){
        Deck.Instance.DrawCardInHand(1);
    }
    public void drawCard(int amount){
        Deck.Instance.DrawCardInHand(amount);
    }
    public void discardRandom()
    {
        Deck.Instance.discardRandom();

    }
    public void multiplyStatusLength(float amount){
        Deck.Instance.multiplyStatusLength( amount);
    }
    
    public void reverse(){
        Deck.Instance.reversed=true;
    }
    public void removeFromEnemy(){
        Deck.Instance.cardsToRemoveFromEnemies.Add(this);
    }
    public void enemyTakeDamage(){
        int amount=1;
       if(!Deck.Instance.PlayerConfused){
        if(!Deck.Instance.reversed){
            Deck.Instance.enemy.takeDamage(amount);
        }else{
            Deck.Instance.enemy.takeDamage(-amount);
        }
        }else{
            if(Deck.Instance.enemy!=null){
                Deck.Instance.takeDamage(amount);
            }
        }
    }
    public void enemyTakeDamage(int amount){
        if(!Deck.Instance.PlayerConfused){
        if(!Deck.Instance.reversed){
            Deck.Instance.enemy.takeDamage(amount);
        }else{
            Deck.Instance.enemy.takeDamage(-amount);
        }
        }else{
            if(Deck.Instance.enemy!=null){
                Deck.Instance.takeDamage(amount);
            }
        }
    }
    public void playerTakeDamage(){
        int amount=1;        
        if(Deck.Instance.enemy!=null){
        if(!Deck.Instance.enemy.confused){
            
           Deck.Instance.takeDamage(amount);
        }else{
            enemyTakeDamage(amount);
        }
        }
    }
    public void playerTakeDamage(int amount){
        if(Deck.Instance.enemy!=null){
        if(!Deck.Instance.enemy.confused){
            
           Deck.Instance.takeDamage(amount);
        }else{
            enemyTakeDamage(amount);
        }
        }
    }
    public void removeRandomCardFromDeck()
    {
        if (Deck.Instance.BattleDeck.Count > 0)
        {
            var c = Deck.Instance.BattleDeck[UnityEngine.Random.Range(0, Deck.Instance.BattleDeck.Count)];
            Deck.Instance.BattleDeckRemove(c);
            Destroy(c.gameObject);
        }
    }
    public void ConfuseEnemy(){
            if(!Deck.Instance.reversed){
                Deck.Instance.enemy.confused=true;
            }else{
                Deck.Instance.enemy.confused=false;
            }
    }
    public void ConfusePlayer(){
        if(!Deck.Instance.reversed){
            Deck.Instance.PlayerConfused=true;
        }else{
            Deck.Instance.PlayerConfused=false;
        }
    }
    public void AddAP(){
        if(!Deck.Instance.reversed){        
            Deck.Instance.actionPoints+=1;
        }else{
            if(Deck.Instance.actionPoints>0){
                Deck.Instance.actionPoints-=1;
            }
        }
    }
    public void AddAP(int amount){
        if(!Deck.Instance.reversed){        
            Deck.Instance.actionPoints+=amount;
        }else{
            if(Deck.Instance.actionPoints-amount>0){
                Deck.Instance.actionPoints-=amount;
            }else{
                Deck.Instance.actionPoints=0;
            }
        }
    }
    public void enemyHeal(){
        if(!Deck.Instance.reversed){
            Deck.Instance.enemy.HP +=1;
        }else{
            Deck.Instance.enemy.HP -=1;
        }
    }
        public void enemyHeal(int amount){
        if(!Deck.Instance.reversed){
            Deck.Instance.enemy.HP +=amount;
        }else{
            Deck.Instance.enemy.HP -=amount;
        }
    }
    public void shuffleEnemyDeck()
    {
        if (Deck.Instance.enemy != null) {
            while (Deck.Instance.enemy.enemyDeck.transform.childCount > 0)
            {
                Card c = Deck.Instance.enemy.enemyDeck.transform.GetChild(0).GetComponent<Card>();
                c.transform.parent = null;
            }
            Deck.Instance.Shuffle(Deck.Instance.enemy.cards);
            int j = 0;
            foreach (Card card in Deck.Instance.enemy.cards)
            {

                //card.transform.position=new Vector2(enemyDeck.transform.position.x-transform.childCount * 10/2  + j * 10,enemyDeck.transform.position.y);
                card.transform.parent = Deck.Instance.enemy.enemyDeck.transform;

                j++;
            }
            Deck.Instance.enemy.reorganize();
        }
    }

}
