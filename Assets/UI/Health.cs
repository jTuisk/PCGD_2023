using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public TMPro.TextMeshProUGUI HealthText;
    // Start is called before the first frame update
    public enum UITextVisual{
        MONEY,HEALTH,MAGIC,ACTIONPOINTS,DECK_SIZE,DISCARD_PILE
    }
    public UITextVisual uiMode = UITextVisual.HEALTH;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //common script for UI elements
        switch (uiMode) {
            case UITextVisual.HEALTH:
                HealthText.text = Deck.Instance.Hp+"    "+Deck.Instance.MaxHp + "";
                break;
            case UITextVisual.MONEY:
                HealthText.text = Deck.Instance.money + " money ";
                break;
            case UITextVisual.MAGIC:
                HealthText.text = Deck.Instance.mana + "";
                break;
            case UITextVisual.ACTIONPOINTS:
                HealthText.text = Deck.Instance.actionPoints+"/"+Deck.Instance.MaxactionPoints+"";
                break;
            case UITextVisual.DECK_SIZE:
                HealthText.text = Deck.Instance.BattleDeck.Count + "";
                break;
            case UITextVisual.DISCARD_PILE:
                HealthText.text = Deck.Instance.BattleDiscardPile.Count + "";
                break;
        }
    }
}
