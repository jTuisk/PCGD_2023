using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (fileName ="New BattleCard", menuName ="Cards/Battle")]
public class BattleCardDataContainer : ScriptableObject
{
    public string effectDescriptor = "";
    public int Damage = 0;
    public int block = 0;
    public int magic = 0;
    public int money = 0;
    public int actionCost = 0;
    public UnityEvent effect;


    public void addCard()
    {
        Deck.Instance.BattleDeckAddCardFromCardData(this);

    }
}
