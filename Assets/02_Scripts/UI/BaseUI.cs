using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseUIData 
{
    public Action OnShow;
    public Action OnClose;
}

public class BaseUI : MonoBehaviour, IPointerDownHandler
{
    public Button _exitBtn;
    private Action _OnShow;
    private Action _OnClose;
    public bool _isSort = true;
    bool alreadySet = false;
    //public Animation _UIOpenAnimation;
    public int _btnCount;

    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    #region Bind구현부
    // 컴퍼넌트에 연결해줄 함수 형태
    protected void Bind<T>(Type type) where T : UnityEngine.Object    // Type 쓰려면 using System;
    {
        if (_objects.ContainsKey(typeof(T)))//이미 바인딩 되어있으면 리턴
            return;
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // 게임 오브젝트용 전용 버전을 하나 더 만들어줌
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            // 잘 찾아주고 있는지 테스트
            if (objects[i] == null)
            {
                Debug.Log($"Failed to bind({names[i]})");
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)  // 값이 없으면 그냥 리턴
            return null;

        return objects[idx] as T;   // 오브젝츠에다가 인덱스 번호를 추출한 다음에 T로 캐스팅 해줌
    }


    // 자주 사용하는 것들은 Get<T> 를 이용하지 않고 바로 사용할 수 있게 만들어 두자
    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }
    protected RawImage GetRawImage(int idx) { return Get<RawImage>(idx); }
    #endregion

    #region 클릭시 최상위로 변경
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isSort && Managers.UI.GetCurrentFrontUI() != this )
        {
            Managers.UI.SetCurrentUI(this);
        }
    }

    public virtual void Init(Transform anchor) {
        Logger.Log($"{GetType()} init");
        _exitBtn = Util.FindChild<Button>(gameObject, "ExitBtn", true);
        _OnShow = null;
        _OnClose = null;

        transform.SetParent(anchor);

        var rectTransform = GetComponent<RectTransform>();

        if (!rectTransform) {
            Logger.LogError("UI에 RectTrancform이 없음");
            return;
        }
        if (_exitBtn != null) {
            _exitBtn.onClick.AddListener(()=>CloseUI());
        }
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
    }

    public virtual void SetInfo(BaseUIData uiData) {
        Logger.Log($"{GetType()} Set info");

        _OnShow = uiData.OnShow;
        _OnClose = uiData.OnClose;

        // 버튼 사운드 설정
        SetButtonSounds();
    }

    protected virtual void SetButtonSounds()
    {
        if (alreadySet) return;

        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            // 마우스 오버 소리 설정
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            //entry.callback.AddListener((data) => { Managers.Sound.Play("정해야하는 부분"); });
            trigger.triggers.Add(entry);

            // 클릭 시 소리 설정
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((data) =>
            {
                if (button.interactable)
                {
                    //Managers.Sound.Play("정해야하는 부분");
                }
                else
                {
                    //Managers.Sound.Play("정해야하는 부분");
                }
            });
            trigger.triggers.Add(clickEntry);
        }

        alreadySet = true;
    }

    public virtual void ShowUI() {

        //애니메이션 구현할때 사용
        //if (_UIOpenAnimation) {
        //    _UIOpenAnimation.Play();
        //}

        _OnShow?.Invoke();

        _OnShow = null;
    }

    public virtual void CloseUI(bool isCloseAll = false) {
        //isCloseAll : 씬을 전환하거나 할 때 열려있는화면을
        //전부 다 닫아줄 필요가 있을 때
        //true를 넘겨줘서 화면을 닫을 때 필요한 처리들을
        //다 무시하고 화면만 닫아주기 위해서 사용할것

        if (!isCloseAll) {
            _OnClose?.Invoke();
        }
        _OnClose = null;

        Managers.UI.CloseUI(this);
    }

    public void OnClosedButton() {
        CloseUI();
    }
}

#endregion