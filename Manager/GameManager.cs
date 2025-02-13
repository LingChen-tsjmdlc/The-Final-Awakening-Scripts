using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private float BGMVolume;
    [SerializeField]private AudioMixer BGMAudioMixer;
    private float FxVolume;
    [SerializeField]private AudioMixer FxAudioMixer;

    public GameObject 氛围音频;
    public GameObject FxAudioFolder;

    /// <summary>
    /// 获取 UIManager 的实例对象
    /// </summary>
    /// <returns>UIManager 实例</returns>
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("未找到UIManage单例！");
            return null;
        }
        return instance;
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (氛围音频 != null) 
        {
            DontDestroyOnLoad(氛围音频);
        }
        if (FxAudioFolder != null)
        {
            DontDestroyOnLoad(FxAudioFolder);
        }
    }


    [Header("玩家")]
    public GameObject player;

    public bool ispause; //全局暂停变量


    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "UIScenes")
        {
            GameObject player = GameObject.Find("Player");
        }
        else
        {
            Debug.Log("没有找到玩家物体");
        }

        // 音频初始化
        BGMVolume = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : -5.5f;    // 检查是否有默认的 BGM 音量大小设置，如果没有设置为默认
        BGMAudioMixer.SetFloat("BGMVolume", BGMVolume);
        FxVolume = PlayerPrefs.HasKey("FxVolume") ? PlayerPrefs.GetFloat("FxVolume") : -5.5f;    // 检查是否有默认的 FX 音量大小设置，如果没有设置为默认
        FxAudioMixer.SetFloat("FxVolume", FxVolume);
    }

    private void Update()
    {
        if(InputManager.GetInstance().Esc && SceneManager.GetActiveScene().name != "UIScenes" && !ispause)
        {
            UIManager.GetInstance().FindPanel("SettingUI");
            Time.timeScale = 0;
            ispause = true;
        }
    }



    /// <summary>
    /// 重载当前场景
    /// </summary>
    public void RestartNowScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
