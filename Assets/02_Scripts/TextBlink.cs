using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBlink : MonoBehaviour
{
    [SerializeField]
    float fadeTime;//페이드 되는 시간
    TextMeshProUGUI fadeText;//페이드 효과에 사용되는 Image UI

    private void Awake()
    {
        fadeText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        //Fade 효과를 In -> out 무한 반복한다
        StartCoroutine("FadeInOut");
    }

    private void OnDisable()
    {
        StopCoroutine("FadeInOut");
    }

    IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0)); //Fade In

            yield return StartCoroutine(Fade(0, 1)); //Fade Out

        }
    }

    IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = fadeText.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeText.color = color;

            yield return null;
        }
    }
}
