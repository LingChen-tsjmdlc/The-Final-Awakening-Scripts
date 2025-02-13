using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueChange : MonoBehaviour
{
    //单例模式设置
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

    [Header("UI 背景框")]
    public GameObject uiBackground;

    [Header("玩家相关")]
    public InputField playerMoveSpeed; // 玩家移动速度
    public InputField playerJumpForce;  // 玩家跳跃速度

    [Header("激光球相关")]
    [Tooltip("激光球的质量输入框")] public InputField bollMassInput;            // 激光球的质量输入框
    [HideInInspector] public float bollMass = 1;                    // 激光球的质量
    [Tooltip("激光球的线性阻力输入框")] public InputField bollLinearDargInput;      // 激光球的线性阻力输入框
    [HideInInspector] public float bollLinearDarg = 1.2f;           // 激光球的线性阻力
    [Tooltip("激光球的重力输入框")] public InputField bollGravityScaleInput;    // 激光球的重力输入框
    [HideInInspector] public float bollGravityScale = 1;            // 激光球的重力

    [Header("传送门相关")]
    [Tooltip("传送门射出玩家速度加成值")] public InputField portalPlayerSpeedInput;    // 传送门射出玩家速度加成值
    [Tooltip("传送门射出激光球速度加成值")] public InputField portalBallSpeedInput;      // 传送门射出激光球速度加成值

    [Header("确认按钮")]
    public Button ackButton;

    private void Start()
    {
        // 确保为ackButton添加了事件监听器
        ackButton.onClick.AddListener(AckButtonClicked);
        AckButtonClicked();
    }

    /// <summary>
    /// 当ackButton被点击时调用此方法
    /// </summary>
    public void AckButtonClicked()
    {
        // 尝试从InputField获取值并转换为float
        bool moveSpeedParsed = float.TryParse(playerMoveSpeed.text, out float moveSpeed);
        bool jumpForceParsed = float.TryParse(playerJumpForce.text, out float jumpForce);
        bool bollMassParsed = float.TryParse(bollMassInput.text, out bollMass);
        bool bollLinearDargParsed = float.TryParse(bollLinearDargInput.text, out bollLinearDarg);
        bool bollGravityScaleParsed = float.TryParse(bollGravityScaleInput.text, out bollGravityScale);
        bool portalPlayerSpeedParsed = float.TryParse(portalPlayerSpeedInput.text, out float portalPlayerSpeed);
        bool portalBallSpeedParsed = float.TryParse(portalBallSpeedInput.text, out float portalBallSpeed);

        // 如果转换成功，则更新PlayrtMovement实例的属性
        if (moveSpeedParsed && jumpForceParsed && bollMassParsed && bollLinearDargParsed && bollGravityScaleParsed && portalPlayerSpeedParsed && portalBallSpeedParsed)
        {
            Debug.Log("现在玩家的移动速度: " + moveSpeed + "现在玩家的跳跃力量: " + jumpForce);
            Debug.Log("现在激光球的质量: " + bollMass + "现在激光球的线性阻力: " + bollLinearDarg + "现在激光球的重力: " + bollGravityScale);
            Debug.Log("现在传送门射出玩家速度加成值: " + portalPlayerSpeed + "现在传送门射出激光球速度加成值: " + portalBallSpeed);

            PlayrtMovement.instance.speed = moveSpeed;
            PlayrtMovement.instance.jumpForce = jumpForce;

            PortalTeleporter.playerAccelerationMultiple = portalPlayerSpeed;
            PortalTeleporter.bollAccelerationMultiple = portalBallSpeed;
        }
        else
        {
            Debug.LogError("请输入数字！！！已经全部恢复默认。");
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
    /// 使UI主界面消失
    /// </summary>
    public void LetUiBackgroundActiveFales()
    {
        Time.timeScale = 1;
        uiBackground.SetActive(false);
    }

    /// <summary>
    /// 打开UI主界面
    /// </summary>
    public void OpenUi()
    {
        Time.timeScale = 0;
        // 确保PlayrtMovement.instance存在并且有speed和jumpForce属性
        if (PlayrtMovement.instance != null)
        {
            // 获取PlayrtMovement实例的speed和jumpForce值
            float currentSpeed = PlayrtMovement.instance.speed;
            float currentJumpForce = PlayrtMovement.instance.jumpForce;

            // 将这些值设置为InputField的文本
            playerMoveSpeed.text = currentSpeed.ToString();
            playerJumpForce.text = currentJumpForce.ToString();
        }

        // 显示UI背景
        uiBackground.SetActive(true);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
