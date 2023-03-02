using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;
    public float sliderValue = 1f;

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

        healthText.gameObject.SetActive(showHealthText);
        if (showHealthText)
        {
            healthText.text = $"{currentHp} / {maxHp}";
        }
        
        if((sliderValue - currentHp / maxHp)  < 0.01f)
        {
            sliderValue = currentHp / maxHp;
        }

        if (currentHp / maxHp > sliderValue)
        {
            sliderValue += Time.deltaTime;
            slider.value = Mathf.Lerp(0, 1, sliderValue);
        }
        else if (currentHp / maxHp < sliderValue)
        {
            sliderValue -= Time.deltaTime;
            slider.value = Mathf.Lerp(0, 1, sliderValue);
        }
        else
        {
            slider.value = sliderValue;
        }
    }
}
