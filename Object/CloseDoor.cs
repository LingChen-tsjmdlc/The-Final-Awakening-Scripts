using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] private Animator doorClose;

    private bool isCloseed;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCloseed)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isCloseed = true;

                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().doorOpen);

                doorClose.SetBool("Close", true);
            }
        }
    }
}
