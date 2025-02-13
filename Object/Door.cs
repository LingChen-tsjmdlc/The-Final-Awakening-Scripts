using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public int doorId;
    public string animaTriggerName;


    private void Awake()
    {
        //animator = GetComponent<Animator>();
        Debug.Log("��ǰ�Ķ������� " +  gameObject.name + " ��");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����ײ�������Ƿ����"Key"��ǩ
        if (collision.CompareTag("Key"))
        {
            // ��ȡ���д���"Key"��ǩ������
            GameObject[] keys = GameObject.FindGameObjectsWithTag("Key");

            // �������д���"Key"��ǩ������
            foreach (GameObject key in keys)
            {
                // ��ȡ�����ϵ�DoorKeyCollisionDetector�ű�
                DoorKeyCollisionDetector keyScript = key.GetComponent<DoorKeyCollisionDetector>();

                // ����ű����ڣ�����keyId��doorId���
                if (keyScript != null && keyScript.keyId == doorId)
                {
                    Debug.Log("��ǰ��Կ��ID��" + keyScript.keyId + ",��ǰ�ŵ�ID��" + doorId + ",��ǰ�Ĵ��������֣�" + animaTriggerName);
                    Open();
                    Destroy(keyScript.gameObject);
                    Invoke(nameof(DestroyDoor), 2f);
                }
            }
        }
    }

    [ContextMenu("�������")]
    public void Open()
    {
        animator.SetTrigger(animaTriggerName);
    }

    private void DestroyDoor()
    {
        Destroy(this.gameObject);
    }
}
