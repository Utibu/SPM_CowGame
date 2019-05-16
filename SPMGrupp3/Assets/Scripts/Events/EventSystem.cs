using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
    {

    private static EventSystem __Current;
    public static EventSystem Current
    {
        get
        {
            if(__Current == null)
            {
                __Current = GameObject.FindObjectOfType<EventSystem>();
            }
            return __Current;
        }
    }

    delegate void EventListener(Event e);
    Dictionary<System.Type, HashSet<EventHandler>> eventListeners;

    public void RegisterListener<T>(System.Action<T> listener) where T : Event
    {
        System.Type eventType = typeof(T);
        var target = listener.Target;
        var method = listener.Method;

        if(eventListeners == null)
        {
            eventListeners = new Dictionary<System.Type, HashSet<EventHandler>>();
        }

        if(!eventListeners.ContainsKey(eventType) || eventListeners[eventType] == null)
        {
            eventListeners[eventType] = new HashSet<EventHandler>();
        }


        eventListeners[eventType].Add(new EventHandler { Target = target, Method = method });
    }

    public void UnregisterListener<T>(System.Action<T> listener) where T : Event
    {
        System.Type eventType = typeof(T);
        Debug.Log("---");
        Debug.Log("BEFORE: " + eventListeners[eventType].Count);
        if (eventListeners == null || !eventListeners.ContainsKey(eventType) || eventListeners[eventType] == null)
        {
            return;
        }
        Debug.Log("TARGET: " + listener.Target);
        Debug.Log("METHOD: " + listener.Method);
        Debug.Log("CURRENT: ----- ");
        eventListeners[eventType].RemoveWhere(item => item.Target == listener.Target && item.Method == listener.Method);
        Debug.Log("AFTER: " + eventListeners[eventType].Count);
    }

    public void FireEvent(Event eventInfo)
    {
        System.Type trueEventType = eventInfo.GetType();
        if (eventListeners == null || !eventListeners.ContainsKey(trueEventType) || eventListeners[trueEventType] == null)
        {
            return;
        }

        //Debug.LogWarning("LISTENERS: " + eventListeners[trueEventType].Count);
        foreach(EventHandler handler in eventListeners[trueEventType])
        {
            handler.Method.Invoke(handler.Target, new[] { eventInfo });
        // destroy event info?
        }
    }

    public void ClearListener<T>() where T : Event
    {
        System.Type eventType = typeof(T);
        /*if (eventListeners != null)
        {
            eventListeners.Clear();
        }*/
        if (eventListeners == null || !eventListeners.ContainsKey(eventType))
        {
            return;
        }

        eventListeners.Remove(eventType);
    }

    public class EventHandler
    {
        public object Target { get; set; }
        public System.Reflection.MethodInfo Method { get; set; }
    }
}

