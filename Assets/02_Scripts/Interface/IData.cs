using UnityEngine;
using System.Linq;
public interface IData
{
    //�ʱ�ȭ
    public void SetDefaultData();
    //���̺�
    public void SaveData();
    //�ε�
    public bool LoadData();

}
