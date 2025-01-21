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
        // Ÿ������ ������Ʈ�� ã��
        Object obj = Object.FindFirstObjectByType<EventSystem>();

        // ������ ����
        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    // ���⼭ �������� ���� ���̶� abstract�� ����
    public abstract void Clear();

    public void OnSceneChange(string sceneName)
    {
        Managers.Scene.SceneChange(sceneName);

        Managers.UI.CloseAllOpenUI();
    }
}
