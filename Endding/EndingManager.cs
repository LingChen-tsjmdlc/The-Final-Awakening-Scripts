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
        // 初始时隐藏所有UI元素
        SetActiveWithAlpha(Title1.gameObject, Title1CanvasGroup, false, 0f);
        SetActiveWithAlpha(Title2.gameObject, Title2CanvasGroup, false, 0f);
        SetActiveWithAlpha(Discribe1.gameObject, Discribe1CanvasGroup, false, 0f);
        SetActiveWithAlpha(Discribe2.gameObject, Discribe2CanvasGroup, false, 0f);
        SetActiveWithAlpha(backButton.gameObject, backButtonCanvasGroup, false, 0f);

        // 启动协程
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
        // 等待0.5秒
        yield return new WaitForSeconds(0.5f);

        // 渐变显示Title1并逐字显示
        SetActiveWithAlpha(Title1.gameObject, Title1CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Title1CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Title1, Title1.text, 0.05f)); // 逐字显示Title1文本
        yield return new WaitForSeconds(1f); // 等待1秒

        // 渐变显示Title2并逐字显示
        SetActiveWithAlpha(Title2.gameObject, Title2CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Title2CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Title2, Title2.text, 0.05f)); // 逐字显示Title2文本
        yield return new WaitForSeconds(1f); // 等待1秒

        // 渐变显示Discribe1并逐字显示
        SetActiveWithAlpha(Discribe1.gameObject, Discribe1CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Discribe1CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Discribe1, Discribe1.text, 0.05f)); // 逐字显示Discribe1文本
        yield return new WaitForSeconds(1f); // 等待1秒

        // 渐变显示Discribe2并逐字显示
        SetActiveWithAlpha(Discribe2.gameObject, Discribe2CanvasGroup, true, 0f);
        StartCoroutine(FadeIn(Discribe2CanvasGroup, 1f));
        yield return StartCoroutine(TypeText(Discribe2, Discribe2.text, 0.05f)); // 逐字显示Discribe2文本
        yield return new WaitForSeconds(1f); // 等待1秒

        // 最后渐变显示backButton
        SetActiveWithAlpha(backButton.gameObject, backButtonCanvasGroup, true, 0f);
        yield return StartCoroutine(FadeIn(backButtonCanvasGroup, 1f));
    }

    private IEnumerator TypeText(Text textComponent, string text, float typeSpeed)
    {
        textComponent.text = ""; // 清空文本
        foreach (char letter in text)
        {
            textComponent.text += letter; // 逐字添加
            yield return new WaitForSeconds(typeSpeed); // 等待一定时间
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
