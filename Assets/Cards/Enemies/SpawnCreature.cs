using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreature : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Creature;

    public void Spawn()
    {
        Deck.Instance.enemy=Instantiate(Creature).GetComponent<EnemyCard>();
        Deck.Instance.inBattle = true;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
