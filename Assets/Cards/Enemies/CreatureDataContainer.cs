using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EventCard", menuName = "Cards/Creature")]
public class CreatureDataContainer : ScriptableObject
{
    // Start is called before the first frame update

    public int HP = 10;
    public int MaxDamageRange = 6;
    public int MinDamageRange = 1;
}
