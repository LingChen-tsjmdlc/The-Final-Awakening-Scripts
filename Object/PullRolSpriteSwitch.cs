using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PullRolSpriteSwitch : MonoBehaviour
{
    [Tooltip("������ʵ��")][SerializeField] private GameObject protal; //������ʵ��

    [SerializeField] private GameObject closeSprite;
    [SerializeField] private GameObject openSprite;

    [SerializeField] private GameObject openPosition;
    [SerializeField] private GameObject closePosition;

    private bool isSwitch;

    private void Start()
    {
        closeSprite.SetActive(true);
        openSprite.SetActive(false);
    }

    private void Update()
    {
        // ֻ�е�isSwitchΪfalseʱ���Ÿ����������isSwitch��ֵ
        if (!isSwitch)
        {
            isSwitch = InputManager.GetInstance().ObjectInteraction;
            Debug.Log("isSwitch: " + isSwitch);
        }
    }

    // TODO: ��bug��������̫����
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player") && isSwitch)
        {
            closeSprite.SetActive(!closeSprite.activeSelf);
            openSprite.SetActive(!openSprite.activeSelf);

            FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().switchPole);

            ProtalChange();
            isSwitch = false;
        }
    }


    /// <summary>
    /// ������������
    /// </summary>
    private void ProtalChange()
    {
        if (closeSprite.activeSelf)
        {
            protal.transform.position = closePosition.transform.position;
        }
        if (openSprite.activeSelf) 
        {
            protal.transform.position = openPosition.transform.position;
        }
    }
}
