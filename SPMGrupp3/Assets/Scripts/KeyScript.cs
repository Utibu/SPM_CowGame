//Main Author: Sofia Kauko
//Secondary Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : Triggable
{
    private Collider keyCollider;
    [SerializeField] private AudioClip keyPickupSound;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        keyCollider = GetComponent<CapsuleCollider>();
    }

    public override void Update()
    {
        base.Update();
        //transform.RotateAroundLocal(Vector3.up, 0.2f);
        transform.Rotate(Vector3.up, 0.2f);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        // ge key = true till level manager? eller playerValues? sen: destroy
        if (keyPickupSound != null)
        {
            EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, keyPickupSound, 1f, 0.9f, 1.1f));
        }
        LevelManager.instance.hasGateKey = true;
        UIManager.instance.ShowKeyImage();
        Destroy(gameObject);
    }

    
}
