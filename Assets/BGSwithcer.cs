using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGSwithcer : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite BGEvent;
    public Sprite BGBattle;
    public GameObject enemylist;
    public GameObject BG_Frame;
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
        BG_Frame.SetActive(!Deck.Instance.inBattle);
        //enemylist.SetActive(!Deck.Instance.inBattle);
        if (bg.EVENT!=state&&Deck.Instance.inBattle!=true){
            state=bg.EVENT;
            this.transform.GetChild(0).gameObject.SetActive(false);
            //sr.sprite=BGEvent;
        }else if(bg.BATTLE!=state&&Deck.Instance.inBattle!=false){
            state=bg.BATTLE;
            this.transform.GetChild(0).gameObject.SetActive(true);
            //sr.sprite=BGBattle;
        }
    }
}
