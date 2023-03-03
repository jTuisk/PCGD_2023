using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;

    public EnemyCard card;

    [SerializeField] bool showHealthText = true;
    [SerializeField] TMPro.TextMeshProUGUI healthText;
    float maxHp = 100f;
    float currentHp = 100f;

    [SerializeField] float difference = 0.05f;

    void Start()
    {
        if (card != null)
        {
            maxHp = card.HP;
            currentHp = maxHp;
            slider.value = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (card == null)
            return;

        currentHp = card.HP;

        healthText.gameObject.SetActive(showHealthText);
        if (showHealthText)
        {
            healthText.text = $"{currentHp} / {maxHp}";
        }

        slider.value = GetNewSliderValue();
    }

    private float GetNewSliderValue()
    {
        float currentSliderValue = slider.value;
        float finalSliderValue = currentHp / maxHp;

        if (currentSliderValue > finalSliderValue + difference * Time.deltaTime)
            return currentSliderValue - difference * Time.deltaTime;

        if (currentSliderValue < finalSliderValue - difference * Time.deltaTime)
            return currentSliderValue + difference * Time.deltaTime;

        return finalSliderValue;
    }
}
