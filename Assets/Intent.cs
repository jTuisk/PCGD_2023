using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intent : Card
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Image idamage;
    public TMPro.TextMeshProUGUI dText;
    public Image iHeal;
    public TMPro.TextMeshProUGUI hText;
    public Image iMana;
        public TMPro.TextMeshProUGUI MText;
    public Image iblock;
    public Image iSpecial;
        public TMPro.TextMeshProUGUI Btext; 
    override
    public void createCard(BattleCardDataContainer data){
        BattleCardData = data;

        Damage = data.Damage;
        block = data.block;
        magic = data.magic;
        money = data.money;
        exaust=data.exaust;
        actionCost = data.actionCost;
        effect = data.effect;
        apCost.text=actionCost+"";
        ManaCost.text="";
        CardName.text=data.cardName;
        conditionalEffects=data.conditionalEffects;
        var temp=APImage.color;
        
        temp.a=1f;
        APImage.color=temp;
        APImage.sprite=APSprite;

        if(data.Damage>0){
            idamage.gameObject.SetActive(true);
            dText.text=""+data.Damage;
        }
        if(data.Damage<0){
            iHeal.gameObject.SetActive(true);
            hText.text=""+-data.Damage;
        }
        if(data.magic>0){
            iMana.gameObject.SetActive(true);
            MText.text=""+data.magic;
        }
        if(data.block>0){
            iblock.gameObject.SetActive(true);
            Btext.text=""+data.block;
        }else{
            iblock.gameObject.SetActive(false);
        }
        if(this.effect.GetPersistentEventCount()>0|this.conditionalEffects.Count>0){
            iSpecial.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
