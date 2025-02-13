using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    private GameObject startPanel;

    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject settingButton; 
    [SerializeField] private GameObject aboutButton;
    [SerializeField] private GameObject ExitGameButton;

    private void Start()
    {
        if (startPanel == null)
        {
            startPanel = this.gameObject;
        }
    }

    /// <summary>
    /// 开始游戏按钮
    /// </summary>
    public void StartGame()
    {
        ScenesManager.GetInstance().TransitionToScene("0", 0.3f, 0.5f, () =>
        {
            ScenesManager.GetInstance().EnableColliders();
        });
        UIManager.GetInstance().DestroyPanel(this.gameObject, false);
        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().startUIButtonDownAudio);
    }

    /// <summary>
    /// 打开游戏设置
    /// </summary>
    public void OpenGameSetting()
    {
        UIManager.GetInstance().FindPanel("SettingUI");
        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().startUIButtonDownAudio);
    }

    public void OpenAbout()
    {
        UIManager.GetInstance().FindPanel("AboutUI");
        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().startUIButtonDownAudio);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
