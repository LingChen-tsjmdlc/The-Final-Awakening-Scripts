using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMCtrl : MonoBehaviour
{
    public AudioMixer audioMixer;   // BGM �����Ŀ���

    [Header("BGM")]
    public Text VolumeText; // BGM ��ֵ�ı�
    public Slider volumeSlider;  // BGM ������
    [HideInInspector] public float nowAudioVolume;

    private void Start()
    {
        // ����Ƿ��б����BGM����ֵ
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            nowAudioVolume = PlayerPrefs.GetFloat("BGMVolume");
        }
        else
        {
            nowAudioVolume = -9f;
        }

        // ������Ƶ������������
        audioMixer.SetFloat("BGMVolume", nowAudioVolume);

        // ���� UI
        UpdateVolumeUI(nowAudioVolume, volumeSlider, VolumeText);
    }

    /// <summary>
    /// ���� UI �ϵ�������ʾ
    /// </summary>
    /// <param name="value">���������Ĵ�С</param>
    /// <param name="slider">Ҫ���ĵĻ�����</param>
    /// <param name="text">Ҫͬ�����ĵ�����</param>
    private void UpdateVolumeUI(float valueInDecibels, Slider slider, Text text)
    {
        // ֱ�����û����ֵΪ�ֱ�ֵ
        slider.value = valueInDecibels;

        // ���ֱ�ֵת��Ϊ�ٷֱȲ������ı�
        float percentVolume = Mathf.InverseLerp(-80f, -3f, valueInDecibels) * 100;
        text.text = Mathf.RoundToInt(percentVolume).ToString() + "%";
    }

    /// <summary>
    /// ��BGM����ֵ�ı�ʱ����
    /// </summary>
    /// <param name="value">�ı��ֵ</param>
    public void SetVBGMolume(float linearValue)
    {
        // �����������ֵת��Ϊ�ֱ�ֵ
        float valueInDecibels = 20f * Mathf.Log10(linearValue);
        nowAudioVolume = valueInDecibels;
        PlayerPrefs.SetFloat("BGMVolume", linearValue);
        PlayerPrefs.Save();
        audioMixer.SetFloat("BGMVolume", linearValue);
        UpdateVolumeUI(linearValue, volumeSlider, VolumeText);
    }
}
