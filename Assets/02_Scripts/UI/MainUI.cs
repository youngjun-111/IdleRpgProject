using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainUI : BaseUI
{
    enum Text
    {
        Gold,
    }

    enum SkillSlot
    {
        Skill_0,
        Skill_1,
        Skill_2,
        Skill_3,
        Skill_4,
    }

    enum Buttons
    {
        Inventory,
        Skill,
        Pickup,
        Option,
    }

    private void Start()
    {
        //GetButton((int)Buttons.Inventory).onClick.AddListener(() => OpenSelectUI<InventoryUI>());
        //GetButton((int)Buttons.Skill).onClick.AddListener(() => OpenSelectUI<SkillUI>());
        //GetButton((int)Buttons.Pickup).onClick.AddListener(() => OpenSelectUI<PickupUI>());
        GetButton((int)Buttons.Option).onClick.AddListener(() => OpenSelectUI<OptionUI>());
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        Bind<Button>(typeof(Buttons));
        //스킬 바인드 해야함
        //Bind<Skills>(typeof(SkillSlot));
    }

    public void OpenSelectUI<T>() where T : BaseUI
    {
        T selectUI = Managers.UI.GetActiveUI<T>() as T;
        if(selectUI != null)
        {
            Managers.UI.CloseUI(selectUI);
        }
        else
        {
            Managers.UI.OpenUI<T>(new BaseUIData());
        }
    }

}
