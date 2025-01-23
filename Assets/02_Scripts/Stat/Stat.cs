using UnityEngine;
using System.Collections.Generic;

public class Stat
{
    protected int _hp;

    protected int _maxHp;

    protected int _attackPower;

    protected int _defanse;

    protected int _attackSpeed;

    protected float _moveSpeed;

    public virtual int Hp { get { return _hp; } set { value = _hp; } }

    public virtual int MaxHp { get { return _maxHp; } set { value = _maxHp; } }

    public virtual int AttackSpeed { get { return _attackSpeed; } set { value = _attackSpeed; } }


}
