using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OpitionsPanel : MonoBehaviour, ISelectHandler
{
    public GameObject ReloadGameButton;

    [Header("AudioMixer")]
    public AudioMixer BGMAudioMixer;
    public AudioMixer FxAudioMixer;

    [Header("BGM")]
    public Button bgmSelectedButton;
    public Text BGMVolumeText;
    public Slider BGMVolumeSlider;
    [HideInInspector] public float BGMVolume;

    [Header("Fx")]
    public Button fxSelectedButton;
    public Text FxVolumeText;
    public Slider FxVolumeSlider;
    [HideInInspector] public float FxVolume;

    [Header("分辨率和窗口化")]
    public Button resolutionSelectedButton;
    [Tooltip("分辨率下拉框")] public Dropdown resolutionDropDown;
    [Tooltip("现在分辨率索引")] public int nowResolutionIndex;

    [Header("输入设备的选择")]
    public Button inputControlButton;
    [Tooltip("输入设备下拉框")] public Dropdown inputControlDropDown;
    [Tooltip("现在输入设备的索引")] public int nowInputControlIndex;

    [Header("第一个选择的选项")]
    [SerializeField] private GameObject menuFisrt;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(menuFisrt);

        if (SceneManager.GetActiveScene().name == "UIScenes")
        {
            ReloadGameButton.SetActive(false);
        }
        else
        {
            ReloadGameButton.SetActive(true);
        }

        BGMVolume = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : -5.5f;    // 检查是否有默认的 BGM 音量大小设置，如果没有设置为默认
        FxVolume = PlayerPrefs.HasKey("FxVolume") ? PlayerPrefs.GetFloat("FxVolume") : -5.5f;    // 检查是否有默认的 FX 音量大小设置，如果没有设置为默认                                                                               
        nowResolutionIndex = PlayerPrefs.HasKey("NowResolutionIndex") ? PlayerPrefs.GetInt("NowResolutionIndex") : 0;   // 检查是否有保存的分辨率设置
        nowInputControlIndex = PlayerPrefs.HasKey("NowInputControlIndex") ? PlayerPrefs.GetInt("NowInputControlIndex") : 0; // 检查是否有保存的输入选项设置

        UpdateVolumeUI(BGMVolume, BGMVolumeSlider, BGMVolumeText);
        UpdateVolumeUI(FxVolume, FxVolumeSlider, FxVolumeText);
    }

    // 当UI元素被选中时调用
    public void OnSelect(BaseEventData eventData)
    {
        // 这里编写当元素被选中时你想执行的代码
        Debug.Log(gameObject.name + " 被选中");
    }

    private void Update()
    {
        if (InputManager.GetInstance().BackButton)
        {
            BackButtonDown();
        }

        // 确保BGM按钮被选中
        if (bgmSelectedButton == EventSystem.current.currentSelectedGameObject)
        {
            Debug.Log("bgmSelectedButton 已经被选中");
            // 读取输入值
            float changeValueAdd = InputManager.GetInstance().ChangeValueAdd;
            float changeValueReduce = InputManager.GetInstance().ChangeValueReduce;

            // 根据输入值调整音量
            if (changeValueAdd > 0f)
            {
                // 将0~1映射到1~30的范围
                float volumeToAdd = changeValueAdd * 29f + 1f;
                SetBGMVolume(BGMVolume + volumeToAdd * Time.deltaTime);
            }
            else if (changeValueReduce > 0f)
            {
                // 将0~1映射到1~30的范围
                float volumeToReduce = changeValueReduce * 29f + 1f;
                SetBGMVolume(BGMVolume - volumeToReduce * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 更新 UI 上的音量显示
    /// </summary>
    /// <param name="value">改变的值</param>
    /// <param name="slider">要更新的滑动条</param>
    /// <param name="text">要更新的文字</param>
    private void UpdateVolumeUI(float value, Slider slider, Text text)
    {
        slider.value = value;

        float percentVolume = Mathf.InverseLerp(-80f, -3f, value) * 100;
        text.text = Mathf.RoundToInt(percentVolume).ToString() + "%";
    }

    // 当BGM音量值改变时调用
    public void SetBGMVolume(float value)
    {
        BGMVolume = value;
        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save();
        BGMAudioMixer.SetFloat("BGMVolume", value);
        UpdateVolumeUI(value, BGMVolumeSlider, BGMVolumeText);
    }

    // 当FX音量值改变时调用
    public void SetFxVolume(float value)
    {
        FxVolume = value;
        PlayerPrefs.SetFloat("FxVolume", value);
        PlayerPrefs.Save();
        FxAudioMixer.SetFloat("FxVolume", value);
        UpdateVolumeUI(value, FxVolumeSlider, FxVolumeText);
    }

    // 当切换分辨率的时候
    public void ResolutionDropDownChange()
    {
        nowResolutionIndex = resolutionDropDown.value;
        PlayerPrefs.SetInt("NowResolutionIndex", nowResolutionIndex);
        PlayerPrefs.Save();
        Debug.Log("分辨率设置成功！索引值为：" + nowResolutionIndex);
        ResolutionSetting.ResolutionChanged(nowResolutionIndex);
    }

    // 当切换输入设备的时候的时候
    public void InputControlDropDownChange()
    {
        nowInputControlIndex = inputControlDropDown.value;
        PlayerPrefs.SetInt("NowInputControlIndex", nowInputControlIndex);
        PlayerPrefs.Save();
        Debug.Log("输入设备设置成功！索引值为：" + nowInputControlIndex);
        PlayerInputControl.InputControlChanged(nowInputControlIndex);
    }

    /// <summary>
    /// 返回按钮按下
    /// </summary>
    public void BackButtonDown()
    {
        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().buttonDownAudio);

        UIManager.GetInstance().DestroyPanel(this.gameObject, false);
        GameManager.GetInstance().ispause = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 返回主界面按钮按下时
    /// </summary>
    public void OutButtonDown()
    {
        StopAllCoroutines();

        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().buttonDownAudio);

        if (SceneManager.GetActiveScene().name == "UIScenes")
        {
            UIManager.GetInstance().DestroyPanel(this.gameObject, false);
        }
        else
        {
            ScenesManager.GetInstance().TransitionToScene("UIScenes", 0.8f, 0.6f, () =>
            {
                ScenesManager.GetInstance().EnableColliders();
                if (SceneManager.GetActiveScene().name == "UIScenes")
                {
                    UIManager.GetInstance().FindPanel("StartUI");
                }
            });
        }
        Time.timeScale = 1f;
        GameManager.GetInstance().ispause = false;
        UIManager.GetInstance().DestroyPanel(this.gameObject, false);
    }

    /// <summary>
    /// 当重开按钮按下的时候
    /// </summary>
    public void RestartGame()
    {
        StopAllCoroutines();

        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().buttonDownAudio);

        ScenesManager.GetInstance().ReloadNowScenes();
        Time.timeScale = 1f;
        GameManager.GetInstance().ispause = false;
        UIManager.GetInstance().DestroyPanel(this.gameObject,false);
    }
}
