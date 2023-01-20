using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New EventCard", menuName = "Cards/Event/Normal")]
public class EventCardData : ScriptableObject
{
    public UnityEvent e;
    public string eventText;
}
