using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public Text Title1;
    public Text Title2;
    public Text Discribe1;
    public Text Discribe2;

    public Button backButton;

    public CanvasGroup Title1CanvasGroup;
    public CanvasGroup Title2CanvasGroup;
    public CanvasGroup Discribe1CanvasGroup;
    public CanvasGroup Discribe2CanvasGroup;
    public CanvasGroup backButtonCanvasGroup;

    private void Start()
    {
        // ��ʼʱ��������UIԪ��
        SetActiveWithAlpha(Title1.gameObject, Title1CanvasGroup, false, 0f);
        SetActiveWithAlpha(Title2.gameObject, Title2CanvasGroup, false, 0f);
        SetActiveWithAlpha(Discribe1.gameObject, Discribe1CanvasGroup, false, 0f);
        SetActiveWithAlpha(Discribe2.gameObject, Discribe2CanvasGroup, false, 0f);
        SetActiveWithAlpha(backButton.gameObject, backButtonCanvasGroup, false, 0f);

        // ����Э��
        StartCoroutine(ShowEndingSequence());
    }

    private void SetActiveWithAlpha(GameObject obj, CanvasGroup canvasGroup, bool active, float alpha)
    {
        obj.SetActive(active);
        canvasGroup.alpha = alpha;
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator ShowEndingSequence()
    {
        // �ȴ�0.5��
        yield return new WaitForSeconds(0.5f);

        // ������ʾTitle1��������ʾ
        SetActiveWithAlpha(Title1.gameObject, Title1CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Title1CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Title1, Title1.text, 0.05f)); // ������ʾTitle1�ı�
        yield return new WaitForSeconds(1f); // �ȴ�1��

        // ������ʾTitle2��������ʾ
        SetActiveWithAlpha(Title2.gameObject, Title2CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Title2CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Title2, Title2.text, 0.05f)); // ������ʾTitle2�ı�
        yield return new WaitForSeconds(1f); // �ȴ�1��

        // ������ʾDiscribe1��������ʾ
        SetActiveWithAlpha(Discribe1.gameObject, Discribe1CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Discribe1CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Discribe1, Discribe1.text, 0.05f)); // ������ʾDiscribe1�ı�
        yield return new WaitForSeconds(1f); // �ȴ�1��

        // ������ʾDiscribe2��������ʾ
        SetActiveWithAlpha(Discribe2.gameObject, Discribe2CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Discribe2CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Discribe2, Discribe2.text, 0.05f)); // ������ʾDiscribe2�ı�
        yield return new WaitForSeconds(1f); // �ȴ�1��

        // ��󽥱���ʾbackButton
        SetActiveWithAlpha(backButton.gameObject, backButtonCanvasGroup, true, 0f);
        yield return StartCoroutine(FadeIn(backButtonCanvasGroup, 1f));
    }

    private IEnumerator TypeText(Text textComponent, string text, float typeSpeed)
    {
        textComponent.text = ""; // ����ı�
        foreach (char letter in text)
        {
            textComponent.text += letter; // �������
            yield return new WaitForSeconds(typeSpeed); // �ȴ�һ��ʱ��
        }
    }


    public void BackToTheStartUI()
    {
        ScenesManager.GetInstance().TransitionToScene("UIScenes", 0.4f, 0.6f, () =>
        {
            UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().buttonDownAudio);
            ScenesManager.GetInstance().EnableColliders();
            if (SceneManager.GetActiveScene().name == "UIScenes")
            {
                UIManager.GetInstance().FindPanel("StartUI");
            }
        });
    }
}
