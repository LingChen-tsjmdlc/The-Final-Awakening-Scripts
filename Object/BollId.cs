using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BollId : MonoBehaviour
{
    public int id; // ������id�ֶ�
    [Tooltip("���߼��ľ���")] public float raycastDistance = 0.3f;
    [Tooltip("������㼶")] public LayerMask groundLayer;

    void Update()
    {
        // ���С���Ƿ񴥵�
        //CheckIfBallTouchesGround();
    }

    void CheckIfBallTouchesGround()
    {
        // ��С��ĵײ�λ�����·�������
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance, Color.green);
        // ��������������ײ
        if (hit.collider != null)
        {
            // �����ﴦ��С�򴥵غ���߼�����������С��
            Destroy(gameObject);
        }
    }
}
