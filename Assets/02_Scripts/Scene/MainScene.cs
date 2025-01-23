using UnityEngine;

public class MainScene : BaseScene
{
    public override void Clear()
    {

    }

    public void OpenMainUI()
    {
        var mainUI = Managers.UI.GetActiveUI<MainUI>();
    }

}
