﻿//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saveable : MonoBehaviour
{
    [SerializeField] private float id;
    public float Id { get { return id; } set { id = value; } }

    private void Awake()
    {
        id = transform.position.sqrMagnitude;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
