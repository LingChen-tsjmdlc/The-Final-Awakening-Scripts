using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Debug = UnityEngine.Debug;

public class InputManager : MonoBehaviour
{
    public bool IsUsingController { get; private set; }

    public InputActionReference jumpAction;
    public InputActionReference moveAction;
    public InputActionReference objectInteractionAction;
    public InputActionReference TextInteractionAction;
    public InputActionReference shootLeftBlueAction;
    public InputActionReference shootRightRedAction;
    public InputActionReference shootAngleAction;
    public InputActionReference escAction;
    public InputActionReference backButtonAction;
    public InputActionReference restartGameAction;
    public InputActionReference changeValueAddAction;
    public InputActionReference changeValueReduceAction;

    public bool IsJump {  get; private set; }
    public Vector2 Move {  get; private set; }
    public bool ObjectInteraction {  get; private set; }
    public bool TextInteraction {  get; private set; }
    public bool ShootLeftBlue {  get; private set; }
    public bool ShootRightRed {  get; private set; }
    public Vector2 ShootAngle {  get; private set; }
    public bool Esc {  get; private set; }
    public bool BackButton {  get; private set; }
    public bool RestartGame {  get; private set; }
    public float ChangeValueAdd {  get; private set; }
    public float ChangeValueReduce { get; private set; }



    private static InputManager instance;

    /// <summary>
    /// 获取 UIManager 的实例对象
    /// </summary>
    /// <returns>UIManager 实例</returns>
    public static InputManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("未找到UIManage单例！");
            return null;
        }
        return instance;
    }

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
        IsUsingController = IsControllerConnected();
    }

    private void Update()
    {
        // 检测玩家是否正在使用控制器
        if(PlayerPrefs.GetInt("NowInputControlIndex") == 0)
        {
            IsUsingController = IsControllerConnected();
        }

        IsJump = jumpAction.action.WasPressedThisFrame();
        Move = moveAction.action.ReadValue<Vector2>();
        ObjectInteraction = objectInteractionAction.action.WasPressedThisFrame();
        TextInteraction = TextInteractionAction.action.WasPressedThisFrame();
        ShootLeftBlue = shootLeftBlueAction.action.WasPressedThisFrame();
        ShootRightRed = shootRightRedAction.action.WasPressedThisFrame();
        ShootAngle = shootAngleAction.action.ReadValue<Vector2>();
        Esc = escAction.action.WasPressedThisFrame();
        BackButton = backButtonAction.action.WasPressedThisFrame();
        RestartGame = restartGameAction.action.WasPressedThisFrame();
        ChangeValueAdd = changeValueAddAction.action.ReadValue<float>();
        ChangeValueReduce = changeValueReduceAction.action.ReadValue<float>();

        // 如果玩家在设置中使用键盘鼠标
        //if (PlayerPrefs.GetInt("NowInputControlIndex") == 1)
        //{
        //    ValueToDefault();
        //}
    }

    private void LateUpdate()
    {
        IsJump = false;
        Move = Vector2.zero;
        ShootLeftBlue = false;
        ShootRightRed = false;
        ShootAngle = Vector2.zero;
    }

    private bool IsControllerConnected()
    {
        // 获取当前玩家输入设备列表
        var devices = InputSystem.devices;
        // 检查列表中是否有任何控制器设备
        foreach (var device in devices)
        {
            // 如果设备是游戏控制器，则返回 true
            if (device is Gamepad || device is Joystick)
            {
                return true;
            }
        }
        // 如果没有找到控制器设备，则返回 false
        return false;
    }

    /// <summary>
    /// 所有值恢复默认
    /// </summary>
    private void ValueToDefault()
    {
        IsJump = false;
        Move = Vector2.zero;
        ObjectInteraction = false;
        TextInteraction = false;
        ShootLeftBlue = false;
        ShootRightRed = false;
        ShootAngle = Vector2.zero;
        Esc = false;
        BackButton = false;
        RestartGame = false;
        ChangeValueAdd = 0;
        ChangeValueReduce = 0;
    }
}
