using System.Collections.Generic;
using UnityEngine;

/* ====================================
    Each scriptable object you make with this class
    will be an event. Use the created object as a
    reference for the GameEventListener.cs script to
    hook them up
   ==================================== 
*/ 

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> listeners = new List<GameEventListener>();

    //Called from an external script (ex: Health.cs)
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    { listeners.Add(listener); }
    
    public void UnregisterListener(GameEventListener listener)
    { listeners.Remove(listener); }
}
