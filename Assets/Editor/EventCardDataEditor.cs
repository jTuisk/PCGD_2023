using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventCardData))]
public class EventCardDataEditor : Editor
{

    SerializedProperty eName;
    SerializedProperty eventText;

    SerializedProperty options;
    //SerializedProperty Operation;
    //SerializedProperty flag;
    //SerializedProperty value;
    //SerializedProperty a;
    //SerializedProperty b;

    //group these
    SerializedProperty ReplaceFrom;
    SerializedProperty ReplaceTo;
    SerializedProperty replaceEvent;
    SerializedProperty addedTag;


    bool replaceDataGroup;


    private void OnEnable()
    {
        eName = serializedObject.FindProperty("eName");
        eventText = serializedObject.FindProperty("eventText");

        options = serializedObject.FindProperty("options");
        //Operation = serializedObject.FindProperty("Operation");
        //flag = serializedObject.FindProperty("flag");
        //value = serializedObject.FindProperty("value");
        //a = serializedObject.FindProperty("a");
        //b = serializedObject.FindProperty("b");

        ReplaceFrom = serializedObject.FindProperty("ReplaceFrom");
        ReplaceTo = serializedObject.FindProperty("ReplaceTo");
        replaceEvent = serializedObject.FindProperty("replaceEvent");
        addedTag = serializedObject.FindProperty("addedTag");
    }

    public override void OnInspectorGUI()
    {
        EventCardData _eventCardData = (EventCardData)target;
        List<EventCardMenuItem> _items = _eventCardData.options;

        serializedObject.Update();

        EditorGUILayout.PropertyField(eName);
        EditorGUILayout.PropertyField(eventText);
        EditorGUILayout.PropertyField(options);


        /*foreach (EventCardMenuItem item in _items)
        {
            // display description, Effect, Conditions

            foreach (Condition condition in item.conditions)
            {  
                // display effects
            }

            foreach (Condition condition in item.conditions)
            {  
                // display Operation

                if (condition.Operation.Equals(Condition.type.FLAG))
                {
                    // display Flag textbox.
                }
                // display value, a, b
            }
        }*/

        replaceDataGroup = EditorGUILayout.BeginFoldoutHeaderGroup(replaceDataGroup, "Replace data");
        if (replaceDataGroup)
        {
            EditorGUILayout.PropertyField(ReplaceFrom);
            EditorGUILayout.PropertyField(ReplaceTo);
            EditorGUILayout.PropertyField(replaceEvent);
            EditorGUILayout.PropertyField(addedTag);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
