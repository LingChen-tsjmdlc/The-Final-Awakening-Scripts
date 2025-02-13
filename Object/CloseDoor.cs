using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] private Animator doorClose;

    private bool isCloseedOnce; // 仅触发一次的变量

    private bool isDoorCloseed;//按钮触发完成变量

    private void Update()
    {
        if (!isDoorCloseed)
        {
            isDoorCloseed = InputManager.GetInstance().ObjectInteraction;
            Debug.Log("isDoorCloseed: " + isDoorCloseed);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCloseedOnce)
        {
            if (isDoorCloseed)
            {
                isCloseedOnce = true;
                isDoorCloseed = false;

                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().doorOpen);

                doorClose.SetBool("Close", true);
            }
        }
    }
}
