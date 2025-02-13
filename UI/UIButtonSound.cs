using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    private AudioSource soundbuttonTouchAudio; // 悬停时的声音
    private AudioSource buttonDownAudio; // 点击时的声音
    private AudioSource startUIButtonDownAudio; // 开始UI界面点击按钮时的声音

    private void Start()
    {
        soundbuttonTouchAudio = UIAudioManager.GetInstance().buttonTouchAudio;
        buttonDownAudio = UIAudioManager.GetInstance().buttonDownAudio;
        startUIButtonDownAudio = UIAudioManager.GetInstance().startUIButtonDownAudio;
    }

    // 当鼠标悬停在Button上时调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (soundbuttonTouchAudio != null)
        {
            soundbuttonTouchAudio.Play();
        }
    }

    // 当点击Button时调用
    public void OnPointerClick(PointerEventData eventData)
    {
        // 检查场景中是否有名为"StartUI"的物体或其克隆体
        GameObject startUI = GameObject.Find("StartUI") ?? GameObject.Find("StartUI(Clone)");
        if (startUI != null && startUIButtonDownAudio != null)
        {
            startUIButtonDownAudio.Play();
        }
        else if (buttonDownAudio != null)
        {
            // 如果不存在，播放buttonDownAudio声音
            buttonDownAudio.Play();
        }
    }
}
