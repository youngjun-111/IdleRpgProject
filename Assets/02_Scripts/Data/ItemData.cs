using UnityEngine;
using System.Collections.Generic;
using System;
public class ItemData
{
    [Serializable]
    public enum ItemType
    {
        Weapon,
        Armor,
    }

    //������ ���̺� ����
    public int Id;
    public string Name;
    public int Grade;
    public ItemType Type;
    public string IconSprite;
}
