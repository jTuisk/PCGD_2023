using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class StatusEffectInstance 
{
    // Start is called before the first frame update
    public int duration = 1;
    public List<UnityEvent> effect = new List<UnityEvent>();
    internal string id;
       internal string name;
    public Sprite icon;
    public bool ISDOTMOD;
    public string desc;
    public bool triggered = false;
    private static bool ActiveTargetsEnemy; 
    public bool targetsEnemy;
    public void trigger()
    {
        ActiveTargetsEnemy=targetsEnemy;
        if(id.Equals("StunEnemy")&&Deck.Instance.enemy.stunned){return;}
        if(id.Equals("StunPlayer")&&Deck.Instance.stunned){return;}
        foreach (var i in effect)
        {
            i.Invoke();

        }
        duration -= 1;


    }
    public static bool getActiveTargetsEnemy(){
        return ActiveTargetsEnemy;
    }
    public void multiplyStatusLength(float amount){
        duration=(int)(amount*duration);    
    }
    public string getID(){
        return id;
    }
}
