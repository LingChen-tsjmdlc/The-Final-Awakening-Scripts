using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    [Header("������������")]
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

        // ��ʼʱ������ʾͼƬ
        canvasGroup.alpha = 0f;
        tipsImg.SetActive(true); // ȷ��GameObject�Ǽ���ģ��Ա���Խ�����ʾ

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
            StartCoroutine(FadeIn(tipsImg, canvasGroup, fadeInTime)); // ��ʼ������ʾ
            if (pressEKey)
            {
                // ��鳡�����Ƿ����"MessageUI"��"MessageUI(Clone)"����
                GameObject messageUI = GameObject.Find("MessageUI") ?? GameObject.Find("MessageUI(Clone)");

                // ������岻���ڣ���򿪾������
                if (messageUI == null)
                {
                    Debug.Log("�򿪾�����壡");
                    FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().openMschine);
                    UIManager.GetInstance().FindPanel("MessageUI");
                    GameManager.GetInstance().ispause = true;
                }
                else
                {
                    // ���������ڣ�������������������߼���������ʾ�Ѿ����ڵ����
                    Debug.Log("��������Ѿ����ڣ�");
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
                // ��鳡�����Ƿ����"MessageUI"��"MessageUI(Clone)"����
                GameObject messageUI = GameObject.Find("MessageUI") ?? GameObject.Find("MessageUI(Clone)");

                // ������岻���ڣ���򿪾������
                if (messageUI == null)
                {
                    Debug.Log("�򿪾�����壡");
                    FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().openMschine);
                    UIManager.GetInstance().FindPanel("MessageUI");
                    GameManager.GetInstance().ispause = true;
                }
                else
                {
                    // ���������ڣ�������������������߼���������ʾ�Ѿ����ڵ����
                    Debug.Log("��������Ѿ����ڣ�");
                }

                pressEKey = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ������ȷ��tipsImg�Ǽ����
            if (tipsImg.activeSelf)
            {
                StartCoroutine(FadeOut(tipsImg, canvasGroup, fadeOutTime)); // ��ʼ��������
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
                Debug.LogWarning("����Э��ֹͣ����Ϊ'tipsImg'���ٻ��");
                yield break; // �˳�Э��
            }
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
