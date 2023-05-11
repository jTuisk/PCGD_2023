using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Cross1;
    public GameObject Cross2;
    public GameObject Scroll;
    void Start()
    {
        
    }
    public void defeat()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Scroll.SetActive(!Deck.Instance.inBattle);

        Cross1.SetActive(Deck.Instance.bossesDefeated > 0);
        
    }
}
