using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "New Status", menuName = "Cards/Status")]
public class StatusEffect:ScriptableObject
{
    // Start is called before the first frame update
    public int duration = 1;
    public List<UnityEvent> effect = new List<UnityEvent>();
    public void add()
    {
        var stat = new StatusEffectInstance();
        stat.duration = this.duration;
        stat.effect = this.effect;
        stat.id=this.name;
        Deck.Instance.statuses.Add(stat);

    }
    public void remove(){
        for(int i =0; i<Deck.Instance.statuses.Count; i++){
        if(Deck.Instance.statuses[i].id.Equals(name)){
            Deck.Instance.statuses.RemoveAt(i);
            i--;
        }
        }
    }

    public void StunEnemy() {
        Deck.Instance.enemy.stunned = true;
    }
    public void StunPlayer() {
        Deck.Instance.stunned = true;
    }
    public void DoubleEnemyDamageModifier()
    {
        Deck.Instance.enemy.EnemyDamageModifier *= 2;
    }
    public void DoublePlayerDamageModifier()
    {
        Deck.Instance.PlayerDamageModifier *= 2;
        Deck.Instance.UpdateEveryCardDescription();
    }
    public void DamageEnemy(){
        Deck.Instance.enemy.takeDamage(1);
    }
    public void DamagePlayer()
    {
        Deck.Instance.takeDamage(1);
    }
}
