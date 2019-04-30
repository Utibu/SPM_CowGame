using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private Collider keyCollider;

    // Start is called before the first frame update
    void Start()
    {
        keyCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        

    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

        }
    }
    */
}
