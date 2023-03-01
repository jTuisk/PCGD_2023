using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public bool autoUpdate = true;

    public Slider slider;
    [Range(0.0f, 1f)]
    public float customSliderValue = 1f;

    public EnemyCard card;


    [SerializeField] bool showHealthText = true;
    [SerializeField] TMPro.TextMeshProUGUI healthText;


    float maxHp = 100f;

    void Start()
    {
        if (card != null)
        {
            maxHp = card.HP;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (card == null)
            return;

        float currentHp = card.HP;

        slider.value = autoUpdate ? currentHp/maxHp : customSliderValue;

        healthText.gameObject.SetActive(showHealthText);
        if (showHealthText)
        {
            healthText.text = $"{currentHp} / {maxHp}";
        }

        /*
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
        else { slider.value = sliderValue; }*/
    }
}
