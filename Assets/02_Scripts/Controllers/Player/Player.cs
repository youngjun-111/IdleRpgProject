using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;

public enum PlayerState
{
    Idle,
    Move,
    Attack,
    Hit,
    Die,
}

public class Player : MonoBehaviour, IDamageable
{
    public PlayerState _curState;

    FSM _pFsm;

    //ÄÄÆ÷³ÍÆ®
    [HideInInspector]
    public CharacterController _cc;

    private void Awake()
    {
        _curState = PlayerState.Idle;

        _pFsm = new FSM(new PlayerIdleState(this));

        _cc = gameObject.GetOrAddComponent<CharacterController>();
    }

    protected virtual void ChangeState()
    {
        switch (_curState)
        {
            case PlayerState.Idle:

                break;
            case PlayerState.Move:
                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Hit:
                break;
            case PlayerState.Die:
                break;
        }
    }

    public void Damaged(int amount)
    {

    }
}
