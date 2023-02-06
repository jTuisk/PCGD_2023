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
        Deck.Instance.statuses.Add(stat);

    }

    public void StunEnemy() {
        Deck.Instance.enemy.stunned = true;
    }
    public void StunPlayer() {
        Deck.Instance.stunned = true;
    }
    public void DoubleEnemyDamageModifier()
    {
        Deck.Instance.enemy.EnemyDamageModifier = 2;
    }
    public void DoublePlayerDamageModifier()
    {
        Deck.Instance.PlayerDamageModifier *= 2;
    }
        public void DamageEnemy(){
        Deck.Instance.enemy.takeDamage(1);
    }
}
