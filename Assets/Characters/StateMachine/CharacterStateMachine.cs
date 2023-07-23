using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    State _currentState;

    public void StateMachineUpdate()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();
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
}
