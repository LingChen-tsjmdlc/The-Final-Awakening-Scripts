using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartPanel : MonoBehaviour
{
    private GameObject startPanel;

    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject settingButton; 
    [SerializeField] private GameObject aboutButton;
    [SerializeField] private GameObject ExitGameButton;

    [Header("��һ��ѡ���ѡ��")]
    [SerializeField] private GameObject menuFisrt;

    private void Start()
    {
        if (startPanel == null)
        {
            startPanel = this.gameObject;
        }

        EventSystem.current.SetSelectedGameObject(menuFisrt);
    }

    /// <summary>
    /// ��ʼ��Ϸ��ť
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
    /// ����Ϸ����
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
    /// �˳���Ϸ
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
