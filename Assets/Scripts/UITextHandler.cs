using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextHandler : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI playerDeckAmounText;
    [SerializeField] TMPro.TextMeshProUGUI discardPileAmounText;


    private void Update()
    {
        playerDeckAmounText.text = $"{ Deck.Instance.BattleDeck.Count}";
        discardPileAmounText.text = $"{Deck.Instance.BattleDiscardPile.Count}";
    }
}
