using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class DayResource
{
    public List<EventCardData> EventPrefabs;
    public List<EventCardData> BossPool;

    public List<EventCardData> FinalBossPool;
    //public List<BattleCardDataContainer> BattlePrefabs;
}

public class DayManager : MonoBehaviour
{
    public List<DayResource> dayResources;
    private int dayCount = 3;
    private int currentDay = 1;

    public void SwitchDay (int dayIndex)
    {
        currentDay = dayIndex + 1;

        // validation
        // for(int i = 0; i < Deck.Instance.deckList.Count; i++)
        // {
        //     if (Deck.Instance.deckList[i] >= dayResources[dayIndex].BattlePrefabs.Count)
        //         Deck.Instance.deckList[i] = 0;
        // }

        // Remove all prefabs in the EventDeck & BattleCardDeck in Deck.Instance
        var limit = Deck.Instance.EventDeck.Count;
        for(int index = 0; index < limit; index ++)
        {
            Deck.Instance.EventDeck.RemoveAt(0);
        }
        // limit = Deck.Instance.cardPrefabs.Count;
        // for(int index = 0; index < limit; index ++)
        // {
        //     Deck.Instance.cardPrefabs.RemoveAt(0);
        // }

        // set the EventDeck & BattleCardDeck in Deck.Instance
        foreach(var prefab in dayResources[dayIndex].EventPrefabs)
        {
            Deck.Instance.EventDeck.Add(prefab);
        }
        // foreach(var prefab in dayResources[dayIndex].BattlePrefabs)
        // {
        //     Deck.Instance.cardPrefabs.Add(prefab);
        // }
    }


    // Start is called before the first frame update
    void Start()
    {
        dayCount = dayResources.Count;

        // Enable the below line to follow the day procedure
        // SwitchDay(0); // default day 1, index in list is 0
    }

    // Update is called once per frame
    void Update()
    {

    }
}
