using UnityEngine;
using UnityEngine.Events;

/* ====================================
    This script will be placed on the object 
    that you want to listen to an event
   ====================================
*/ 
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event; //The Scriptable Object GameEvent that you want to listen for
    public UnityEvent Response; //The functions that you want to execute when the event is raised

    void OnEnable()
    { Event.RegisterListener(this); }

    void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised() //Invokes the response functions specified in the inspector
    { Response.Invoke(); } 
}
