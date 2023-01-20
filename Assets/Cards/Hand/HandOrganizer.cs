using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOrganizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public float HitboxSize;
    void Update()
    {
        int i = 0;
        foreach(Transform child in transform)
        {
            var campos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousepos = new Vector3(campos.x,campos.y,transform.position.z);
            if ((mousepos - child.position).magnitude < HitboxSize&& Input.GetMouseButton(0))
            {
                    child.position = mousepos;

            }
            else
            {
                if (child.position.y > 5 && child.position.x<700) {
                    Debug.Log("play Card");

                    var card=child.GetComponent<Card>();
                    if (Deck.Instance.actionPoints >= card.actionCost)
                    {
                        Deck.Instance.actionPoints -= card.actionCost;
                        card.Playcard();
                        Deck.Instance.BattleDiscardPile.Add(card);
                        child.parent = null;
                        child.position = new Vector2(10000, 100000);
                        break;
                    }
                }
                child.position = new Vector2(-transform.childCount * 10/2  + i * 10, -10);
            }
            i++;
        }
    }
}
