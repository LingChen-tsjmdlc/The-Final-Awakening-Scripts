using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
    public static void InputControlChanged(int inputControlIndex)
    {
        if (inputControlIndex == 0)
        {
            Debug.Log("ʹ��Ĭ�������豸��");
        }
        else if (inputControlIndex == 1)
        {
            Debug.Log("ʹ�ü��������豸��");
        }
        else if (inputControlIndex == 2)
        {
            Debug.Log("ʹ���ֱ������豸��");
        }
        else
        {
            Debug.LogWarning($"���������쳣, ��ǰ����Ϊ��{inputControlIndex}");
        }
    }
}
