using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    [Tooltip("Ҫ��ʾ����ʾ")] public CanvasGroup TipsCanvasGroup;
    [Tooltip("�����ʱ��")] public float fadeDuration = 0.5f;

    private void Start()
    {
        TipsCanvasGroup.alpha = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeIn(fadeDuration));
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeOut(fadeDuration));
        }
    }

    IEnumerator FadeIn(float duration)
    {
        float startAlpha = TipsCanvasGroup.alpha;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            TipsCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1, t / duration);
            yield return null;
        }
        TipsCanvasGroup.alpha = 1; // ȷ��͸���ȴﵽ1
    }

    IEnumerator FadeOut(float duration)
    {
        float startAlpha = TipsCanvasGroup.alpha;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            TipsCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / duration);
            yield return null;
        }
        TipsCanvasGroup.alpha = 0; // ȷ��͸���ȴﵽ0
    }
}
