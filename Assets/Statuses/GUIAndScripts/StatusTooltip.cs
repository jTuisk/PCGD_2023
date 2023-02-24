using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTooltip : MonoBehaviour
{

    public TMPro.TextMeshProUGUI _name;
    public TMPro.TextMeshProUGUI _description;
    // Start is called before the first frame update
    public void createCard(StatusEffectInstance data){
        _name.text=data.id;
        _description.text =data.desc;
    }


}
