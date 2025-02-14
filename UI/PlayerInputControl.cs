using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
    public static void InputControlChanged(int inputControlIndex)
    {
        if (inputControlIndex == 0)
        {
            Debug.Log("使用默认输入设备。");
        }
        else if (inputControlIndex == 1)
        {
            Debug.Log("使用键鼠输入设备。");
        }
        else if (inputControlIndex == 2)
        {
            Debug.Log("使用手柄输入设备。");
        }
        else
        {
            Debug.LogWarning($"输入索引异常, 当前索引为：{inputControlIndex}");
        }
    }
}
