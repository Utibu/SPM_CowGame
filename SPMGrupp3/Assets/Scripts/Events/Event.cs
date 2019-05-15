using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event
{
        
}

    public class DebugEvent: Event
    {
        public string eventDescription = "";
        public DebugEvent(string eventDescription)
        {
            this.eventDescription = eventDescription;
        }
    }

    public class DieEvent: DebugEvent
    {
        public GameObject gameObject;
        public GameObject explosionParticleSystemObject;
        public AudioClip dieAudioClip;

        public DieEvent(GameObject gameObject, GameObject explosionParticleSystemObject, AudioClip dieAudioClip, string eventDescription = "") : base(eventDescription)
        {
            this.gameObject = gameObject;
            this.explosionParticleSystemObject = explosionParticleSystemObject;
            this.dieAudioClip = dieAudioClip;
        }
    }

public class OnInteractionFinishedEvent: DebugEvent
{
    public GameObject gameObject;
    public OnInteractionFinishedEvent(GameObject gameObject, string eventDescription = "") : base(eventDescription)
    {
        this.gameObject = gameObject;
    }
}

public class HayEatingFinishedEvent: OnInteractionFinishedEvent
{
    public HayEatingFinishedEvent(GameObject gameObject, string eventDescription = "") : base(gameObject, eventDescription)
    {
    }
}

public class OnPlayerDiedEvent: DebugEvent
{
    public GameObject gameObject;
    public OnPlayerDiedEvent(GameObject gameObject, string eventDescription = "") : base(eventDescription)
    {
        this.gameObject = gameObject;
    }
}


public class PlaySoundEvent : Event
{
    public Vector3 position;
    public AudioClip sound;
    public float volume;
    public float pitchMin;
    public float pitchMax;

    public PlaySoundEvent(Vector3 position, AudioClip sound, float volume, float pitchMin, float pitchMax)
    {
        this.position = position;
        this.sound = sound;
        this.volume = volume;
        this.pitchMin = pitchMin;
        this.pitchMax = pitchMax;
    }

    
}


public class PauseEvent : DebugEvent
{
    public PauseEvent(string eventDescription) : base(eventDescription)
    {
        
    }
}

public class ResumeEvent : DebugEvent
{
    public ResumeEvent(string eventDescription) : base(eventDescription)
    {

    }
}

public class EnemyDieEvent : DebugEvent
{
    public GameObject enemy;
    public EnemyDieEvent(string eventDescription, GameObject enemy) : base(eventDescription)
    {
        this.enemy = enemy;
    }
}