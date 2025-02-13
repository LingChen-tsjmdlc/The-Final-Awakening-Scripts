using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOneDoor : MonoBehaviour
{
    [SerializeField] private GameObject closeSprite;
    [SerializeField] private GameObject openSprite;

    [SerializeField] private Animator door;

    private bool isOpen;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpen = true;

                closeSprite.SetActive(false);
                openSprite.SetActive(true);

                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().doorOpen);
                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().switchPole);

                door.SetBool("Open", true);
            }
        }
    }
}
