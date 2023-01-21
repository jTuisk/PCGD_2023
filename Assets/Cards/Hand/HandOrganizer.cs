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
            //move card to mouse position if player drags mouse and card distance of the cursor is lower than hitboxsize
            var campos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousepos = new Vector3(campos.x,campos.y,transform.position.z);
            if ((mousepos - child.position).magnitude < HitboxSize&& Input.GetMouseButton(0))
            {
                    child.position = mousepos;

            }
            else
            {
                //when player releases card and its y position is above 700 and the player can afford it play the card
                if (child.position.y > 5 && child.position.x < 700)
                {

                    var card = child.GetComponent<Card>();
                    Debug.Log("play Card");
                    if (Deck.Instance.mana + card.magic >= 0)
                    {

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
                }
                //move cards to correct position
                child.position = new Vector2(-transform.childCount * 10/2  + i * 10, -10);
            }
            i++;
        }
    }
}
