using UnityEngine;

public class FSM
{
    BaseState _curState;

    public FSM(BaseState currentState)
    {
        _curState = currentState;
        ChangeState(_curState);
    }

    public void ChangeState(BaseState nextState)
    {
        if (nextState == _curState)
            return;

        if (_curState != null)
            _curState.OnStateExit();

        _curState = nextState;
        _curState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (_curState != null)
            _curState.OnStateUpdate();
    }

    public void FixedUpdateState()
    {
        if (_curState != null)
            _curState.OnStateFixedUpdate();
    }
}
