using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public EnemyCard card;
    float max;
    void Start()
    {
        max=card.HP;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value=card.HP/max;        
    }
}
