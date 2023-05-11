using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlayerTurn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hide;
    void Start()
    {
        if (Deck.Instance.enemyTurn)
        {
            hide.SetActive(false);
        }   
    }

    // Update is called once per frame
    
}
