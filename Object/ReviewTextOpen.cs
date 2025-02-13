using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewTextOpen : MonoBehaviour
{
    public static bool isReviewTextPanelOpen = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && !isReviewTextPanelOpen)
            {
                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().openMschine);
                UIManager.GetInstance().FindPanel("ReviewText");
                isReviewTextPanelOpen = true;
            }
        }
    }
}
