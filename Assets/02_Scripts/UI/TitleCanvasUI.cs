using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleCanvasUI : BaseUI
{
    enum Buttons
    {
        StartBtn,
    }

    public void Awake()
    {
        Bind<Button>(typeof(Buttons));
    }

    protected virtual void OnEnable()
    {
        GetButton((int)Buttons.StartBtn).onClick.AddListener(()=>
        {
            OnClickStartBtn();
        });
    }

    void OnClickStartBtn()
    {
        Managers.UI.CloseUI(this);

        Managers.Scene.LoadScene(Define.Scene.Loading);
    }
}
