using UnityEditor;
using UnityEngine;

public class EventDebugMenu : EditorWindow
{

    EventCardData evnt;
    string result = "";
    [MenuItem("Window/EventDebugMenu")]
    public static void Display()
    {
        GetWindow<EventDebugMenu>("Event Debugmenu");
    }
    private void OnGUI()
    {
        evnt = EditorGUILayout.ObjectField("", evnt, typeof(EventCardData), true) as EventCardData;

        if (!Application.isPlaying) { return; }
        if(GUILayout.Button("Trigger Event"))
        {
            Debug.Log("TriggeringEvent");
            if (evnt != null)
            {
                if (!Deck.Instance.eventVisible&&!Deck.Instance.inBattle)
                {
                    evnt.Trigger();
                    result = "Triggered Event Successfully";
                }
                else {
                    result = "Error already in battle or event";
                }
            }
            else
            {

                result = "Error No Event Given";
            }
            
        }

        GUILayout.Label(result, EditorStyles.boldLabel);
    }

}
