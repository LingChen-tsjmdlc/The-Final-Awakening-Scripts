using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorKeyCollisionDetector : MonoBehaviour
{
    public int keyId;
    public Vector2 offset; // ����һ����ά��������ʾ��Կ�׵�ƫ����
    public Vector2 discardOffset; // ����һ����ά��������ʾ������Կ�׵�ƫ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����ײ�������Ƿ������
        if (collision.CompareTag("Player"))
        {
            // ������������Ƿ��Ѿ��д���"Key"��ǩ������
            Transform existingKey = collision.transform.Find("Key");

            if (existingKey != null)
            {
                // ����Ѿ���һ��Կ�ף����滻��
                existingKey.SetParent(null); // ȡ����ǰԿ�׵ĸ������ϵ
                existingKey.gameObject.SetActive(true); // ȷ����Կ���Ǽ����
                existingKey.position = collision.transform.position + (Vector3)discardOffset; // ����Կ���Ƶ�����ƫ������λ��
            }

            // ����ǰԿ������Ϊ��ҵ������壬������ƫ����
            transform.SetParent(collision.transform, false);
            transform.localPosition = offset;
            transform.tag = "Key"; // ȷ����ǰԿ�׵ı�ǩΪ"Key"
        }
    }
}
