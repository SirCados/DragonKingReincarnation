using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    protected State _currentState;

    public void StateControllerUpdate()
    {        
        if (_currentState != null)
        {            
            _currentState.OnUpdate();

            if (_currentState.NextState != null && _currentState.ShouldStateChange)
            {              
                
                ChangeState(_currentState.NextState);
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
        _currentState.ShouldStateChange = false;
        _currentState.OnEnterState();
    }
}
