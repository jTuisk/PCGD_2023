using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    public int Damage = 0;
    public int block = 0;
    public int magic = 0;
    public int money = 0;
    public int actionCost = 0;
    public TMPro.TextMeshProUGUI description;
    public TMPro.TextMeshProUGUI apCost;
        public TMPro.TextMeshProUGUI CardName;
    public UnityEvent effect;

    public void Playcard()
    {
        if (Deck.Instance.enemy != null)
        {
            Deck.Instance.enemy.HP -= Damage;
        }
        Deck.Instance.block += block;
        Deck.Instance.mana += magic;
        Deck.Instance.money -= money;
        effect.Invoke();

    }
    public void EnemyPlayCard(){
        Deck.Instance.takeDamage(Damage-Deck.Instance.block);
        effect.Invoke();

    }
    public void createCard(BattleCardDataContainer data) {

        
        Damage = data.Damage;
        block = data.block;
        magic = data.magic;
        money = data.money;
        actionCost = data.actionCost;
        effect = data.effect;
        apCost.text=actionCost+"";
        CardName.text=data.cardName;
        description.text =(data.Damage>0? "Deal " +data.Damage+" Damage":"")+(data.block>0? "Block "+data.block+" Damage":"")+(data.magic>0? "gain "+data.magic+"mana":"")+data.effectDescriptor;
    }

    private bool movable = false;
    //private Vector3 moveStartPosition; 
    private Vector3 moveTargetPosition;
    private float moveStartTime = 0.0f;
    private float moveDuration = 5.0f; //5.0 seconds
    public bool inHand = false; 

    // Called by HandOrganizer to activate the card movement
    public void triggerMove(Vector3 startPos, Vector3 endPos)
    {
        // TODO: Consider if the card is the child of Deck, should firstly unbound
        // to make sure the transform.position is in the same hierarchy with startPos & endPos

        //moveStartPosition = startPos;
        moveTargetPosition = endPos;
        moveStartTime = Time.time;
        transform.position = startPos;

        movable = true;
    }

    // Called when the card arrive its destination so that possible post-tasks can be done here
    public void terminateMove()
    {
        movable = false;
        inHand = true;

        // possible TODO: set the card as child of target
    }

    // Called every frame from Update(), so that the cards can slide to hand 
    public void moveToHand()
    {
        if(movable == false){return;}
        if(transform.position == moveTargetPosition){terminateMove(); return;}
        
        // the card is movable, and it has not reached the destination.
        transform.position = Vector3.Lerp(transform.position, moveTargetPosition, (Time.time - moveStartTime)/moveDuration);
    }

    public void moveToTargetPos(Vector3 targetPos)
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, (Time.time - moveStartTime)/moveDuration);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.parent!=null){
            moveToHand();
            
        }else{
            inHand=false;
        }
    }
}
