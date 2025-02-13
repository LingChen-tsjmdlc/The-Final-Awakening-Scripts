using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bollCatch : MonoBehaviour
{
    [SerializeField] private Transform catchPoint;
    [SerializeField] private float moveSpeed = 3f; // ����Ը�����Ҫ�����ٶ�
    [Header("���Ӧ�ķ�����")] [SerializeField] private GameObject correspondShooter;
    [Header("Ҫ�����¼�������")] [SerializeField]  private GameObject triggerEventsObject;
    [Header("")][SerializeField] private GameObject endPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����ײ�����tag�Ƿ�Ϊ"boll"
        if (collision.CompareTag("boll"))
        {
            bool bollIsRightId = correspondShooter.GetComponent<BoolCreat>().bollId == collision.GetComponent<BollId>().id ? true :false;
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (rb != null && bollIsRightId)
            {
                // ����Rigidbody2D���Է�ֹ����Ӱ��
                rb.isKinematic = true;

                // ʹ��Э��ƽ�����ƶ����嵽catchPoint
                StartCoroutine(MoveObjectToCatchPoint(rb.gameObject));
            }
        }
    }

    private IEnumerator MoveObjectToCatchPoint(GameObject obj)
    {
        if (obj != null)
        {
            // �����岻��catchPointλ��ʱ�������ƶ���
            while (Vector2.Distance(obj.transform.position, catchPoint.position) > 0.3f && obj != null)
            {
                obj.transform.position = Vector2.MoveTowards(obj.transform.position, catchPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // �����嵽��catchPointʱ��������
            Destroy(obj);
            // �ƶ�������
            triggerEventsObject.transform.position = endPoint.transform.position;
            // ����Э�̣���ֹ�����ĵ������Է��������ٵĶ���
            yield break;
        }
        else
        {
            Debug.LogWarning("����������Ϊ�գ�����");
        }
    }
}
