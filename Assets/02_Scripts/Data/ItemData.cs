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

    //데이터 테이블 정보
    public int Id;
    public string Name;
    public int Grade;
    public ItemType Type;
    public string IconSprite;
}
