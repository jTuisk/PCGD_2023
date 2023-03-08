using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [Serializable]
public class BattleCardMenuItem 
{
    public static void Activate(List<Effect> effects,bool Independent)
    {
        var sumOfprob=0.0f;
        float v=0;
        if(Deck.Instance.enemyTurn){
        if(!Deck.Instance.Lucky){
            v = UnityEngine.Random.value;
        }else{
            v =0.95f;
        }
        }else{
            if(Deck.Instance.enemy!=null){
            if(!Deck.Instance.enemy.Lucky){
                v = UnityEngine.Random.value;
            }else{
                v =0.95f;
            }} 
        }
        Deck.Instance.eventVisible = false;
        foreach(var effect in effects)
        {
            if(effect.possibility == 0)
                continue;


            var p = effect.possibility;

            if(sumOfprob+p >= v && v >= sumOfprob)
            {
                effect.targetEvent.Invoke();
                if(Independent){
                Debug.Log("'" + effect.name +"' takes place cuz a " + p * 100 + "% chance of happening is fulfilled by random value " + v);
            
            }else{
                Debug.Log("'" + effect.name +"' takes place cuz rolled " + v + "is between " + (float)(p+sumOfprob)+" and "+sumOfprob);
            
            }
            }
            else
            {
                if(!Independent){
                    Debug.Log("'" + effect.name +"'wont take take place cuz rolled " + v + "is not between " + p+sumOfprob+" and "+sumOfprob);
                }
                Debug.Log("'" + effect.name +"' won't take place cuz a " + p * 100 + "% chance of happening is not fulfilled by random value " + v);
            }
            if(!Independent){
                sumOfprob+=effect.possibility;
                if(sumOfprob>1){
                    Debug.Log("Warning sum of propabilities is greater than 1 this might make some events unable to trigger. if this was intended change the mode to IndependentRNG");
                }
            }
        }

    }
    //public UnityEvent effect;
    public bool independentRNG=true;
    public List<Effect> ConditionalEffects;
    public List<BattleCondition> conditions;
}

[Serializable]
public class BattleCondition
{

    private float getTarget(target t) {
        switch (t)
        {
            case target.HEALTH:
                return Deck.Instance.Hp;
            case target.MANA:
                return Deck.Instance.mana;
            case target.FLOAT:
                return value;
         
        }
        return 0;


    
    }
    public bool IsStunned(){
        foreach(var status in Deck.Instance.statuses){
            if(status.getID().Contains("Stun")){return true;}
        }
        return false;
    }
    
    
    public enum type {LESS,EQUALS,FLAG,NOFLAG,ISSTUNNED,PLAYERISCONFUSED,ENEMYISCONFUSED };
    public enum target { HEALTH, MANA, FLOAT };
    public string flag;
    public type Operation;
    public float value;
    public target a;
    public target b;
    public bool Evaluate()
    {

        switch (Operation)
        {
            case type.ENEMYISCONFUSED:
                return Deck.Instance.enemy.confused;
            case type.PLAYERISCONFUSED:
                return Deck.Instance.PlayerConfused;            
            case type.ISSTUNNED:
                return IsStunned();
            case type.NOFLAG:
                return !Deck.Instance.flags.Contains(flag);
            case type.FLAG:
                return Deck.Instance.flags.Contains(flag);
            case type.LESS:
                return getTarget(a) <= getTarget(b);
            case type.EQUALS:
                return getTarget(a) == getTarget(b);

        }
        return true;

    }}
