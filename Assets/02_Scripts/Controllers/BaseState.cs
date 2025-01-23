using UnityEngine;

public abstract class BaseState
{
    protected Player _player;

    protected BaseState(Player player)
    {
        _player = player;
    }

    //상태 진입
    public abstract void OnStateEnter();
    //상태 업데이트
    public abstract void OnStateUpdate();
    //상태 탈출
    public abstract void OnStateExit();

    public virtual void OnStateFixedUpdate() { }
}
