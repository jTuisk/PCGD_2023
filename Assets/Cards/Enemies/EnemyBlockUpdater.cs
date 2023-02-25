using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI text;


    // Update is called once per frame
    void Update()
    {
        if(Deck.Instance.enemy!=null){
        text.text=Deck.Instance.enemy.block+"";
        }
    }
}
