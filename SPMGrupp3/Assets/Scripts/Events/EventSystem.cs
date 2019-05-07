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
        Dictionary<System.Type, HashSet<EventListener>> eventListeners;

        public void RegisterListener<T>(System.Action<T> listener) where T : Event
        {
            System.Type eventType = typeof(T);
            if(eventListeners == null)
            {
                eventListeners = new Dictionary<System.Type, HashSet<EventListener>>();
            }

            if(!eventListeners.ContainsKey(eventType) || eventListeners[eventType] == null)
            {
                eventListeners[eventType] = new HashSet<EventListener>();
            }

            EventListener wrapper = (ei) => { listener((T)ei); };
            eventListeners[eventType].Add(wrapper);
        }

        public void UnregisterListener<T>(System.Action<T> listener) where T : Event
        {
            System.Type eventType = typeof(T);
            if (eventListeners == null || !eventListeners.ContainsKey(eventType) || eventListeners[eventType] == null)
            {
                return;
            }
            EventListener wrapper = (ei) => { listener((T)ei); };
            eventListeners[eventType].Remove(wrapper);
        }

        public void FireEvent(Event eventInfo)
        {
            System.Type trueEventType = eventInfo.GetType();
            if (eventListeners == null || !eventListeners.ContainsKey(trueEventType) || eventListeners[trueEventType] == null)
            {
                return;
            }

            Debug.LogWarning("LISTENERS: " + eventListeners[trueEventType].Count);
            foreach(EventListener el in eventListeners[trueEventType])
            {
                el(eventInfo);
            }
        }

        public void ClearListeners()
        {
            if(eventListeners != null)
            {
                eventListeners.Clear();
            }
        }
    }