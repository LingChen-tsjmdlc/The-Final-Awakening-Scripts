using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEndding : MonoBehaviour
{
    [Header("下一个场景的名称")] public string nextPartName;

    private void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ScenesManager.GetInstance().TransitionToScene(nextPartName, 0.4f, 0.6f, () =>
                {
                    FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().openMschine);
                    ScenesManager.GetInstance().EnableColliders();
                    if (SceneManager.GetActiveScene().name == "UIScenes")
                    {
                        UIManager.GetInstance().FindPanel("StartUI");
                    }
                });
            }
        }
    }
}
