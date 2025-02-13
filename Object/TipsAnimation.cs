using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsAnimation : MonoBehaviour
{
    public float moveSpeedDown = 60f; // �����ƶ����ٶ�
    public float moveSpeedUp = 20f;   // �����ƶ����ٶ�
    public float moveDistance = 40f; // �ƶ��ľ���

    private RectTransform rectTransform; // UI�����RectTransform
    private Vector2 downPosition;       // �����ƶ���Ŀ��λ��
    private Vector2 upPosition;         // �����ƶ���Ŀ��λ��
    private bool isMovingDown = true;   // �Ƿ����������ƶ�

    void Start()
    {
        // ��ȡRectTransform���
        rectTransform = GetComponent<RectTransform>();

        // �������Ϻ����µ�Ŀ��λ��
        upPosition = rectTransform.anchoredPosition;
        downPosition = upPosition - Vector2.up * moveDistance;
    }

    void Update()
    {
        // �����ƶ�����ѡ��Ŀ��λ�ú��ٶ�
        Vector2 targetPosition = isMovingDown ? downPosition : upPosition;
        float currentSpeed = isMovingDown ? moveSpeedDown : moveSpeedUp;

        // ƽ���ƶ���Ŀ��λ��
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, currentSpeed * Time.deltaTime);

        // ����Ƿ񵽴�Ŀ��λ�ã����л��ƶ�����
        if ((isMovingDown && rectTransform.anchoredPosition.y <= downPosition.y) ||
            (!isMovingDown && rectTransform.anchoredPosition.y >= upPosition.y))
        {
            isMovingDown = !isMovingDown; // �л��ƶ�����
        }
    }
}
