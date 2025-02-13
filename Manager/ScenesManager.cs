using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [Header("黑色转场遮罩")] public GameObject blackMask;
    [Header("黑色转场遮罩CanvasGroup")] public CanvasGroup blackMaskCanvasGroup;

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
        // 确保 blackMask 不被摧毁
        DontDestroyOnLoad(blackMask);

        // 初始化 blackMaskCanvasGroup
        if (blackMask)
        {
            if (blackMaskCanvasGroup == null)
            {
                blackMaskCanvasGroup = blackMask.AddComponent<CanvasGroup>();
            }
            blackMaskCanvasGroup.alpha = 0f; // 初始时完全透明
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
    /// 获取 ScenesManager 的实例对象
    /// </summary>
    /// <returns>ScenesManager 实例</returns>
    public static ScenesManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("未找到 ScenesManager 单例！");
            return null;
        }
        return instance;
    }

    /// <summary>
    /// 重新载入当前场景
    /// </summary>
    public void ReloadNowScenes()
    {
        // 禁用所有Collider组件
        DisableColliders();

        // 执行场景切换和黑色渐变转场效果
        ScenesManager.GetInstance().TransitionToScene(SceneManager.GetActiveScene().name, 0.8f, 0.3f, () =>
        {
            // 场景加载完成后，重新启用Collider组件
            EnableColliders();
        });
    }

    private void DisableColliders()
    {
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false; // 禁用碰撞
        }
    }

    public void EnableColliders()
    {
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true; // 重新启用碰撞
        }
    }

    /// <summary>
    /// 切换场景并执行黑色渐变转场效果
    /// </summary>
    /// <param name="sceneName">要加载的场景名称</param>
    /// <param name="fadeInDuration">渐变到全黑效果的持续时间（秒）</param>
    /// <param name="fadeOutDuration">渐变到透明效果的持续时间（秒）</param>
    public void TransitionToScene(string sceneName, float fadeInDuration, float fadeOutDuration, System.Action onSceneLoaded)
    {
        StartCoroutine(DoTransition(sceneName, fadeInDuration, fadeOutDuration, onSceneLoaded));
    }

    private IEnumerator DoTransition(string sceneName, float fadeInDuration, float fadeOutDuration, System.Action onSceneLoaded)
    {
        // 确保blackMask是激活的
        blackMask.SetActive(true);
        // 计算渐变到全黑的alpha变化速度
        float fadeInSpeed = 1f / fadeInDuration;
        // 渐变到全黑
        while (blackMaskCanvasGroup.alpha < 1f)
        {
            blackMaskCanvasGroup.alpha += fadeInSpeed * Time.deltaTime;
            yield return null;
        }
        blackMaskCanvasGroup.alpha = 1f;    // 确保alpha值为1（全黑）
        yield return Time.timeScale = 0f;
        // 加载新场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        yield return Time.timeScale = 1f;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // 计算渐变到透明的alpha变化速度
        float fadeOutSpeed = 1f / fadeOutDuration;
        // 渐变到透明
        while (blackMaskCanvasGroup.alpha > 0f)
        {
            blackMaskCanvasGroup.alpha -= fadeOutSpeed * Time.deltaTime;
            yield return null;
        }
        // 确保alpha值为0（完全透明）
        blackMaskCanvasGroup.alpha = 0f;

        // 场景加载完成后，执行回调函数
        onSceneLoaded?.Invoke();
        // 转场结束后，禁用blackMask
        blackMask.SetActive(false);
    }
}
