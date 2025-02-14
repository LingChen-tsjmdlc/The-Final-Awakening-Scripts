using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    [Header("光标样式")]
    public Texture2D cursorClickTexture;
    public Texture2D cursorShootTexture;
    [Header("光标特效")]
    public Texture2D clickFx;
    public Texture2D shootFx;

    [HideInInspector] public bool playerHanGun;   //玩家是否拥有激光枪
    [SerializeField] private Canvas canvas; // 添加一个Canvas用于显示UI元素

    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private static CursorController instance;

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

    /// <summary>
    /// 获取 ScenesManager 的实例对象
    /// </summary>
    /// <returns>ScenesManager 实例</returns>
    public static CursorController GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("未找到 ScenesManager 单例！");
            return null;
        }
        return instance;
    }

    void Update()
    {
        if (!GameManager.GetInstance().ispause)
        {
            if (playerHanGun)
            {
                Cursor.SetCursor(cursorShootTexture, hotSpot, cursorMode);
                // 检测鼠标左键是否被按下
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    SetCursorClickTexture(shootFx);
                    ShowClickEffect();
                }
                // 检测鼠标左键是否被释放
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    Cursor.SetCursor(cursorShootTexture, hotSpot, cursorMode);
                }
            }
            else
            {
                // 检测鼠标左键是否被按下
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    SetCursorClickTexture(cursorClickTexture);
                    //ShowClickEffect();
                }
                // 检测鼠标左键是否被释放
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    ResetCursor();
                }
            }
        }
    }

    void SetCursorClickTexture(Texture2D texture2D)
    {
        // 设置光标为点击时的纹理
        Cursor.SetCursor(texture2D, hotSpot, cursorMode);
    }

    public void ResetCursor()
    {
        // 恢复默认光标
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    void ShowClickEffect()
    {
        // 创建一个新的Image GameObject
        GameObject clickEffectObj = new GameObject("ClickEffect");
        clickEffectObj.transform.SetParent(canvas.transform, false);
        Image clickEffectImage = clickEffectObj.AddComponent<Image>();
        clickEffectImage.sprite = Sprite.Create(clickFx, new Rect(0.0f, 0.0f, clickFx.width, clickFx.height), new Vector2(0.5f, 0.5f), 100.0f);

        // 设置Image的位置为鼠标点击的位置
        Vector2 mousePos = Input.mousePosition;
        clickEffectImage.rectTransform.position = mousePos;

        // 开始淡入淡出动画
        StartCoroutine(AnimateClickEffect(clickEffectImage));
    }

    IEnumerator AnimateClickEffect(Image clickEffectImage)
    {
        // 淡入
        float duration = 0.3f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            clickEffectImage.color = new Color(1, 1, 1, 1 - duration / 0.3f);
            yield return null;
        }

        // 显示
        yield return new WaitForSeconds(0.1f);

        // 淡出
        duration = 0.2f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            clickEffectImage.color = new Color(1, 1, 1, duration / 0.2f);
            yield return null;
        }

        // 销毁GameObject
        Destroy(clickEffectImage.gameObject);
    }
}
