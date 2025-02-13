using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [Header("��ɫת������")] public GameObject blackMask;
    [Header("��ɫת������CanvasGroup")] public CanvasGroup blackMaskCanvasGroup;

    private static ScenesManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // ȷ�� blackMask �����ݻ�
        DontDestroyOnLoad(blackMask);

        // ��ʼ�� blackMaskCanvasGroup
        if (blackMask)
        {
            if (blackMaskCanvasGroup == null)
            {
                blackMaskCanvasGroup = blackMask.AddComponent<CanvasGroup>();
            }
            blackMaskCanvasGroup.alpha = 0f; // ��ʼʱ��ȫ͸��
        }
    }

    private void Update()
    {
        if (InputManager.GetInstance().RestartGame)
        {
            StopAllCoroutines();
            ReloadNowScenes();
        }
    }

    /// <summary>
    /// ��ȡ ScenesManager ��ʵ������
    /// </summary>
    /// <returns>ScenesManager ʵ��</returns>
    public static ScenesManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("δ�ҵ� ScenesManager ������");
            return null;
        }
        return instance;
    }

    /// <summary>
    /// �������뵱ǰ����
    /// </summary>
    public void ReloadNowScenes()
    {
        // ��������Collider���
        DisableColliders();

        // ִ�г����л��ͺ�ɫ����ת��Ч��
        ScenesManager.GetInstance().TransitionToScene(SceneManager.GetActiveScene().name, 0.8f, 0.3f, () =>
        {
            // ����������ɺ���������Collider���
            EnableColliders();
        });
    }

    private void DisableColliders()
    {
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false; // ������ײ
        }
    }

    public void EnableColliders()
    {
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true; // ����������ײ
        }
    }

    /// <summary>
    /// �л�������ִ�к�ɫ����ת��Ч��
    /// </summary>
    /// <param name="sceneName">Ҫ���صĳ�������</param>
    /// <param name="fadeInDuration">���䵽ȫ��Ч���ĳ���ʱ�䣨�룩</param>
    /// <param name="fadeOutDuration">���䵽͸��Ч���ĳ���ʱ�䣨�룩</param>
    public void TransitionToScene(string sceneName, float fadeInDuration, float fadeOutDuration, System.Action onSceneLoaded)
    {
        StartCoroutine(DoTransition(sceneName, fadeInDuration, fadeOutDuration, onSceneLoaded));
    }

    private IEnumerator DoTransition(string sceneName, float fadeInDuration, float fadeOutDuration, System.Action onSceneLoaded)
    {
        // ȷ��blackMask�Ǽ����
        blackMask.SetActive(true);
        // ���㽥�䵽ȫ�ڵ�alpha�仯�ٶ�
        float fadeInSpeed = 1f / fadeInDuration;
        // ���䵽ȫ��
        while (blackMaskCanvasGroup.alpha < 1f)
        {
            blackMaskCanvasGroup.alpha += fadeInSpeed * Time.deltaTime;
            yield return null;
        }
        blackMaskCanvasGroup.alpha = 1f;    // ȷ��alphaֵΪ1��ȫ�ڣ�
        yield return Time.timeScale = 0f;
        // �����³���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        yield return Time.timeScale = 1f;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // ���㽥�䵽͸����alpha�仯�ٶ�
        float fadeOutSpeed = 1f / fadeOutDuration;
        // ���䵽͸��
        while (blackMaskCanvasGroup.alpha > 0f)
        {
            blackMaskCanvasGroup.alpha -= fadeOutSpeed * Time.deltaTime;
            yield return null;
        }
        // ȷ��alphaֵΪ0����ȫ͸����
        blackMaskCanvasGroup.alpha = 0f;

        // ����������ɺ�ִ�лص�����
        onSceneLoaded?.Invoke();
        // ת�������󣬽���blackMask
        blackMask.SetActive(false);
    }
}
