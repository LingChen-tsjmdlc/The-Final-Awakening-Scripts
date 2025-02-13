using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformMove : MonoBehaviour
{
    [SerializeField] private GameObject[] points;   //�������
    [SerializeField] private float speed = 2f;  // ƽ̨�ƶ����ٶ�

    private bool disableCollision = false; // ���ڸ����Ƿ�Ӧ�ý�����ײ
    private float disableCollisionTimer = 0f; // ���ڼ�ʱ������ײ��ʱ��

    private int pointNumber = 1; //���ȡֵ
    private float waiteTime = 0.5f;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[pointNumber].transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, points[pointNumber].transform.position) < 0.1f)
        {
            if (waiteTime < 0)
            {
                if (pointNumber == 0)
                {
                    pointNumber = 1;
                }
                else
                {
                    pointNumber = 0;
                }
                waiteTime = 0.5f;
            }
            else
            {
                waiteTime -= Time.deltaTime;
            }
        }

        // ���� 'R' ��ʱ������ײ
        if (Input.GetKeyDown(KeyCode.R))
        {
            disableCollision = true;
            disableCollisionTimer = 1f; // ���ý���ʱ��Ϊ 1 ��
        }

        // ���½�����ײ��ʱ��
        if (disableCollision)
        {
            if (disableCollisionTimer > 0)
            {
                disableCollisionTimer -= Time.deltaTime;
            }
            else
            {
                disableCollision = false; // ʱ�䵽�ˣ�����������ײ
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && gameObject.activeInHierarchy)
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && gameObject.activeInHierarchy)
        {
            StartCoroutine(SetParentAfterDelay(collision.gameObject.transform, null));
        }
    }

    private IEnumerator SetParentAfterDelay(Transform childTransform, Transform newParent)
    {
        // �ȴ�һ֡��ȷ�����ڼ������ù��������ø���
        yield return null;
        childTransform.SetParent(newParent);
    }

    private void OnDestroy()
    {
        // ֹͣ����Э��
        StopAllCoroutines();
    }

}
