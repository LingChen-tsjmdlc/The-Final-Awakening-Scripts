using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueChange : MonoBehaviour
{
    //����ģʽ����
    public static ValueChange instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("UI ������")]
    public GameObject uiBackground;

    [Header("������")]
    public InputField playerMoveSpeed; // ����ƶ��ٶ�
    public InputField playerJumpForce;  // �����Ծ�ٶ�

    [Header("���������")]
    [Tooltip("����������������")] public InputField bollMassInput;            // ����������������
    [HideInInspector] public float bollMass = 1;                    // �����������
    [Tooltip("��������������������")] public InputField bollLinearDargInput;      // ��������������������
    [HideInInspector] public float bollLinearDarg = 1.2f;           // ���������������
    [Tooltip("����������������")] public InputField bollGravityScaleInput;    // ����������������
    [HideInInspector] public float bollGravityScale = 1;            // �����������

    [Header("���������")]
    [Tooltip("�������������ٶȼӳ�ֵ")] public InputField portalPlayerSpeedInput;    // �������������ٶȼӳ�ֵ
    [Tooltip("����������������ٶȼӳ�ֵ")] public InputField portalBallSpeedInput;      // ����������������ٶȼӳ�ֵ

    [Header("ȷ�ϰ�ť")]
    public Button ackButton;

    private void Start()
    {
        // ȷ��ΪackButton������¼�������
        ackButton.onClick.AddListener(AckButtonClicked);
        AckButtonClicked();
    }

    /// <summary>
    /// ��ackButton�����ʱ���ô˷���
    /// </summary>
    public void AckButtonClicked()
    {
        // ���Դ�InputField��ȡֵ��ת��Ϊfloat
        bool moveSpeedParsed = float.TryParse(playerMoveSpeed.text, out float moveSpeed);
        bool jumpForceParsed = float.TryParse(playerJumpForce.text, out float jumpForce);
        bool bollMassParsed = float.TryParse(bollMassInput.text, out bollMass);
        bool bollLinearDargParsed = float.TryParse(bollLinearDargInput.text, out bollLinearDarg);
        bool bollGravityScaleParsed = float.TryParse(bollGravityScaleInput.text, out bollGravityScale);
        bool portalPlayerSpeedParsed = float.TryParse(portalPlayerSpeedInput.text, out float portalPlayerSpeed);
        bool portalBallSpeedParsed = float.TryParse(portalBallSpeedInput.text, out float portalBallSpeed);

        // ���ת���ɹ��������PlayrtMovementʵ��������
        if (moveSpeedParsed && jumpForceParsed && bollMassParsed && bollLinearDargParsed && bollGravityScaleParsed && portalPlayerSpeedParsed && portalBallSpeedParsed)
        {
            Debug.Log("������ҵ��ƶ��ٶ�: " + moveSpeed + "������ҵ���Ծ����: " + jumpForce);
            Debug.Log("���ڼ����������: " + bollMass + "���ڼ��������������: " + bollLinearDarg + "���ڼ����������: " + bollGravityScale);
            Debug.Log("���ڴ������������ٶȼӳ�ֵ: " + portalPlayerSpeed + "���ڴ���������������ٶȼӳ�ֵ: " + portalBallSpeed);

            PlayrtMovement.instance.speed = moveSpeed;
            PlayrtMovement.instance.jumpForce = jumpForce;

            PortalTeleporter.playerAccelerationMultiple = portalPlayerSpeed;
            PortalTeleporter.bollAccelerationMultiple = portalBallSpeed;
        }
        else
        {
            Debug.LogError("���������֣������Ѿ�ȫ���ָ�Ĭ�ϡ�");
            PlayrtMovement.instance.speed = 5f;
            PlayrtMovement.instance.jumpForce = 8f;

            bollMass = 1;
            bollLinearDarg = 1.2f;
            bollGravityScale = 1;

            PortalTeleporter.playerAccelerationMultiple = 130f;
            PortalTeleporter.bollAccelerationMultiple = 3f;
        }
        LetUiBackgroundActiveFales();
        //Invoke("LetUiBackgroundActiveFales", 0.1f);
    }

    private void OnDestroy()
    {
        if (ackButton != null)
        {
            ackButton.onClick.RemoveListener(AckButtonClicked);
        }
    }

    /// <summary>
    /// ʹUI��������ʧ
    /// </summary>
    public void LetUiBackgroundActiveFales()
    {
        Time.timeScale = 1;
        uiBackground.SetActive(false);
    }

    /// <summary>
    /// ��UI������
    /// </summary>
    public void OpenUi()
    {
        Time.timeScale = 0;
        // ȷ��PlayrtMovement.instance���ڲ�����speed��jumpForce����
        if (PlayrtMovement.instance != null)
        {
            // ��ȡPlayrtMovementʵ����speed��jumpForceֵ
            float currentSpeed = PlayrtMovement.instance.speed;
            float currentJumpForce = PlayrtMovement.instance.jumpForce;

            // ����Щֵ����ΪInputField���ı�
            playerMoveSpeed.text = currentSpeed.ToString();
            playerJumpForce.text = currentJumpForce.ToString();
        }

        // ��ʾUI����
        uiBackground.SetActive(true);
    }

    /// <summary>
    /// �˳���Ϸ
    /// </summary>
    public void ExitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
