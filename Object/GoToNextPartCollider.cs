using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextPartCollider : MonoBehaviour
{
    [Header("下一个场景的名称")] public string nextPartName;

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            ScenesManager.GetInstance().TransitionToScene(nextPartName, 0.4f, 0.6f, () =>
            {
                ScenesManager.GetInstance().EnableColliders();
                if (SceneManager.GetActiveScene().name == "UIScenes")
                {
                    UIManager.GetInstance().FindPanel("StartUI");
                }
            });
        }
    }
}
