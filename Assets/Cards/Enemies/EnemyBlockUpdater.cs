using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI text;

    [SerializeField] GameObject BlockGameObject;

    // Update is called once per frame
    void Update()
    {
        if(Deck.Instance.enemy!=null && BlockGameObject != null){
            int block = Deck.Instance.enemy.block;
            BlockGameObject.SetActive(block != 0);

            if(block != 0)
            {
                text.text = $"{block}";
            }
        }
    }
}
