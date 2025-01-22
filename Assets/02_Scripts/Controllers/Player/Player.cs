using Unity.VisualScripting;
using UnityEngine;

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
    public PlayerState _pState;

    //������Ʈ
    [HideInInspector]
    public CharacterController _cc;

    private void Awake()
    {
        _pState = PlayerState.Idle;
        _cc = gameObject.GetOrAddComponent<CharacterController>();
    }




    public void Damaged(int amount)
    {
        //���� �� �´����̸� ����
        if (_pState == PlayerState.Attack) { return; } else if (_pState == PlayerState.Hit) { return; }

        _pState = PlayerState.Hit;

        if (amount <= 0)
        {
            _pState = PlayerState.Die;
        }
    }
}
