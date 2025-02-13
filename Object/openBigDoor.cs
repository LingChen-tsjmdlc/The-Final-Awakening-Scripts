using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openBigDoor : MonoBehaviour
{
    [SerializeField]private Animator door1;
    [SerializeField]private Animator door2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                door1.SetBool("Open", true);
                door2.SetBool("Open", true);

                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().doorOpen);
            }
        }
    }
}
