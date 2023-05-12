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
            public Image ComboImage;
    public TMPro.TextMeshProUGUI ComboText;
    public Sprite SpecialEffect;
    
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
        AudioOnPlay = data.AudioOnPlay;
        CardName.text=data.cardName;
        conditionalEffects=data.conditionalEffects;
        SpecialEffect = data.enemyCardData.SpecialAnimation;
        //var temp=APImage.color;
        
        //temp.a=1f;
        //APImage.color=temp;
        //APImage.sprite=APSprite;
        int effectCount=0;
        if(data.enemyCardData.Dot>0){
            DotImage.gameObject.SetActive(true);
            DotText.text=""+data.enemyCardData.Dot;
            effectCount++;
        }
        if(data.enemyCardData.Confusion>0){
            iConfusion.gameObject.SetActive(true);
            confusionText.text=""+data.enemyCardData.Confusion;
            effectCount+=2;
        }
        if(data.enemyCardData.Reverse>0){
            ReverseImage.gameObject.SetActive(true);
            ReverseText.text=""+data.enemyCardData.Reverse;
            effectCount++;
        }
        if(data.enemyCardData.Vulnerable>0){
            VulnerableImage.gameObject.SetActive(true);
            VulnerableText.text=""+data.enemyCardData.Vulnerable;
            effectCount++;
        }
        if(data.enemyCardData.stun>0){
            StunImage.gameObject.SetActive(true);
            StunText.text=""+data.enemyCardData.stun;
            effectCount++;
        }
        if(data.enemyCardData.combo>0){
            ComboImage.gameObject.SetActive(true);
            ComboText.text=""+data.enemyCardData.combo;
            effectCount++;
        }
        
        if(data.Damage>0|data.enemyCardData.DamageOverride){
            idamage.gameObject.SetActive(true);
            dText.text=""+(data.enemyCardData.DamageOverride?data.enemyCardData.DamageOverrideString:data.Damage*Deck.Instance.PlayerDamageModifier);
            if(data.enemyCardData.DamageOverride){effectCount++;}
        }
        if(data.Damage<0|data.enemyCardData.HealOverride){
            iHeal.gameObject.SetActive(true);
            hText.text=""+(data.enemyCardData.HealOverride?data.enemyCardData.HealOverrideString:-data.Damage*Deck.Instance.PlayerDamageModifier);
            if(data.enemyCardData.HealOverride){effectCount++;}
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
        if((effectCount<this.effect.GetPersistentEventCount()+this.conditionalEffects.Count)&&(this.effect.GetPersistentEventCount()>0|this.conditionalEffects.Count>0)){
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

        updateState();
        UpdateDamageText();
    }
}
