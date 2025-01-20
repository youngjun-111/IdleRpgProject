using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    //  Resources.Load<T>(path); 대신 Load<T>(path)로 사용하기 위해 랩핑
    public T Load<T>(string path) where T : Object
    {
        // 1. original 이미 들고 있으면 바로 사용

        // 만약 프리팹인 경우 오피지널을 풀에서 한번 찾아서 그것을 바로 반환
        if (typeof(T) == typeof(GameObject))    // 이러면 프리팹일 확률이 높음
        {
            // 여기 로드에 Load<GameObject>($"Prefabs/{path}"); 이런 식으로
            // 전체 경로를 넘겨주었는데 풀은 그냥 최종적인 이름을 사요앟고 있으니
            // /(name)으로 되어 있으면 (name)만 사용해야 하니까
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
            {
                name = name.Substring(index + 1);   // 이래야 ('/') 다음 부터가 됨
            }

            // (name)을 찾아봣는데 있으면 이것을 반환
            GameObject go = Managers.Pool.GetOriginal(name);
            // 근데 없다면 예전처럼 그냥 return Resources.Load<T>(path);로 진행되게
            if (go != null)
            {
                return go as T;
            }
        }
        // 없다면 이전 Load처럼 사용
        return Resources.Load<T>(path);
    }
    
    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 경로를 랩핑해서 이후에는 Instantiate("경로안의 프리팹 이름")으로 해결 가능해짐

        // 1. original이 있으면 바로 사용, 없으면 아래처럼 사용
        GameObject original = Load<GameObject>($"Prefabs/{path}");  // 의미상 혼동될 수 있어서 변수명 수정

        if (original == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return null;
        }

        // 2. 혹시 풀링된 오브젝트가 있으면 그것을 반환
        if (original.GetComponent<Poolable>() != null)
        {
            return Managers.Pool.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        int index = go.name.IndexOf("(Clone)"); // Clone 문자열을 찾아서 인덱스를 반환
        if (index > 0 )
        {
            go.name = go.name.Substring(0, index);  // UI_Inven_Item//(Clone)
        }
        return go;

    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // 3. 만약에 풀링이 필요한 오브젝트라면 바로 삭제하는 것이 아니라 풀링 매니저한테 위탁
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
