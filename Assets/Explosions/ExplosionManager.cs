using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    // Start is called before the first frame

    public static ExplosionManager Instance { get; private set; }
    private void Awake()
    {
        //singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public GameObject heartExplosion;
    public Transform heartExplosionTarget; 
    public GameObject armorExplosion;
    public Transform armorExplosionTarget;
    public GameObject manaExplosion;
    public Transform manaExplosionTarget;
    public void playCard(Card card){
        if(card.block!=0){
            PlayArmorAnimation(card.block);
        }
        if(card.magic!=0){
            PlayManaAnimation(card.magic);
        }
    }
    public void PlayHealthAnimation(int damage){
        var i=Instantiate(heartExplosion);
        var t=i.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        t.text=(damage>0?"-":"+") +Mathf.Abs(damage);
        i.transform.position=heartExplosionTarget.position;
    }
        public void PlayHealthAnimation(int damage,Vector3 pos){
        var i=Instantiate(heartExplosion);
        var t=i.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        t.text=(damage>0?"-":"+") +Mathf.Abs(damage);
        i.transform.position=pos;
    }
        public void PlayArmorAnimation(int block){
        var i=Instantiate(armorExplosion);
        i.transform.position=armorExplosionTarget.position;
        var t=i.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        t.text=(block>0?"-":"+") +Mathf.Abs(block);
        i.transform.position=armorExplosionTarget.position;
    }
    public void PlayArmorAnimation(int block,Vector3 pos){
        var i=Instantiate(armorExplosion);
        var t=i.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        t.text=(block>0?"-":"+") +Mathf.Abs(block);
        i.transform.position=pos;
    }
        public void PlayManaAnimation(int mana){
        var i=Instantiate(manaExplosion);
        i.transform.position=manaExplosionTarget.position;
        var t=i.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        t.text=(mana>0?"-":"+") +Mathf.Abs(mana);
        i.transform.position=manaExplosionTarget.position;
    }

}
