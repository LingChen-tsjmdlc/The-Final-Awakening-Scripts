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
        // 只有当isSwitch为false时，才根据输入更新isSwitch的值
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
