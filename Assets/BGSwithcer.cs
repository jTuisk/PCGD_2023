using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGSwithcer : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite BGEvent;
     public Sprite BGBattle;
     Image sr;
    void Start()
    {
        sr=GetComponent<Image>();
    }
    enum bg{EVENT,BATTLE};
    bg state=bg.EVENT;
    // Update is called once per frame
    void Update()
    {
        if(bg.EVENT!=state&&Deck.Instance.inBattle!=true){
            state=bg.EVENT;
            sr.sprite=BGEvent;
        }else if(bg.BATTLE!=state&&Deck.Instance.inBattle!=false){
            state=bg.BATTLE;
            sr.sprite=BGBattle;
        }
    }
}
