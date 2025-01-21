using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        // 타입으로 오브젝트를 찾고
        Object obj = Object.FindFirstObjectByType<EventSystem>();

        // 없으면 생성
        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    // 여기서 정의하지 않을 것이라 abstract로 제작
    public abstract void Clear();

    public void OnSceneChange(string sceneName)
    {
        Managers.Scene.SceneChange(sceneName);

        Managers.UI.CloseAllOpenUI();
    }
}
