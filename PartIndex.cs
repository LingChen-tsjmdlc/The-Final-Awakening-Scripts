using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����һ�����Ա����л��Ľṹ�����洢Transform��Ϣ
[System.Serializable]
public struct TransformData
{
    public Transform transform;
}

public class PartIndex : MonoBehaviour
{
    //����ģʽ����
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
        // ����partTransforms���飬��Ϊÿ��transform����MyScript�ű�
        foreach (var part in partTransforms)
        {
            // ���transform�Ƿ�Ϊnull
            if (part.transform != null)
            {
                // ����MyScript�ű���ÿ��transform
                CameraCtrl scriptInstance = Instantiate(myScriptPrefab, part.transform);
                scriptInstance.transform.localPosition = Vector3.zero;
            }
        }
    }
}
