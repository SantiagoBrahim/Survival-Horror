using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStates : MonoBehaviour
{
    public enum States {Idle, Wandering, Seeking, Chasing, Stunned, Fleeing, Attacking};
    public States actualState;

    private void Start()
    {
        actualState = States.Idle;
    }

    public void ChangeState(States newState)
    {
        actualState = newState;
    }
}
