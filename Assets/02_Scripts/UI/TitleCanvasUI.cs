using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleCanvasUI : BaseUI
{
    enum Buttons
    {
        StartBtn,
        QuitBtn,
    }

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
    }

    protected virtual void OnEnable()
    {
        GetButton((int)Buttons.StartBtn).onClick.AddListener(()=>
        {
            OnClickStartBtn();
        });
        GetButton((int)Buttons.QuitBtn).onClick.AddListener(() =>
        {
            OnApplicationQuit();
        });
    }

    protected virtual void OnDisable()
    {
        GetButton((int)Buttons.StartBtn).onClick.RemoveAllListeners();
        GetButton((int)Buttons.QuitBtn).onClick.RemoveAllListeners();
    }
    void OnClickStartBtn()
    {
        Managers.UI.CloseUI(this);

        Managers.Scene.LoadScene(Define.Scene.Loading);
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
