using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusVisual : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler
{
    // Start is called before the first frame update
    public StatusEffectInstance stat;
    public Image statImage; 
    public TMPro.TextMeshProUGUI text;
    StatusAnimator animator;
    public Sprite defaultSprite;
public void init(StatusEffectInstance status,StatusAnimator animator){
    this.animator=animator;
    stat=status;
    statImage.sprite=status.icon;
    if(status.icon=null){
            statImage.sprite=defaultSprite;
        }
}

    // Update is called once per frame
    void Update()
    {
        text.text=""+stat.duration;
        if(stat.duration<=0||!( Deck.Instance.statuses.Contains(stat))){
            animator.statusImages.Remove(this);
            Destroy(this.gameObject);
        }
    }
    GameObject focused;
    public GameObject tooltipBase;
        public void OnPointerEnter(PointerEventData eventData)
    {
        var i=Instantiate(tooltipBase);
        i.GetComponent<StatusTooltip>().createCard(this.stat);
        i.transform.position=this.transform.position;
        focused=i;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exited");
        Destroy(focused);
    }
    private void OnDestroy()
    {
        if (focused != null) {
            Destroy(focused);
        }
    }
}
