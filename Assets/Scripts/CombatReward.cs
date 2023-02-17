using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
// using UnityEngine.

public class CombatReward : MonoBehaviour
{
    public TMPro.TextMeshProUGUI gainMagicianCardButtonText; 
    public Canvas baseCanvas;
    public Canvas breakTimeCanvas;
    public GameObject battleCardPrefab;
    public GameObject ButtonPrefab;
    public GameObject notificationText;

    public int manaThreshold = 0; // the minimal mana that can be converted into maxHP

    public int affectedEnemyCount = 3;

    [HideInInspector]
    public List<BattleCardDataContainer> enemySpecialCardData;

    public void GainMagicianCard()
    {
        // add a random special card into player's deck
        var index = UnityEngine.Random.Range(0, enemySpecialCardData.Count);

        // Instantiate the card prefab & add to player's deck
        var card=Instantiate(battleCardPrefab).GetComponent<Card>();
        card.createCard(enemySpecialCardData[index]);
        card.transform.position = new Vector2(1000000, 100000);
        Deck.Instance.BattleDeckAdd(card);
        card.status = Card.BelongTo.Deck;
        card.name = "enemySpecialCard" + (Deck.Instance.BattleDeck.Count - 1);

        EndReward();

        // show notification
        var obj = Instantiate(notificationText);
        var text = obj.GetComponent<NotificationText>();
        text.SetText("Add card '" + card.CardName.text + "' to your deck.");
    }

    public void EndReward()
    {
        // Set inReward status as false
        Deck.Instance.inReward = false;
        Deck.Instance.eventVisible = false;

        // Set the reward panel false
        Destroy(this.gameObject);
    }

    public void EnterBreakTime()
    {
        baseCanvas.gameObject.SetActive(false);
        breakTimeCanvas.gameObject.SetActive(true);

        // Instantiate buttons
        var verticalMenu = breakTimeCanvas.GetComponentInChildren<VerticalLayoutGroup>();
        if(!verticalMenu)
        {
            Debug.LogError("Vertical menu is missing!");
        }
        else
        {
            if (Deck.Instance.Hp < Deck.Instance.MaxHp && Deck.Instance.mana >= manaThreshold)
            {
                DisplayHpReward(verticalMenu);
            }      
            
            // affect magician deck
            DisplayAffectMagicianReward(verticalMenu);
        }
    }

    private void DisplayHpReward(VerticalLayoutGroup verticalMenu)
    {
        Transform menuTransform = verticalMenu.transform;
        var button = Instantiate(ButtonPrefab);
        button.GetComponent<Button>().onClick.AddListener(delegate { Deck.Instance.GainMaxHpByMama(); });
        button.GetComponent<Button>().onClick.AddListener(delegate { EndReward(); });
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Gain Max HP with the Cost of HP";
        button.transform.SetParent(menuTransform);        
    }

    private void DisplayAffectMagicianReward(VerticalLayoutGroup verticalMenu)
    {
        List<int> enemyIndexes = new List<int>();
        var enemyCount = Deck.Instance.allEnemies.Count;
        var affected = Deck.Instance.enemyAffectedByCombatRewards.Count;

        // pick x pieces of enemy card that can be affected
        do
        {
            // Check if enemy cards are enough
            if((affected + enemyIndexes.Count) >= enemyCount)
            {
                Debug.Log("No more available enemies.");
                break;
            }

            var randomIndex = UnityEngine.Random.Range(0, enemyCount);
            if(!enemyIndexes.Contains(randomIndex) && 
                !Deck.Instance.enemyAffectedByCombatRewards.ContainsKey(Deck.Instance.allEnemies[randomIndex]))
            {
                // make sure the enemy has proper battle cards to be affected.
                if(Deck.Instance.allEnemies[randomIndex].deck.Count <= 0)
                {continue;}

                enemyIndexes.Add(randomIndex);
            }
        }while(enemyIndexes.Count < affectedEnemyCount);
        
        Transform menuTransform = verticalMenu.transform;
        foreach(var index in enemyIndexes)
        {
            // decide which battle card to affect
            var enemyDeck = Deck.Instance.allEnemies[index].deck;
            var battleCardIndex = UnityEngine.Random.Range(0, enemyDeck.Count);

            var button = Instantiate(ButtonPrefab);
            button.GetComponent<Button>().onClick.AddListener(
                delegate { AffectRandomMagiciansDeck(Deck.Instance.allEnemies[index], battleCardIndex); }
                );
            button.GetComponent<Button>().onClick.AddListener(delegate { EndReward(); });
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text 
                = "Make " + Deck.Instance.allEnemies[index].name
                + " lose card "
                + enemyDeck[battleCardIndex].cardName;
            button.transform.SetParent(menuTransform);
        }
    }

    public void AffectRandomMagiciansDeck(CreatureDataContainer enemy, int battleCardIndex)
    {
        Deck.Instance.enemyAffectedByCombatRewards.Add(enemy, battleCardIndex);

        // show notification
        var obj = Instantiate(notificationText);
        var text = obj.GetComponent<NotificationText>();
        text.SetText("Magician " + enemy.name + " loses card " + enemy.deck[battleCardIndex].cardName);
    }


    public void OnCreate(CreatureDataContainer creatureData)
    {
        if(enemySpecialCardData != null)
        {
            enemySpecialCardData = new List<BattleCardDataContainer>();
        }

        // Set the enemy special card
        foreach(var battleCardDataContainer in creatureData.deck)
        {
            if(battleCardDataContainer.IsEnemySpecialCard && !enemySpecialCardData.Contains(battleCardDataContainer))
            {
                enemySpecialCardData.Add(battleCardDataContainer);
            }
        }

        // If the enemy does not have any special cards, enter the breakTime directly
        if(enemySpecialCardData.Count > 0)
        {
            baseCanvas.gameObject.SetActive(true);
            breakTimeCanvas.gameObject.SetActive(false);
        }
        else
        {
            EnterBreakTime();
        }
    }

    public void Awake()
    {

    }

    public void Update()
    {

    }
}
