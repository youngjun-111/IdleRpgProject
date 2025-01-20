using System;
using System.Collections.Generic;

public class DataManager
{
    public Dictionary<Type, IData> _IDataDict = new Dictionary<Type, IData>();

    public void DataInit()
    {
        DataTypes();
        Logger.Log("타입 체크");
    }

    void DataTypes()
    {
        //IData를 상속받고있는 타입 설정
        var dataType = typeof(IData);
        //Logger.Log($"{dataType.Name.ToString()}타입 입니다.");
        //실행중인 어셈블리 가져오고
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //어셈블리 내의 모든 타입을 탐색
        foreach (var assembly in assemblies)
        {
            //어셈블리 타입을 가져와서
            var types = assembly.GetTypes();
            //각 타입에서
            foreach (var type in types)
            {
                //IData 를 상속받는 클래스또는 추상클래스인지 확인
                if (dataType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                {
                    Logger.Log($"타입 찾기{type.Name.ToString()}");
                    //딕셔너리에 해당 타입의 인스턴스가 없을 경우
                    if (!_IDataDict.ContainsKey(type))
                    {
                        try
                        {
                            _IDataDict[type] = (IData)Activator.CreateInstance(type);
                            Logger.Log($"타입 인스턴스 생성 및 추가 성공: {type.Name.ToString()}");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError($"{type.Name} 인스턴스 생성 실패: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
    public void SaveData<T>() where T : class, IData
    {
        T dataToSave = GetData<T>();
        if (dataToSave != null)
        {
            dataToSave.SaveData();
        }
        else
        {
            Logger.LogWarning($"{typeof(T).Name} 저장할 데이터가 없음");
        }
    }

    public void LoadData<T>() where T : class, IData
    {
        T dataToLoad = GetData<T>();
        if (dataToLoad != null)
        {
            bool success = dataToLoad.LoadData();
            if (success)
            {
                Logger.Log($"{typeof(T).Name} 로드");
            }
            else
            {
                Logger.LogError($"{typeof(T).Name} 로드 실패");
            }
        }
        else
        {
            Logger.LogWarning($"{typeof(T).Name} 로드할 데이터가 없음");
        }
    }

    public T GetData<T>() where T : class, IData
    {
        Type type = typeof(T);

        if (_IDataDict.TryGetValue(type, out IData dataInstance))
        {
            return dataInstance as T;
        }
        Logger.LogError($"{type.Name}의 타입이 등록되지 않았습니다.");
        return null;
    }
}