using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject gameCamera;
    public Transform cameraCenter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ȷ��ֻ�е���ҽӴ���������ʱ���ƶ����
        {
            // ���������X��Y����ΪcameraCenter������
            Vector3 newPosition = cameraCenter.position;
            // �޸�Z����Ϊ-10
            newPosition.z = -10;
            // Ӧ���µ�λ�õ���Ϸ���
            gameCamera.transform.position = newPosition;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ȷ��ֻ�е���ҽӴ���������ʱ���ƶ����
        {
            // ���������X��Y����ΪcameraCenter������
            Vector3 newPosition = cameraCenter.position;
            // �޸�Z����Ϊ-10
            newPosition.z = -10;
            // Ӧ���µ�λ�õ���Ϸ���
            gameCamera.transform.position = newPosition;
        }
    }
}
