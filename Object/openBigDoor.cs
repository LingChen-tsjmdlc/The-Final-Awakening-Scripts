using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openBigDoor : MonoBehaviour
{
    [SerializeField]private Animator door1;
    [SerializeField]private Animator door2;

    private bool isBigDoorOpen;

    private void Update()
    {
        if (!isBigDoorOpen)
        {
            isBigDoorOpen = InputManager.GetInstance().ObjectInteraction;
            Debug.Log("isBigDoorOpen: " + isBigDoorOpen);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isBigDoorOpen)
        {
            if (isBigDoorOpen)
            {
                isBigDoorOpen = false;

                door1.SetBool("Open", true);
                door2.SetBool("Open", true);

                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().doorOpen);
            }
        }
    }
}
