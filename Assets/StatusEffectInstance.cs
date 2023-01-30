using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class StatusEffectInstance 
{
    // Start is called before the first frame update
    public int duration = 1;
    public List<UnityEvent> effect = new List<UnityEvent>();
    public void trigger()
    {

        foreach (var i in effect)
        {
            i.Invoke();

        }
        duration -= 1;

    }
}
