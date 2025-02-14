using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    [Header("�����ʽ")]
    public Texture2D cursorClickTexture;
    public Texture2D cursorShootTexture;
    [Header("�����Ч")]
    public Texture2D clickFx;
    public Texture2D shootFx;

    [HideInInspector] public bool playerHanGun;   //����Ƿ�ӵ�м���ǹ
    [SerializeField] private Canvas canvas; // ���һ��Canvas������ʾUIԪ��

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
    /// ��ȡ ScenesManager ��ʵ������
    /// </summary>
    /// <returns>ScenesManager ʵ��</returns>
    public static CursorController GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("δ�ҵ� ScenesManager ������");
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
                // ����������Ƿ񱻰���
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    SetCursorClickTexture(shootFx);
                    ShowClickEffect();
                }
                // ����������Ƿ��ͷ�
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    Cursor.SetCursor(cursorShootTexture, hotSpot, cursorMode);
                }
            }
            else
            {
                // ����������Ƿ񱻰���
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    SetCursorClickTexture(cursorClickTexture);
                    //ShowClickEffect();
                }
                // ����������Ƿ��ͷ�
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    ResetCursor();
                }
            }
        }
    }

    void SetCursorClickTexture(Texture2D texture2D)
    {
        // ���ù��Ϊ���ʱ������
        Cursor.SetCursor(texture2D, hotSpot, cursorMode);
    }

    public void ResetCursor()
    {
        // �ָ�Ĭ�Ϲ��
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    void ShowClickEffect()
    {
        // ����һ���µ�Image GameObject
        GameObject clickEffectObj = new GameObject("ClickEffect");
        clickEffectObj.transform.SetParent(canvas.transform, false);
        Image clickEffectImage = clickEffectObj.AddComponent<Image>();
        clickEffectImage.sprite = Sprite.Create(clickFx, new Rect(0.0f, 0.0f, clickFx.width, clickFx.height), new Vector2(0.5f, 0.5f), 100.0f);

        // ����Image��λ��Ϊ�������λ��
        Vector2 mousePos = Input.mousePosition;
        clickEffectImage.rectTransform.position = mousePos;

        // ��ʼ���뵭������
        StartCoroutine(AnimateClickEffect(clickEffectImage));
    }

    IEnumerator AnimateClickEffect(Image clickEffectImage)
    {
        // ����
        float duration = 0.3f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            clickEffectImage.color = new Color(1, 1, 1, 1 - duration / 0.3f);
            yield return null;
        }

        // ��ʾ
        yield return new WaitForSeconds(0.1f);

        // ����
        duration = 0.2f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            clickEffectImage.color = new Color(1, 1, 1, duration / 0.2f);
            yield return null;
        }

        // ����GameObject
        Destroy(clickEffectImage.gameObject);
    }
}
