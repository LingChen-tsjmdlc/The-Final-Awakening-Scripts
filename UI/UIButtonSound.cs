using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    private AudioSource soundbuttonTouchAudio; // ��ͣʱ������
    private AudioSource buttonDownAudio; // ���ʱ������
    private AudioSource startUIButtonDownAudio; // ��ʼUI��������ťʱ������

    private void Start()
    {
        soundbuttonTouchAudio = UIAudioManager.GetInstance().buttonTouchAudio;
        buttonDownAudio = UIAudioManager.GetInstance().buttonDownAudio;
        startUIButtonDownAudio = UIAudioManager.GetInstance().startUIButtonDownAudio;
    }

    // �������ͣ��Button��ʱ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (soundbuttonTouchAudio != null)
        {
            soundbuttonTouchAudio.Play();
        }
    }

    // �����Buttonʱ����
    public void OnPointerClick(PointerEventData eventData)
    {
        // ��鳡�����Ƿ�����Ϊ"StartUI"����������¡��
        GameObject startUI = GameObject.Find("StartUI") ?? GameObject.Find("StartUI(Clone)");
        if (startUI != null && startUIButtonDownAudio != null)
        {
            startUIButtonDownAudio.Play();
        }
        else if (buttonDownAudio != null)
        {
            // ��������ڣ�����buttonDownAudio����
            buttonDownAudio.Play();
        }
    }
}
