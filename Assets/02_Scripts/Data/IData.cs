using UnityEngine;

public interface IData
{
    //초기화
    void SetDefaultData();
    //세이브
    void SaveData();
    //로드
    bool LoadData();

}
