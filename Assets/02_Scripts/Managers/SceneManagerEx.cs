using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    // 씬 전환 시 목표로 하는 씬
    public Define.Scene _targetScene;

    // 현재 씬 반환하는 메서드
    public BaseScene CurrentScene
    {
        get { return GameObject.FindFirstObjectByType<BaseScene>(); }
    }

    // OnSceneLoaded를 이벤트에 등록
    public void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 씬이 로드될 때마다 OnSceneLoaded 실행
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (Managers.Game._player == null) return;
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    // 현재 씬이 무엇인지를 bool값으로 반환하는 메서드
    public bool LoadingSceneCheck()
    {
        return SceneManager.GetActiveScene().buildIndex == (int)Define.Scene.Loading;
    }

    public void LoadScene(Define.Scene type)
    {
        Time.timeScale = 1f;
        Clear();
        SceneManager.LoadScene((int)type);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public AsyncOperation LoadSceneAsync(Define.Scene sceneType)
    {
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync((int)sceneType);
    }

    // 로딩 씬을 로드하기 위한 메서드
    public void SceneChange(string sceneName)
    {
        try
        {
            Define.Scene targetScene = (Define.Scene)Enum.Parse(typeof(Define.Scene), sceneName, true);
            _targetScene = targetScene;
            LoadScene(Define.Scene.Loading);
        }
        catch (Exception e)
        {
            Logger.LogError(e + "씬 없음, 지정한 씬 이름을 다시 확인");
        }
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
