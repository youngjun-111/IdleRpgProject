using UnityEngine;

public abstract class BaseState
{
    protected Player _player;

    protected BaseState(Player player)
    {
        _player = player;
    }

    //���� ����
    public abstract void OnStateEnter();
    //���� ������Ʈ
    public abstract void OnStateUpdate();
    //���� Ż��
    public abstract void OnStateExit();

    public virtual void OnStateFixedUpdate() { }
}
