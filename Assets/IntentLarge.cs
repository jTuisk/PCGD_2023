using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntentLarge : MonoBehaviour
{
    public TMPro.TextMeshProUGUI _name;
    public TMPro.TextMeshProUGUI _description;
    // Start is called before the first frame update
    public void createCard(BattleCardDataContainer data){
        _name.text=data.cardName;
        _description.text =(data.Damage>0? "Deal " +data.Damage+" Damage":"")+(data.block>0? " Block "+data.block+" Damage":"")+(data.magic>0? " gain "+data.magic+" mana":"")+(data.magic<0? " Costs "+(-data.magic)+" mana":"")+data.effectDescriptor;
    }
}
