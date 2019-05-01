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

    // iden här är att flytta ut pickup logiken till objektet självt. så den kan ta bort sig själv, spela eget pickup-ljud eller liknande

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

        }
    }
    */
}
