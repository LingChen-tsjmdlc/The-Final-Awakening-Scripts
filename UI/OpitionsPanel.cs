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

    [Header("�ֱ��ʺʹ��ڻ�")]
    public Button resolutionSelectedButton;
    [Tooltip("�ֱ���������")] public Dropdown resolutionDropDown;
    [Tooltip("���ڷֱ�������")] public int nowResolutionIndex;

    [Header("�����豸��ѡ��")]
    public Button inputControlButton;
    [Tooltip("�����豸������")] public Dropdown inputControlDropDown;
    [Tooltip("���������豸������")] public int nowInputControlIndex;

    [Header("��һ��ѡ���ѡ��")]
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

        BGMVolume = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : -5.5f;    // ����Ƿ���Ĭ�ϵ� BGM ������С���ã����û������ΪĬ��
        FxVolume = PlayerPrefs.HasKey("FxVolume") ? PlayerPrefs.GetFloat("FxVolume") : -5.5f;    // ����Ƿ���Ĭ�ϵ� FX ������С���ã����û������ΪĬ��                                                                               
        nowResolutionIndex = PlayerPrefs.HasKey("NowResolutionIndex") ? PlayerPrefs.GetInt("NowResolutionIndex") : 0;   // ����Ƿ��б���ķֱ�������
        nowInputControlIndex = PlayerPrefs.HasKey("NowInputControlIndex") ? PlayerPrefs.GetInt("NowInputControlIndex") : 0; // ����Ƿ��б��������ѡ������

        UpdateVolumeUI(BGMVolume, BGMVolumeSlider, BGMVolumeText);
        UpdateVolumeUI(FxVolume, FxVolumeSlider, FxVolumeText);
    }

    // ��UIԪ�ر�ѡ��ʱ����
    public void OnSelect(BaseEventData eventData)
    {
        // �����д��Ԫ�ر�ѡ��ʱ����ִ�еĴ���
        Debug.Log(gameObject.name + " ��ѡ��");
    }

    private void Update()
    {
        if (InputManager.GetInstance().BackButton)
        {
            BackButtonDown();
        }

        // ȷ��BGM��ť��ѡ��
        if (bgmSelectedButton == EventSystem.current.currentSelectedGameObject)
        {
            Debug.Log("bgmSelectedButton �Ѿ���ѡ��");
            // ��ȡ����ֵ
            float changeValueAdd = InputManager.GetInstance().ChangeValueAdd;
            float changeValueReduce = InputManager.GetInstance().ChangeValueReduce;

            // ��������ֵ��������
            if (changeValueAdd > 0f)
            {
                // ��0~1ӳ�䵽1~30�ķ�Χ
                float volumeToAdd = changeValueAdd * 29f + 1f;
                SetBGMVolume(BGMVolume + volumeToAdd * Time.deltaTime);
            }
            else if (changeValueReduce > 0f)
            {
                // ��0~1ӳ�䵽1~30�ķ�Χ
                float volumeToReduce = changeValueReduce * 29f + 1f;
                SetBGMVolume(BGMVolume - volumeToReduce * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// ���� UI �ϵ�������ʾ
    /// </summary>
    /// <param name="value">�ı��ֵ</param>
    /// <param name="slider">Ҫ���µĻ�����</param>
    /// <param name="text">Ҫ���µ�����</param>
    private void UpdateVolumeUI(float value, Slider slider, Text text)
    {
        slider.value = value;

        float percentVolume = Mathf.InverseLerp(-80f, -3f, value) * 100;
        text.text = Mathf.RoundToInt(percentVolume).ToString() + "%";
    }

    // ��BGM����ֵ�ı�ʱ����
    public void SetBGMVolume(float value)
    {
        BGMVolume = value;
        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save();
        BGMAudioMixer.SetFloat("BGMVolume", value);
        UpdateVolumeUI(value, BGMVolumeSlider, BGMVolumeText);
    }

    // ��FX����ֵ�ı�ʱ����
    public void SetFxVolume(float value)
    {
        FxVolume = value;
        PlayerPrefs.SetFloat("FxVolume", value);
        PlayerPrefs.Save();
        FxAudioMixer.SetFloat("FxVolume", value);
        UpdateVolumeUI(value, FxVolumeSlider, FxVolumeText);
    }

    // ���л��ֱ��ʵ�ʱ��
    public void ResolutionDropDownChange()
    {
        nowResolutionIndex = resolutionDropDown.value;
        PlayerPrefs.SetInt("NowResolutionIndex", nowResolutionIndex);
        PlayerPrefs.Save();
        Debug.Log("�ֱ������óɹ�������ֵΪ��" + nowResolutionIndex);
        ResolutionSetting.ResolutionChanged(nowResolutionIndex);
    }

    // ���л������豸��ʱ���ʱ��
    public void InputControlDropDownChange()
    {
        nowInputControlIndex = inputControlDropDown.value;
        PlayerPrefs.SetInt("NowInputControlIndex", nowInputControlIndex);
        PlayerPrefs.Save();
        Debug.Log("�����豸���óɹ�������ֵΪ��" + nowInputControlIndex);
        PlayerInputControl.InputControlChanged(nowInputControlIndex);
    }

    /// <summary>
    /// ���ذ�ť����
    /// </summary>
    public void BackButtonDown()
    {
        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().buttonDownAudio);

        UIManager.GetInstance().DestroyPanel(this.gameObject, false);
        GameManager.GetInstance().ispause = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// ���������水ť����ʱ
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
    /// ���ؿ���ť���µ�ʱ��
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
