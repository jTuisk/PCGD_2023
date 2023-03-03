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

    public Image DotImage;
    public TMPro.TextMeshProUGUI DotText;
    public Image iConfusion;
    public TMPro.TextMeshProUGUI confusionText;
        public Image ReverseImage;
    public TMPro.TextMeshProUGUI ReverseText;
        public Image VulnerableImage;
    public TMPro.TextMeshProUGUI VulnerableText;
        public Image StunImage;
    public TMPro.TextMeshProUGUI StunText;
    
    
    override
    public void createCard(BattleCardDataContainer data, bool saveContainerData = true)
    {

        if (saveContainerData)
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
        if(data.enemyCardData.Dot>0){
            DotImage.gameObject.SetActive(true);
            DotText.text=""+data.enemyCardData.Dot;
        }
        if(data.enemyCardData.Confusion>0){
            iConfusion.gameObject.SetActive(true);
            confusionText.text=""+data.enemyCardData.Confusion;
        }
        if(data.enemyCardData.Reverse>0){
            ReverseImage.gameObject.SetActive(true);
            ReverseText.text=""+data.enemyCardData.Reverse;
        }
        if(data.enemyCardData.Vulnerable>0){
            VulnerableImage.gameObject.SetActive(true);
            VulnerableText.text=""+data.enemyCardData.Vulnerable;
        }
        if(data.enemyCardData.stun>0){
            StunImage.gameObject.SetActive(true);
            StunText.text=""+data.enemyCardData.stun;
        }
        if(data.Damage>0){
            idamage.gameObject.SetActive(true);
            dText.text=""+data.Damage*Deck.Instance.PlayerDamageModifier;
        }
        if(data.Damage<0){
            iHeal.gameObject.SetActive(true);
            hText.text=""+(data.enemyCardData.DamageOverride?data.enemyCardData.DamageOverrideString:-data.Damage*Deck.Instance.PlayerDamageModifier);
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
    public void UpdateDamageText(){
        if(Damage>0){
            dText.text=""+Damage*Deck.Instance.PlayerDamageModifier;
        }
        if(Damage<0){
            hText.text=""+-Damage*Deck.Instance.PlayerDamageModifier;
        }

    }
    // Update is called once per frame
    void Update()
    {       
        UpdateDamageText();
    }
}
