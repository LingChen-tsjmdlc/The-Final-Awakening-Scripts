using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMCtrl : MonoBehaviour
{
    public AudioMixer audioMixer;   // BGM 音量的控制

    [Header("BGM")]
    public Text VolumeText; // BGM 数值文本
    public Slider volumeSlider;  // BGM 滚动条
    [HideInInspector] public float nowAudioVolume;

    private void Start()
    {
        // 检查是否有保存的BGM音量值
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            nowAudioVolume = PlayerPrefs.GetFloat("BGMVolume");
        }
        else
        {
            nowAudioVolume = -9f;
        }

        // 设置音频混音器的音量
        audioMixer.SetFloat("BGMVolume", nowAudioVolume);

        // 更新 UI
        UpdateVolumeUI(nowAudioVolume, volumeSlider, VolumeText);
    }

    /// <summary>
    /// 更新 UI 上的音量显示
    /// </summary>
    /// <param name="value">现在声音的大小</param>
    /// <param name="slider">要更改的滑动条</param>
    /// <param name="text">要同步更改的文字</param>
    private void UpdateVolumeUI(float valueInDecibels, Slider slider, Text text)
    {
        // 直接设置滑块的值为分贝值
        slider.value = valueInDecibels;

        // 将分贝值转换为百分比并更新文本
        float percentVolume = Mathf.InverseLerp(-80f, -3f, valueInDecibels) * 100;
        text.text = Mathf.RoundToInt(percentVolume).ToString() + "%";
    }

    /// <summary>
    /// 当BGM音量值改变时调用
    /// </summary>
    /// <param name="value">改变的值</param>
    public void SetVBGMolume(float linearValue)
    {
        // 将滑块的线性值转换为分贝值
        float valueInDecibels = 20f * Mathf.Log10(linearValue);
        nowAudioVolume = valueInDecibels;
        PlayerPrefs.SetFloat("BGMVolume", linearValue);
        PlayerPrefs.Save();
        audioMixer.SetFloat("BGMVolume", linearValue);
        UpdateVolumeUI(linearValue, volumeSlider, VolumeText);
    }
}
