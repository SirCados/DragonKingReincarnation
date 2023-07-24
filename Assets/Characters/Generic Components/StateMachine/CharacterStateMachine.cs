using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    protected State _currentState;
    protected State _nextState;

    public void StateMachineUpdate()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();

            if (_currentState.ShouldStateChange)
            {
                ChangeState(_nextState);
            }
        }
    }

    public void ChangeState(State newState)
    {
        if(_currentState != null)
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
