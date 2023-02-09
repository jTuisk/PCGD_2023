using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public int Damage = 0;
    public int block = 0;
    public int magic = 0;
    public int money = 0;
    public int actionCost = 0;
    public TMPro.TextMeshProUGUI description;
    public TMPro.TextMeshProUGUI apCost;
    public TMPro.TextMeshProUGUI ManaCost;
    public TMPro.TextMeshProUGUI CardName;
    public UnityEvent effect;

    public Sprite APSprite;
    public Sprite ManaSprite;
    public Image APImage;
    public Image ManaImage;
    public BattleCardDataContainer BattleCardData { get; private set; }

    public void Playcard()
    {
        if (Deck.Instance.enemy != null)
        {
            Deck.Instance.enemy.takeDamage( Damage);
        }
        Deck.Instance.block += block;
        Deck.Instance.mana += magic;
        Deck.Instance.money -= money;
        effect.Invoke();

        AudioManager.Instance.PlayCardEffectsWhenCardPlayed(this);
    }
    public void EnemyPlayCard(EnemyCard e){
        if(!e.confused){
            Deck.Instance.takeDamage(Damage-Deck.Instance.block);
        }else{
            e.takeDamage(Damage);
        }

        // used cards from enermy go to discard pile?
        this.status = BelongTo.DiscardPile;
        effect.Invoke();

    }
    public void createCard(BattleCardDataContainer data) {

        BattleCardData = data;

        Damage = data.Damage;
        block = data.block;
        magic = data.magic;
        money = data.money;
        actionCost = data.actionCost;
        effect = data.effect;
        apCost.text=actionCost+"";
        ManaCost.text="";
        CardName.text=data.cardName;
        
        var temp=APImage.color;
        
        temp.a=1f;
        APImage.color=temp;
        APImage.sprite=APSprite;

        if(magic<0){
        temp=ManaImage.color;
        temp.a=1f;
        ManaImage.color=temp;
        ManaImage.sprite=ManaSprite;
        ManaCost.text+=-magic;
        }
        description.text =(data.Damage>0? "Deal " +data.Damage+" Damage":"")+(data.block>0? " Block "+data.block+" Damage":"")+(data.magic>0? " gain "+data.magic+" mana":"")+(data.magic<0? " Costs "+(-data.magic)+" mana":"")+data.effectDescriptor;
    }

    private bool movable = false;
    //private Vector3 moveStartPosition; 
    private Vector3 moveTargetPosition;
    private float moveStartTime = 0.0f;
    private float moveDuration = 5.0f; //5.0 seconds
    public bool inHand = false;
    
    // The Card GameObject is a Child of Deck/Hand/Enermy ...
    public enum BelongTo {Default, Deck, PlayerHand, Enermy, DiscardPile} 
    public BelongTo status = BelongTo.Default;

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
    private bool reset=false;
    void Update()
    {
        if (this.transform.parent != null) {
            moveToHand();
            reset = false;
        } else{
            inHand=false;
            if(reset==false){
            reset=true;
            resetScale();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardHandler.Instance.SetCurrentFocusedCard(this);
        DisplayOnPointerEnter(eventData);
        PlayAudioOnPointerEnter(eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CardHandler.Instance.RemoveCurrentFocusedCard();
        DisplayOnPointerExit(eventData);
    }


    private void DisplayOnPointerEnter(PointerEventData eventData)
    {
        if(status == BelongTo.PlayerHand)// || true) // true -> showContentOfDeck == true
        {
            // enlarge the card scale;
            var scale = CardHandler.Instance.cardScaleInPlayerHandWhenHovering;
            this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            // todo render order
            gameObject.GetComponentInChildren<Canvas>().sortingOrder=20;
        }
    }
    private void resetScale(){
        // enlarge the card scale;
        var scale = CardHandler.Instance.cardOriginalScale;
        this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        gameObject.GetComponentInChildren<Canvas>().sortingOrder=0;
    }
    private void DestroyFocused(){
        // enlarge the card scale;
        var scale = CardHandler.Instance.cardOriginalScale;
        this.gameObject.transform.localScale = new Vector3(scale, scale, scale);            
        gameObject.GetComponentInChildren<Canvas>().sortingOrder=0;
    }
    private void DisplayOnPointerExit(PointerEventData eventData)
    {
        if(status == BelongTo.PlayerHand)// || true) // true -> showContentOfDeck == true
        {
            resetScale();
            // enlarge the card scale;
            //var scale = CardHandler.Instance.cardOriginalScale;
            //this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            //gameObject.GetComponentInChildren<Canvas>().sortingOrder=0;
        }
    }

    private void PlayAudioOnPointerEnter(PointerEventData eventData)
    {
        if(status == BelongTo.PlayerHand)
        {
            AudioManager.Instance.PlayOneShot(AudioManager.AudioEffects.exploreCard);
        }
    }
}
