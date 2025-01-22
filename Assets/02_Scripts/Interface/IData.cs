using UnityEngine;
using System.Linq;
public interface IData
{
    //초기화
    public void SetDefaultData();
    //세이브
    public void SaveData();
    //로드
    public bool LoadData();

}
