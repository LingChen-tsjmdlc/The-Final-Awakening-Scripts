using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewTextPanel : MonoBehaviour
{
    [SerializeField] private Button closePanelButtonl;

    public void ClosePanel()
    {
        UIManager.GetInstance().DestroyPanel(this.gameObject, false);
        ReviewTextOpen.isReviewTextPanelOpen = false;
    }
}
