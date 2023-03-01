using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    [SerializeField] bool autoUpdate = true;

    [Header("Panel TMP")]
    [SerializeField] TMPro.TextMeshProUGUI blockText;
    [SerializeField] TMPro.TextMeshProUGUI currentHealthText;
    [SerializeField] TMPro.TextMeshProUGUI maxHealthText;
    [SerializeField] TMPro.TextMeshProUGUI manaText;
    [SerializeField] TMPro.TextMeshProUGUI currentToolPointsText;
    [SerializeField] TMPro.TextMeshProUGUI maxToolPointsText;

    // Update is called once per frame
    void Update()
    {
        blockText.text = $"{Deck.Instance.block}";

        currentHealthText.text = $"{Deck.Instance.Hp}";
        maxHealthText.text = $"{Deck.Instance.MaxHp}";

        manaText.text = $"{Deck.Instance.mana}";

        currentToolPointsText.text = $"{Deck.Instance.actionPoints}";
        maxToolPointsText.text = $"{Deck.Instance.MaxactionPoints}";
    }


}
