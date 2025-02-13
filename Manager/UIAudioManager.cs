using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    private static UIAudioManager instance;

    [Header("按钮触碰和点击")]
    public AudioSource buttonTouchAudio;
    public AudioSource buttonDownAudio; 
    public AudioSource startUIButtonDownAudio;

    /// <summary>
    /// 获取 UIManager 的实例对象
    /// </summary>
    /// <returns>UIManager 实例</returns>
    public static UIAudioManager GetInstance()
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

    /// <summary>
    /// 通用播放 UI 的 Fx 声音方法
    /// </summary>
    /// <param name="soundName">要播放的音频的名称</param>
    public void PlaySound(AudioSource soundName)
    {
        if (soundName != null)
        {
            soundName.Play();
        }
    }
}
