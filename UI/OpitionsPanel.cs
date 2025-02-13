using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpitionsPanel : MonoBehaviour
{
    public GameObject ReloadGameButton;

    [Header("AudioMixer")]
    public AudioMixer BGMAudioMixer;
    public AudioMixer FxAudioMixer;

    [Header("BGM")]
    public Text BGMVolumeText;
    public Slider BGMVolumeSlider;
    [HideInInspector] public float BGMVolume;

    [Header("Fx")]
    public Text FxVolumeText;
    public Slider FxVolumeSlider;
    [HideInInspector] public float FxVolume;

    [Header("�ֱ��ʺʹ��ڻ�")]
    [Tooltip("�ֱ���������")] public Dropdown resolutionDropDown;
    [Tooltip("���ڷֱ�������")] public int nowResolutionIndex;


    private void Start()
    {
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

        UpdateVolumeUI(BGMVolume, BGMVolumeSlider, BGMVolumeText);
        UpdateVolumeUI(FxVolume, FxVolumeSlider, FxVolumeText);
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
