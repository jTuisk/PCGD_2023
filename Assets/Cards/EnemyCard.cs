using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP = 10;
    public int MaxDamageRange = 6;
    public int MinDamageRange = 1;
    public int damage = 0;
    public TMPro.TextMeshProUGUI text; 
    void Start()
    {
        Deck.Instance.battleStart();
    }

    public void Create(CreatureDataContainer data)
    {
        HP = data.HP;
        MaxDamageRange = data.MaxDamageRange;
        MinDamageRange = data.MinDamageRange;


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
            Destroy(gameObject);
        }
        
    }
}
