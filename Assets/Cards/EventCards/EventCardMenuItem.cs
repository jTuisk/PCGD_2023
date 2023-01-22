using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventCardMenuItem 
{
    public string description="";
    public UnityEvent effect;
    public List<Condition> conditions;

}

[Serializable]
public class Condition
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
    public enum type {LESS,EQUALS,FLAG };
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
            case type.FLAG:
                return Deck.Instance.flags.Contains(flag);
            case type.LESS:
                return getTarget(a) <= getTarget(b);
            case type.EQUALS:
                return getTarget(a) == getTarget(b);

        }
        return true;

    }
}
