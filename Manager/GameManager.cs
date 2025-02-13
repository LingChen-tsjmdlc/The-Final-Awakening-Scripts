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

    public GameObject ��Χ��Ƶ;
    public GameObject FxAudioFolder;

    /// <summary>
    /// ��ȡ UIManager ��ʵ������
    /// </summary>
    /// <returns>UIManager ʵ��</returns>
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("δ�ҵ�UIManage������");
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

        if (��Χ��Ƶ != null) 
        {
            DontDestroyOnLoad(��Χ��Ƶ);
        }
        if (FxAudioFolder != null)
        {
            DontDestroyOnLoad(FxAudioFolder);
        }
    }


    [Header("���")]
    public GameObject player;

    public bool ispause; //ȫ����ͣ����


    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "UIScenes")
        {
            GameObject player = GameObject.Find("Player");
        }
        else
        {
            Debug.Log("û���ҵ��������");
        }

        // ��Ƶ��ʼ��
        BGMVolume = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : -5.5f;    // ����Ƿ���Ĭ�ϵ� BGM ������С���ã����û������ΪĬ��
        BGMAudioMixer.SetFloat("BGMVolume", BGMVolume);
        FxVolume = PlayerPrefs.HasKey("FxVolume") ? PlayerPrefs.GetFloat("FxVolume") : -5.5f;    // ����Ƿ���Ĭ�ϵ� FX ������С���ã����û������ΪĬ��
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
    /// ���ص�ǰ����
    /// </summary>
    public void RestartNowScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// �˳���Ϸ
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
