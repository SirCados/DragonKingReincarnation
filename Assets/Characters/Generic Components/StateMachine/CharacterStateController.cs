using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    protected State _currentState;
    protected State _nextState;

    private void LateUpdate()
    {
        //print("current state " + _currentState);
        StateControllerUpdate();
    }

    public void StateControllerUpdate()
    {        
        if (_currentState != null)
        {            
            _currentState.OnUpdate();

            if (_nextState != null && _currentState.ShouldStateChange)
            {

                ChangeState(_nextState);
            }
        }
    }

    public void ChangeState(State newState)
    {
        if (_currentState != null)
        {
            _currentState.OnExitState();
        }

        _currentState = newState;
        _currentState.OnEnterState();
    }

    public void ChangeState(State newState, State nextState)
    {
        if (_currentState != null)
        {
            _currentState.OnExitState();
        }

        _currentState = newState;
        _nextState = nextState;
        _currentState.OnEnterState();
    }
}
