using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New EventCard", menuName = "Cards/Event/SpawnCreature")]
public class SpawnCreature : ScriptableObject
{
    // Start is called before the first frame update

    public GameObject CreatureBase;
    public CreatureDataContainer Creature;

    public void Spawn()
    {
        Deck.Instance.enemy=Instantiate(CreatureBase).GetComponent<EnemyCard>();
        Deck.Instance.enemy.Create(Creature);
        Deck.Instance.inBattle = true;

    }

}
