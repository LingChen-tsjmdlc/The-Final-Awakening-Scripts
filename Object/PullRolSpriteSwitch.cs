using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PullRolSpriteSwitch : MonoBehaviour
{
    [Tooltip("传送门实例")][SerializeField] private GameObject protal; //传送门实例

    [SerializeField] private GameObject closeSprite;
    [SerializeField] private GameObject openSprite;

    [SerializeField] private GameObject openPosition;
    [SerializeField] private GameObject closePosition;

    private void Start()
    {
        closeSprite.SetActive(true);
        openSprite.SetActive(false);
    }

    // TODO: 有bug；按键不太灵敏
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                closeSprite.SetActive(!closeSprite.activeSelf);
                openSprite.SetActive(!openSprite.activeSelf);

                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().switchPole);

                ProtalChange();     
            }
        }
    }


    /// <summary>
    /// 传送门主方法
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
