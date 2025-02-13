using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    private static UIAudioManager instance;

    [Header("��ť�����͵��")]
    public AudioSource buttonTouchAudio;
    public AudioSource buttonDownAudio; 
    public AudioSource startUIButtonDownAudio;

    /// <summary>
    /// ��ȡ UIManager ��ʵ������
    /// </summary>
    /// <returns>UIManager ʵ��</returns>
    public static UIAudioManager GetInstance()
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

    /// <summary>
    /// ͨ�ò��� UI �� Fx ��������
    /// </summary>
    /// <param name="soundName">Ҫ���ŵ���Ƶ������</param>
    public void PlaySound(AudioSource soundName)
    {
        if (soundName != null)
        {
            soundName.Play();
        }
    }
}
