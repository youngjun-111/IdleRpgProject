using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{
    public Slider _loadingBar;

    public TextMeshProUGUI _loadingTxt;
    public TextMeshProUGUI _skipTxt;
    //비동기 로딩을 로드하기 위한 선언
    AsyncOperation ao;

    Animator _fadeAnim;

    public void Start()
    {
        _fadeAnim = GameObject.FindWithTag("SceneManager").GetComponent<Animator>();
        StartCoroutine(GoNextScene(Managers.Scene._targetScene));
    }

    // 비동기 씬
    IEnumerator GoNextScene(Define.Scene sceneType)
    {
        yield return null;

        // 지정된 씬을 비동기 형식으로 로드한다
        ao = Managers.Scene.LoadSceneAsync(sceneType);

        // 준비가 완료되어도 다음 씬으로 넘어가지 않게하기 위한 처리
        ao.allowSceneActivation = false;

        // 로딩이 완료될 때까지 반복해서 요소들을 로드하고 진행 과정을 하면에 표시한다
        while (!ao.isDone)
        {
            // 로딩 진행률을 슬라이더 바와 텍스트로 표시한다
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            _loadingBar.value = progress;
            _loadingTxt.text = (progress * 100f).ToString("F0") + "%";

            // 만일 씬 로드 진행률이 90%를 넘어가면
            if (ao.progress >= 0.9f)
            {
                _skipTxt.enabled = true;
                if (Input.anyKeyDown)
                {
                    //null 방지용 UI ESC키로 전부 삭제하고 씬 로드
                    Managers.UI.CloseAllOpenUI();
                    _fadeAnim.SetTrigger("doFade");
                }
            }
            yield return null;
        }
    }
    public override void Clear()
    {

    }

}
