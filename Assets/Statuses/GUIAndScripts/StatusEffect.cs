using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Status", menuName = "Cards/Status")]
public class StatusEffect:ScriptableObject
{
    // Start is called before the first frame update
    public int duration = 1;
    public List<UnityEvent> effect = new List<UnityEvent>();
    public Sprite icon;
    public bool ISDOTMOD=false;
    public string desc;
    public bool targetsEnemy = false;
    public void add()
    {
        var stat = new StatusEffectInstance();
        stat.duration = this.duration;
        stat.effect = this.effect;
        stat.id=this.name;
        stat.icon=this.icon;
        stat.ISDOTMOD=this.ISDOTMOD;
        stat.targetsEnemy=targetsEnemy;
        Deck.Instance.statuses.Add(stat);
        stat.desc=this.desc;
    }
    public void remove(){
        for(int i =0; i<Deck.Instance.statuses.Count; i++){
        if(Deck.Instance.statuses[i].id.Equals(name)){
            Deck.Instance.statuses.RemoveAt(i);
            i--;
        }
        }
    }
        private void ConfuseEnemy(){
            if(!Deck.Instance.reversed){
                Deck.Instance.enemy.confused=true;
            }else{
                Deck.Instance.enemy.confused=false;
            }
    }

    private void ConfusePlayer(){
        if(!Deck.Instance.reversed){
            Deck.Instance.PlayerConfused=true;
        }else{
            Deck.Instance.PlayerConfused=false;
        }
    }
    public void Confuse(){
        if(targetsEnemy){
            ConfuseEnemy();
        }else{
            ConfusePlayer();
        }
    }
    public void Stun() {
        if(targetsEnemy){
            Deck.Instance.enemy.stunned = true;
    }else{
        Deck.Instance.stunned=true;
    }
    }
    public void StunPlayer() {
        Deck.Instance.stunned = true;
    }
    public void gainAPOnExhaust(int amount){
        Deck.Instance.gainAPOnExhaust+=amount;
    }
        public void DoubleDamageModifier()
    {
        if(!targetsEnemy){
        Deck.Instance.enemy.EnemyDamageModifier *= 2;
    }else{
        Deck.Instance.PlayerDamageModifier *= 2;
        Deck.Instance.UpdateEveryCardDescription();        
    }
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

        public void MultiplyEnemyDamageModifier(float amount)
    {
        Deck.Instance.enemy.EnemyDamageModifier *= amount;
    }
            public void MultiplyDamageModifier(float amount)
    {
        if(targetsEnemy){
        Deck.Instance.enemy.EnemyDamageModifier *= amount;
        
    }else{
        Deck.Instance.PlayerDamageModifier *= amount;
        Deck.Instance.UpdateEveryCardDescription();        
    }
    
    }
    public void lucky(){
        if(targetsEnemy){
            Deck.Instance.enemy.Lucky=true;
        }else{
            Deck.Instance.Lucky=true;
        }
    } 
    public void MultiplyPlayerDamageModifier(float amount)
    {
        Deck.Instance.PlayerDamageModifier *= amount;
        Deck.Instance.UpdateEveryCardDescription();
    }
    public void DamageEnemy(int amount){
        Deck.Instance.enemy.takeDamage((int)(amount*Deck.Instance.dotDamageMultiplier));
    }
    public void Damage(int amount){
        if(targetsEnemy){
            Deck.Instance.enemy.takeDamage((int)(amount*Deck.Instance.dotDamageMultiplier));

        }else{

            Deck.Instance.takeDamage((int)(amount*Deck.Instance.dotDamageMultiplier));            
        }
    }
    public void DamagePlayer(int amount)
    {
        Deck.Instance.takeDamage((int)(amount*Deck.Instance.dotDamageMultiplier));
    }
    public void MultiplydotDamageMultiplier(float amount){
        Deck.Instance.dotDamageMultiplier*=amount;
    }
}
