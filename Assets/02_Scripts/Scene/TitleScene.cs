using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    private void Start()
    {
        OpenTitle();
    }

    public override void Clear()
    {
        
    }

    public void OpenTitle()
    {
        TitleCanvasUI titleCanvasUI = Managers.UI.GetActiveUI<TitleCanvasUI>() as TitleCanvasUI;

        if (titleCanvasUI == null )
        {
            Managers.UI.OpenUI<TitleCanvasUI>(new BaseUIData());
        }
    }
}
