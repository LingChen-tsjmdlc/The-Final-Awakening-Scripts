using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] private Animator doorClose;

    private bool isCloseedOnce; // ������һ�εı���

    private bool isDoorCloseed;//��ť������ɱ���

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
