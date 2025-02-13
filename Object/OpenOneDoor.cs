using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOneDoor : MonoBehaviour
{
    [SerializeField] private GameObject closeSprite;
    [SerializeField] private GameObject openSprite;

    [SerializeField] private Animator door;

    private bool isOpenedOnce;
    private bool isOneDoorOpen;

    private void Update()
    {
        // ֻ�е�isSwitchΪfalseʱ���Ÿ����������isSwitch��ֵ
        if (!isOneDoorOpen)
        {
            isOneDoorOpen = InputManager.GetInstance().ObjectInteraction;
            Debug.Log("isOneDoorOpen: " + isOneDoorOpen);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpenedOnce)
        {
            if (isOneDoorOpen)
            {
                isOpenedOnce = true;
                isOneDoorOpen = false;

                closeSprite.SetActive(false);
                openSprite.SetActive(true);

                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().doorOpen);
                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().switchPole);

                door.SetBool("Open", true);
            }
        }
    }
}
