using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class UIManager
{
    int _order = 10;    // 혹시 모르니깐 여유를 두고 먼저 생성할게 있다면 10보다 작은 수로 팝업할 수 있게함
    GameObject _root = null;
    public GameObject Root
    {
        get
        {
            if (_root == null)
            {
                _root = GameObject.Find("@UI_Root");
                if (_root == null)
                {
                    _root = new GameObject { name = "@UI_Root" };
                }
                _root = new GameObject { name = "@UI_Root" };
            }
            return _root;
        }
    }
    public Transform _uiCanvasTrs;//UI화면을 랜더링할 캐너ㅡ 컴포넌트 트랜스폼
    //UI 화면을 이 UI 캔버스 트랜스폼 하위에 위치시켜주어야 하기 때문에 필요함

    public Transform _closeUITrs;//UI 화면을 닫을 때 비활성화 시킨 UI 화면들을 위치시켜줄 트랜스폼

    private BaseUI _frontUI; //UI화면에 열려있는 가장 상단에 열려있는 UI
    //활성화된 UI
    private Dictionary<Type, GameObject> _OpenUIPool = new Dictionary<Type, GameObject>();
    //비활성화된 UI
    private Dictionary<Type, GameObject> _CloseUIPool = new Dictionary<Type, GameObject>();
    //UI 화면이 열려있는지 닫혀있는지 구분이 필요하기 때문에 UI 풀을 2개의 변수로 관리

    LinkedList<BaseUI> _sortingList = new LinkedList<BaseUI>();
    //표기 우선순위를 변경하기위한 링크드 리스트
    public void Init()
    {
        if (_uiCanvasTrs == null)
        {
            // 풀링을 할 오브젝트가 있다면 @Pool_Root 산하에 들고 있게 할 예정
            _uiCanvasTrs = new GameObject { name = "@UI_Root" }.transform;
            UnityEngine.Object.DontDestroyOnLoad(_uiCanvasTrs);
        }
        if (_closeUITrs == null)
        {
            // 풀링을 할 오브젝트가 있다면 @Pool_Root 산하에 들고 있게 할 예정
            _closeUITrs = new GameObject { name = "@CloseUI_Root" }.transform;
            _closeUITrs.SetParent(_uiCanvasTrs);
        }
    }

    //열기를 원하는 UI화면의 인스턴스를 가져오는함수
    private BaseUI GetUI<T>(out bool isAlreadyOpen, bool isNew = false)
    {
        System.Type uiType = typeof(T);

        BaseUI ui = null;
        isAlreadyOpen = false;

        if (!isNew&&_OpenUIPool.ContainsKey(uiType) )//활성화된 UI면
        {
            ui = _OpenUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        else if (!isNew && _CloseUIPool.ContainsKey(uiType))//비활성화된 UI면
        {

            ui = _CloseUIPool[uiType].GetComponent<BaseUI>();
            _CloseUIPool.Remove(uiType);
        }
        else
        { //한번도 생성된 적이 없는 UI 면
            GameObject uiObj = Managers.Resource.Instantiate($"UI/{uiType}");
            //프리팹의 이름이 클래스명이랑 같아야함
            //클래스명을 기준으로 참조하기 때문
            ui = uiObj.GetComponent<BaseUI>();
        }
        return ui;
    }
    //UI의 프리팹을 새로 생성하고 초기화하는 함수 이미 열려있으면 null리턴
    public T OpenUI<T>(BaseUIData uidata,bool sort=true,bool isNew =false) where T : BaseUI
    {

        System.Type uiType = typeof(T);

        Logger.Log($"{GetType()}::OpneUI({uiType})");

        bool isAlreadyOpen = false;


        var ui = GetUI<T>(out isAlreadyOpen, isNew);
        if (!ui)
        {
            Logger.Log($"{uiType} dose not exist");
            return null;
        }

        if (isAlreadyOpen)//이미 열려있으면 비정상적인 요청이라고 로그
        {
            Logger.Log($"{uiType} is Already Open ");
            return null;
        }


        var siblingIdx = _uiCanvasTrs.childCount - 1;//하위 오브젝트 개수
           

        //ui.transform.SetSiblingIndex(siblingIdx);
        //하이어라키 창 우선순위변경

        ui.gameObject.SetActive(true);
        ui.Init(_uiCanvasTrs);//화면 초기화
        ui.SetInfo(uidata);
        
        ui.ShowUI();
        _OpenUIPool[uiType] = ui.gameObject;
        SetCurrentUI(ui, sort); //소팅 우선순위 변경
        return ui as T;
    }
    //매개변수로 받은 UI를 닫고 비활성화 하는 함수
    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();
        Logger.Log($"{GetType()}::CloseUI({uiType})");

        ui.gameObject.SetActive(false);
        
        _OpenUIPool.Remove(uiType);
        _CloseUIPool[uiType] = ui.gameObject;
        
        _sortingList.Remove(ui);
        _frontUI = null;

        if (_sortingList.Count > 0)
        {
            var lastChild = _sortingList.Last.Value;
            _frontUI = lastChild;
        }
        else {
            _order = 10;
        }
        ui.transform.SetParent(_closeUITrs);
    }
    //비활성화해서 재사용하지 않는 UI의 경우 영구적으로 종료시키는 함수
    public void DeleteUI(BaseUI ui) {
        ui.CloseUI();
        UnityEngine.Object.Destroy(ui.gameObject);
    }
    //특정 UI화면이 열려있는지 확인하고 그 열려있는 UI화면을 가져오는 함수
    public BaseUI GetActiveUI<T>() 
    {
        var uiType = typeof(T);
        //_OpenUIPool에 특정 화면 인스턴스가 존재한다면 그 화면 인스턴스를 리턴해 주고 그렇지 않으면 널 리턴
        return _OpenUIPool.ContainsKey(uiType) ? _OpenUIPool[uiType].GetComponent<BaseUI>() : null;
    }
    //특정 UI화면이 열려있는지 확인
    public bool IsActiveUI<T>() 
    {
        var uiType = typeof(T);
        return _OpenUIPool.ContainsKey(uiType);
    }

    // 닫혀있는 UI중에 특정한 UI 참조용
    public BaseUI IsClosedUI<T>()
    {
        var uiType = typeof(T);
        return _CloseUIPool.ContainsKey(uiType) ? _CloseUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    //UI화면이 열린것이 하나라도 있는지 확인하는 함수
    public bool ExistsOpenUI()
    {
        return _frontUI != null; //_FrontUI가 null인지 아닌지 확인해서 bool값을 반환
    }

    //현재 가장 최상단에 있는 인스턴스를 리턴하는 함수

    public BaseUI GetCurrentFrontUI()
    {
        return _frontUI;
    }

    //가장 최상단에 있는 UI화면 인스턴스를 닫는 함수
    public void CloseCurrFrontUI(bool delete=false)
    {
        if (_frontUI == null) { return; }
        if (delete)
        {
            DeleteUI(_frontUI);
        }
        else {
            _frontUI.CloseUI();
        }
    }

    //열려있는 모든 UI화면을 닫으라는 함수

    public void CloseAllOpenUI()
    {
        while (_frontUI)
        {
            _frontUI.CloseUI(true);
        }
    }
    public void SetCurrentUI(BaseUI ui, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(ui.gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // 캔버스 안에 캔버스가 중첩해서 있을 때 그 부모가 어떤 값을 가지던 자신은 무조건 내 sorting order를 가진다
        // overrideSorting을 통해 혹시라도 중첩 캔버스라 자식 캔버스가 있더라도 부모 캔버스가 어떤 값을 가지던
        // 자신은 자신의 오더값을 가지려 할 때 true;
        canvas.overrideSorting = true;
        if (_sortingList.Contains(ui)) {
            _sortingList.Remove(ui);
        }
        
        if (sort)
        {
            _sortingList.AddLast(ui);
            _frontUI = ui;
            canvas.sortingOrder = _order;
            _order++;
        }
        else    //우선순위로 관리되지 않는 UI
        {
            ui._isSort = sort;
        }
    }

    //씬을 넘어갈 때 처리해줘야 하는 부분 작성
    public void Clear()
    {
        
    }
}
