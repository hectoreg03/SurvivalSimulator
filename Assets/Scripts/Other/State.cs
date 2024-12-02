using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    private State nextState;

    public State(State nextState)
    {
        this.nextState = nextState;
    }

    public void run()
    {
        
    }
    
    
}
