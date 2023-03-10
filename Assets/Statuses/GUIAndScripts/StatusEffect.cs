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
    public string nameText;
    public bool ISDOTMOD=false;
    public string desc;
    public bool targetsEnemy = false;
    public void add()
    {
        var stat = new StatusEffectInstance();
        stat.duration = this.duration;
        stat.effect = this.effect;
        stat.id=this.name;
        stat.name=this.nameText;
        stat.icon=this.icon;
        stat.ISDOTMOD=this.ISDOTMOD;
        if(targetsEnemy){
        if(Deck.Instance.PlayerConfused){
            stat.targetsEnemy=!targetsEnemy;
        }else{
            stat.targetsEnemy=targetsEnemy;
        }}
        if(!targetsEnemy){
        if(Deck.Instance.enemy.confused){
            stat.targetsEnemy=!targetsEnemy;
        }else{
            stat.targetsEnemy=targetsEnemy;
        }}
        Deck.Instance.statuses.Add(stat);
        stat.desc=this.desc;
    }
    public void AddTobattleStart(string enemy){
        List<StatusEffect> output;
        Debug.Log("Adding status :"+this.name+" to "+enemy);
        if(!Deck.Instance.ApplyStatusToEnemy.TryGetValue(enemy,out output)){
            Deck.Instance.ApplyStatusToEnemy.TryAdd(enemy,new List<StatusEffect>());
            Deck.Instance.ApplyStatusToEnemy[enemy].Add(this);
        }else{
            Deck.Instance.ApplyStatusToEnemy[enemy].Add(this);
        }
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
            if(!Deck.Instance.PlayerConfused){
                Deck.Instance.enemy.confused=true;
            }else{
                Deck.Instance.enemy.confused=false;
            }
    }

    private void ConfusePlayer(){
        if(!Deck.Instance.enemy.confused){
            Deck.Instance.PlayerConfused=true;
        }else{
            Deck.Instance.PlayerConfused=false;
        }
    }
    public void Confuse(){
        if(StatusEffectInstance.getActiveTargetsEnemy()){
            ConfuseEnemy();
        }else{
            ConfusePlayer();
        }
    }
    public void Stun() {
        if(StatusEffectInstance.getActiveTargetsEnemy()){
            Deck.Instance.enemy.stunned = true;
    }else{
        Deck.Instance.stunned=true;
    }
    }
    public void StunPlayer() {
        Deck.Instance.stunned = true;
    }
    public void StunEnemy() {
        Deck.Instance.stunned = true;
    }
    public void gainAPOnExhaust(int amount){
        Deck.Instance.gainAPOnExhaust+=amount;
    }
        public void DoubleDamageModifier()
    {
        if(!StatusEffectInstance.getActiveTargetsEnemy()){
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
        if(StatusEffectInstance.getActiveTargetsEnemy()){
        Deck.Instance.enemy.EnemyDamageModifier *= amount;
        
    }else{
        Deck.Instance.PlayerDamageModifier *= amount;
        Deck.Instance.UpdateEveryCardDescription();        
    }
    
    }
    public void lucky(){
        if(StatusEffectInstance.getActiveTargetsEnemy()){
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
        if(StatusEffectInstance.getActiveTargetsEnemy()){
            Deck.Instance.enemy.takeDamage((int)(amount*Deck.Instance.dotDamageMultiplier));

        }else{

            Deck.Instance.takeDamage((int)(amount*Deck.Instance.playerDotDamageMultiplier));            
        }
    }
    public void DamagePlayer(int amount)
    {
        Deck.Instance.takeDamage((int)(amount*Deck.Instance.playerDotDamageMultiplier));
    }
    public void MultiplydotDamageMultiplier(float amount){
        if(StatusEffectInstance.getActiveTargetsEnemy()){
        Deck.Instance.dotDamageMultiplier*=amount;
    }else{
        Deck.Instance.playerDotDamageMultiplier*=amount;
    }
    }
}
