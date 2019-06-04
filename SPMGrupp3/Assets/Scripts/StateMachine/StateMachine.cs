//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State[] states;
    private Dictionary<Type, State> allStates = new Dictionary<Type, State>();
    [SerializeField] private State currentState;
    [HideInInspector] public State lastState;

    protected virtual void Awake()
    {
        foreach(State s in states)
        {
            State state = Instantiate(s);
            state.Initialize(this);
            allStates.Add(state.GetType(), state);
            if (currentState == null)
            {
                currentState = state;
            }
                
        }
        currentState.Enter();
    }

    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
        
    }

    public void Transition<T>() where T: State
    {
        currentState.Leave();
        lastState = currentState;
        currentState = allStates[typeof(T)];
        currentState.Enter();
    }

    public State GetCurrentState()
    {
        return currentState;
    }
}
