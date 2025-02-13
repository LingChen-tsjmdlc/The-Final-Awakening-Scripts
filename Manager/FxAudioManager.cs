using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxAudioManager : MonoBehaviour
{
    private static FxAudioManager instance;

    [Header("玩家")]
    public AudioSource playerShoot;
    public AudioSource playerJump;
    public AudioSource playerRun;
    public AudioSource downToTheWater;
    public AudioSource playerDie;

    [Header("传送门")]
    public AudioSource portalBlue;
    public AudioSource portalRed;

    [Header("玩家可交互物体")]
    public AudioSource switchPole;
    public AudioSource laserShoot;
    public AudioSource doorOpen;
    public AudioSource fallingWaterLevels;
    public AudioSource openMschine;

    /// <summary>
    /// 获取 UIManager 的实例对象
    /// </summary>
    /// <returns>UIManager 实例</returns>
    public static FxAudioManager GetInstance()
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

    private void Update()
    {
        // 检查GameManager是否暂停游戏
        if (GameManager.GetInstance().ispause)
        {
            // 暂停所有玩家音效
            StopSound(playerShoot);
            StopSound(playerJump);
            StopSound(playerRun);
            StopSound(downToTheWater);
            StopSound(playerDie);

            // 暂停所有传送门音效
            StopSound(portalBlue);
            StopSound(portalRed);

            // 暂停所有玩家可交互物体音效
            StopSound(switchPole);
            StopSound(laserShoot);
            StopSound(doorOpen);
            StopSound(fallingWaterLevels); 
        }
    }

    /// <summary>
    /// 通用播放 Fx 声音方法
    /// </summary>
    /// <param name="soundName">要播放的音频的名称</param>
    public void PlaySound(AudioSource soundName)
    {
        if (soundName != null)
        {
            soundName.Play();
        }
    }

    /// <summary>
    /// 通用停止播放 Fx 声音方法
    /// </summary>
    /// <param name="soundName">要播放的音频的名称</param>
    public void StopSound(AudioSource soundName)
    {
        if (soundName != null)
        {
            soundName.Stop();
        }
    }
}
