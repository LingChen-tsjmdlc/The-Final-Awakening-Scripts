using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel : MonoBehaviour
{
    [SerializeField] private Button closePanelButtonl;

    private void Update()
    {
        if (InputManager.GetInstance().BackButton)
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        UIManager.GetInstance().DestroyPanel(this.gameObject, false);
        ReviewTextOpen.isReviewTextPanelOpen = false;
    }
}
