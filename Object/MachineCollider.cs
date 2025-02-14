using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    [Header("剧情标题和描述")]
    [TextArea(5, 100)][SerializeField] private string messageTitleText;
    [TextArea(5, 100)][SerializeField] private string messageDisCribeText;
    public static string messageTitleString;
    public static string messageDisCribeString;

    public GameObject tipsImg;
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    private bool pressEKey;

    private void Start()
    {
        tipsImg.SetActive(false);
        canvasGroup = tipsImg.GetComponent<CanvasGroup>();

        // 初始时隐藏提示图片
        canvasGroup.alpha = 0f;
        tipsImg.SetActive(true); // 确保GameObject是激活的，以便可以渐变显示

        messageTitleString = messageTitleText;
        messageDisCribeString = messageDisCribeText;
    }

    private void Update()
    {
        if (!pressEKey)
        {
            pressEKey = InputManager.GetInstance().TextInteraction;
            //Debug.Log("isOneDoorOpen: " + pressEKey);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FadeIn(tipsImg, canvasGroup, fadeInTime)); // 开始渐变显示
            if (pressEKey)
            {
                // 检查场景中是否存在"MessageUI"或"MessageUI(Clone)"物体
                GameObject messageUI = GameObject.Find("MessageUI") ?? GameObject.Find("MessageUI(Clone)");

                // 如果物体不存在，则打开剧情面板
                if (messageUI == null)
                {
                    Debug.Log("打开剧情面板！");
                    FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().openMschine);
                    UIManager.GetInstance().FindPanel("MessageUI");
                    GameManager.GetInstance().ispause = true;
                }
                else
                {
                    // 如果物体存在，可以在这里添加其他逻辑，例如显示已经存在的面板
                    Debug.Log("剧情面板已经存在！");
                }

                pressEKey = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (pressEKey)
            {
                // 检查场景中是否存在"MessageUI"或"MessageUI(Clone)"物体
                GameObject messageUI = GameObject.Find("MessageUI") ?? GameObject.Find("MessageUI(Clone)");

                // 如果物体不存在，则打开剧情面板
                if (messageUI == null)
                {
                    Debug.Log("打开剧情面板！");
                    FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().openMschine);
                    UIManager.GetInstance().FindPanel("MessageUI");
                    GameManager.GetInstance().ispause = true;
                }
                else
                {
                    // 如果物体存在，可以在这里添加其他逻辑，例如显示已经存在的面板
                    Debug.Log("剧情面板已经存在！");
                }

                pressEKey = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 在这里确保tipsImg是激活的
            if (tipsImg.activeSelf)
            {
                StartCoroutine(FadeOut(tipsImg, canvasGroup, fadeOutTime)); // 开始渐变隐藏
            }
            pressEKey = false;
        }
    }

    private IEnumerator FadeIn(GameObject obj, CanvasGroup canvasGroup, float duration)
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

    private IEnumerator FadeOut(GameObject obj, CanvasGroup canvasGroup, float duration)
    {
        if (InputManager.GetInstance().RestartGame)
        {
            yield break;
        }
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (!obj.activeSelf)
            {
                Debug.LogWarning("淡出协程停止，因为'tipsImg'不再活动。");
                yield break; // 退出协程
            }
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
