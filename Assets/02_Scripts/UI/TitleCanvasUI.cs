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

    public void OnClickStartBtn()
    {
        Managers.UI.CloseUI(this);

        Managers.Scene.LoadScene(Define.Scene.Loading);
    }
}
