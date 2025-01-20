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
    //�񵿱� �ε��� �ε��ϱ� ���� ����
    AsyncOperation ao;

    Animator _fadeAnim;

    public void Start()
    {
        _fadeAnim = GameObject.FindWithTag("SceneManager").GetComponent<Animator>();
        StartCoroutine(GoNextScene(Managers.Scene._targetScene));
    }

    // �񵿱� ��
    IEnumerator GoNextScene(Define.Scene sceneType)
    {
        yield return null;

        // ������ ���� �񵿱� �������� �ε��Ѵ�
        ao = Managers.Scene.LoadSceneAsync(sceneType);

        // �غ� �Ϸ�Ǿ ���� ������ �Ѿ�� �ʰ��ϱ� ���� ó��
        ao.allowSceneActivation = false;

        // �ε��� �Ϸ�� ������ �ݺ��ؼ� ��ҵ��� �ε��ϰ� ���� ������ �ϸ鿡 ǥ���Ѵ�
        while (!ao.isDone)
        {
            // �ε� ������� �����̴� �ٿ� �ؽ�Ʈ�� ǥ���Ѵ�
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            _loadingBar.value = progress;
            _loadingTxt.text = (progress * 100f).ToString("F0") + "%";

            // ���� �� �ε� ������� 90%�� �Ѿ��
            if (ao.progress >= 0.9f)
            {
                _skipTxt.enabled = true;
                if (Input.anyKeyDown)
                {
                    //null ������ UI ESCŰ�� ���� �����ϰ� �� �ε�
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
