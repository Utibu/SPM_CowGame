using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Dashable : MonoBehaviour
{
    [SerializeField] protected float requiredLevel = 1;
    [SerializeField] protected AudioClip[] destructionSounds;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if(GetComponent<Saveable>() != null)
        {
            GameManager.instance.SaveManager.Dashables.Add(GetComponent<Saveable>().Id, this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetClip()
    {
        if(destructionSounds.Length == 0)
        {
            return null;
        }
        else
        {
            return destructionSounds[Random.Range(0, destructionSounds.Length)];
        }
    }

    public virtual void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision, int dashLevel)
    {
        EventSystem.Current.FireEvent(new PlaySoundEvent(gameObject.transform.position, GetClip(), 1f, 0.8f, 1.1f));
        skipCollision = false;
    }

}
