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
    /// ��ȡ UIManager ��ʵ������
    /// </summary>
    /// <returns>UIManager ʵ��</returns>
    public static InputManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("δ�ҵ�UIManage������");
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
        // �������Ƿ�����ʹ�ÿ�����
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

        // ��������������ʹ�ü������
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
        // ��ȡ��ǰ��������豸�б�
        var devices = InputSystem.devices;
        // ����б����Ƿ����κο������豸
        foreach (var device in devices)
        {
            // ����豸����Ϸ���������򷵻� true
            if (device is Gamepad || device is Joystick)
            {
                return true;
            }
        }
        // ���û���ҵ��������豸���򷵻� false
        return false;
    }

    /// <summary>
    /// ����ֵ�ָ�Ĭ��
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
