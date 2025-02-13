using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("����Canvas")] public GameObject canvas;
    [Header("Canvas��������")] public GameObject canvasObj;

    public static bool isPause; //ȫ����ͣ����

    private static UIManager instance;

    /// <summary>
    /// ��ȡ UIManager ��ʵ������
    /// </summary>
    /// <returns>UIManager ʵ��</returns>
    public static UIManager GetInstance()
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
    }

    private void Start()
    {
        if (!canvas)
        {
            canvas = GameObject.Find("Canvas");
        }
        FindPanel("StartUI");
        isPause = false;
    }

    private void Update()
    {
        DontDestroyOnLoad(canvas);
    }


    //Ѱ��Panel���
    public void FindPanel(string panelName)
    {
        canvasObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/"+panelName));
        canvasObj.transform.SetParent(canvas.gameObject.transform, false);
    }

    //ɾ��Panel����
    public void DestroyPanel(GameObject panel,bool isClear)
    {
        if (isClear)
        {
            foreach (Transform t in canvas.transform)
            {
                GameObject.Destroy(t.gameObject);
            }
        }
        else
        {
            GameObject.Destroy(panel);
        }
    }
}

