using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 创建一个可以被序列化的结构体来存储Transform信息
[System.Serializable]
public struct TransformData
{
    public Transform transform;
}

public class PartIndex : MonoBehaviour
{
    //单例模式设置
    public static PartIndex instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }

    public CameraCtrl myScriptPrefab;
    public TransformData[] partTransforms;

    private void Start()
    {
        // 遍历partTransforms数组，并为每个transform挂载MyScript脚本
        foreach (var part in partTransforms)
        {
            // 检查transform是否为null
            if (part.transform != null)
            {
                // 挂载MyScript脚本到每个transform
                CameraCtrl scriptInstance = Instantiate(myScriptPrefab, part.transform);
                scriptInstance.transform.localPosition = Vector3.zero;
            }
        }
    }
}
