using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("场景Canvas")] public GameObject canvas;
    [Header("Canvas挂载物体")] public GameObject canvasObj;

    public static bool isPause; //全局暂停变量

    private static UIManager instance;

    /// <summary>
    /// 获取 UIManager 的实例对象
    /// </summary>
    /// <returns>UIManager 实例</returns>
    public static UIManager GetInstance()
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


    //寻找Panel面板
    public void FindPanel(string panelName)
    {
        canvasObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/"+panelName));
        canvasObj.transform.SetParent(canvas.gameObject.transform, false);
    }

    //删除Panel物体
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

