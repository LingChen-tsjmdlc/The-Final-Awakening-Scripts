using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewTextOpen : MonoBehaviour
{
    /// <summary>
    /// 阻止再次打开的变量
    /// </summary>
    public static bool isReviewTextPanelOpen = false;

    private bool isOpen;

    private void Update()
    {
        if (!isOpen)
        {
            isOpen = InputManager.GetInstance().TextInteraction;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InputManager.GetInstance().TextInteraction && !isReviewTextPanelOpen)
            {
                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().openMschine);
                UIManager.GetInstance().FindPanel("ReviewText");
                isReviewTextPanelOpen = true;
                isOpen = false;
            }
        }
    }
}
