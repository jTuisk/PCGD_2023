using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {

        if (Deck.Instance.inBattle&&Deck.Instance.enemyTurn&&Input.GetKeyDown(KeyCode.Space)) { Debug.Log("Space key was pressed.");
            if (Deck.Instance.enemyTurn && !Deck.Instance.stunned)
            {
                StartCoroutine(Deck.Instance.inBattleEndTurn());
            }
        }


    } }
