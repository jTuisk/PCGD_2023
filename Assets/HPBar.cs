using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public EnemyCard card;
    public float sliderValue = 1;
    float max;
    void Start()
    {
        max=card.HP;
        sliderValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if((sliderValue-(float)card.HP / max)  < 0.001)
        {
            sliderValue = (float)card.HP / max;
        }
        if ((float)card.HP / max > sliderValue)
        {
            sliderValue += Time.deltaTime;
            slider.value = Mathf.Lerp( 0, 1, sliderValue);
        }
        else if ((float)card.HP / max < sliderValue)
        {
            sliderValue -=   Time.deltaTime;
            slider.value = Mathf.Lerp( 0, 1, sliderValue);
        }
        else { slider.value = sliderValue; }
    }
}
