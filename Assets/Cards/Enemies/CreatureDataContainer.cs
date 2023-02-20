using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New EventCard", menuName = "Cards/Creature")]
public class CreatureDataContainer : ScriptableObject
{
    // Start is called before the first frame update

    public int HP = 10;
    public int MaxDamageRange = 6;
    public int MinDamageRange = 1;
    public List<BattleCardDataContainer> deck;
    public GameObject CreatureBase;
    public EventCardData postBattleEvent;
    public Sprite Picture;
    public void Spawn()
    {
        Deck.Instance.enemy = Instantiate(CreatureBase).GetComponent<EnemyCard>();
        Deck.Instance.enemy.Create(this);
        Deck.Instance.inBattle = true;

    }

}
