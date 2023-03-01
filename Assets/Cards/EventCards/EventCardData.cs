using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New EventCard", menuName = "Cards/Event/Normal")]
public class EventCardData : ScriptableObject
{
    public string eName;
    //public UnityEvent e;
    [TextArea(3,10)]
    public string eventText;
    //public string eventButtonText;

    public List<EventCardMenuItem> options;

    public BattleCardDataContainer ReplaceFrom;
    public BattleCardDataContainer ReplaceTo;

    public EventCardData replaceEvent;
    public string addedTag;
    public void takeDamage(){
        Deck.Instance.takeDamage(1);
    }
        public void takeDamage(int amount){
        Deck.Instance.takeDamage(amount);
    }
    public void addHealth()
    {
        Deck.Instance.Hp += 1;
        Deck.Instance.takeDamage(0);


    }
        public void addHealth(int amount)
    {
        Deck.Instance.Hp += amount;
        Deck.Instance.takeDamage(0);


    }
        public void addMaxHealth()
    {
        Deck.Instance.MaxHp+=1;

    }
    public void addMaxHealth(int amount)
    {
        Deck.Instance.MaxHp+=amount;

    }
    public void cliffside()
    {
        Deck.Instance.Hp = 1;
    }
    public void HealToFull(){
        Deck.Instance.Hp += 1000000;
        Deck.Instance.takeDamage(0);

    }
    public void Trigger(){
            Instantiate(Deck.Instance.eventBase).GetComponent<EventCard>().CreateEventCard(this);
            Deck.Instance.eventVisible = true;

    }
    public void addMana()
    {
        Deck.Instance.mana += 1;
    }
        public void addMana(int amount)
    {
        Deck.Instance.mana += 1;
    }

    public void replaceCardWrapper(){
        Deck.Instance.ReplaceAllCardsOfType(Deck.Instance.BattleDeck,ReplaceFrom,ReplaceTo);

    }
    public void remove(){
        Deck.Instance.EventDeck.Remove(this);
    }
    public void add(){
        Deck.Instance.EventDeck.Add(this);
    }
    public void addTag(){
        Deck.Instance.flags.Add(addedTag);

    }
    public void WaterEncounter(){
            Deck.Instance.mana = 0;
    }
    public void StartSelectDeck(int n){
        ContentsOfDeck.Instance.DisplayCards(ContentsOfDeck.DeckTask.selectStartDeck, (uint) n);
    }

    public void RemoveCard(int n){
        ContentsOfDeck.Instance.DisplayCards(ContentsOfDeck.DeckTask.removeCardFromDeck, (uint) n);
    }

    public void AmateurEncounter(){
        Deck.Instance.Hp = 10;
    }
    public void replaceWithThis(){
        
        for(int i=0; i< Deck.Instance.EventDeck.Count; i++){
            if(Deck.Instance.EventDeck[i].name==replaceEvent.name){
                Deck.Instance.EventDeck[i]=this;
            }
        }
    }
}
