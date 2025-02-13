using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxAudioManager : MonoBehaviour
{
    private static FxAudioManager instance;

    [Header("���")]
    public AudioSource playerShoot;
    public AudioSource playerJump;
    public AudioSource playerRun;
    public AudioSource downToTheWater;
    public AudioSource playerDie;

    [Header("������")]
    public AudioSource portalBlue;
    public AudioSource portalRed;

    [Header("��ҿɽ�������")]
    public AudioSource switchPole;
    public AudioSource laserShoot;
    public AudioSource doorOpen;
    public AudioSource fallingWaterLevels;
    public AudioSource openMschine;

    /// <summary>
    /// ��ȡ UIManager ��ʵ������
    /// </summary>
    /// <returns>UIManager ʵ��</returns>
    public static FxAudioManager GetInstance()
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

    private void Update()
    {
        // ���GameManager�Ƿ���ͣ��Ϸ
        if (GameManager.GetInstance().ispause)
        {
            // ��ͣ���������Ч
            StopSound(playerShoot);
            StopSound(playerJump);
            StopSound(playerRun);
            StopSound(downToTheWater);
            StopSound(playerDie);

            // ��ͣ���д�������Ч
            StopSound(portalBlue);
            StopSound(portalRed);

            // ��ͣ������ҿɽ���������Ч
            StopSound(switchPole);
            StopSound(laserShoot);
            StopSound(doorOpen);
            StopSound(fallingWaterLevels); 
        }
    }

    /// <summary>
    /// ͨ�ò��� Fx ��������
    /// </summary>
    /// <param name="soundName">Ҫ���ŵ���Ƶ������</param>
    public void PlaySound(AudioSource soundName)
    {
        if (soundName != null)
        {
            soundName.Play();
        }
    }

    /// <summary>
    /// ͨ��ֹͣ���� Fx ��������
    /// </summary>
    /// <param name="soundName">Ҫ���ŵ���Ƶ������</param>
    public void StopSound(AudioSource soundName)
    {
        if (soundName != null)
        {
            soundName.Stop();
        }
    }
}
