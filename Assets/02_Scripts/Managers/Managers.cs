using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성 보장
    // 유일한 매니저를 가져옴 // 프로퍼티 // 읽기 전용
    static Managers Instance { get { Init(); return s_instance; } }

    #region Contents
    GameManager _game = new GameManager();
    
    public static GameManager Game { get { return Instance._game; } }
    
    #endregion

    #region Core
    ResourceManager _resourece = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    PoolManager _pool = new PoolManager();
    //SoundManager _sound = new SoundManager();

    public static ResourceManager Resource { get { return Instance._resourece; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static PoolManager Pool { get { return Instance._pool; } }

    //public static SoundManager Sound { get { return Instance._sound; } }
    #endregion

    private void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
        }

    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)  // go가 없으면
            {
                go = new GameObject { name = "@Managers" }; // 코드상으로 오브젝트를 만들어줌
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            //s_instance._sound.Init();
            s_instance._pool.Init();
            s_instance._ui.Init();
            s_instance._scene.Init();
            
        }
    }

    public static void Clear()
    {
        UI.Clear();
        Scene.Clear();
        Pool.Clear();

        //Sound.Clear();
    }
}

