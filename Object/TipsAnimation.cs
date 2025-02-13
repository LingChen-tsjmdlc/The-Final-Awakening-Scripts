using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsAnimation : MonoBehaviour
{
    public float moveSpeedDown = 60f; // 向下移动的速度
    public float moveSpeedUp = 20f;   // 向上移动的速度
    public float moveDistance = 40f; // 移动的距离

    private RectTransform rectTransform; // UI组件的RectTransform
    private Vector2 downPosition;       // 向下移动的目标位置
    private Vector2 upPosition;         // 向上移动的目标位置
    private bool isMovingDown = true;   // 是否正在向下移动

    void Start()
    {
        // 获取RectTransform组件
        rectTransform = GetComponent<RectTransform>();

        // 计算向上和向下的目标位置
        upPosition = rectTransform.anchoredPosition;
        downPosition = upPosition - Vector2.up * moveDistance;
    }

    void Update()
    {
        // 根据移动方向选择目标位置和速度
        Vector2 targetPosition = isMovingDown ? downPosition : upPosition;
        float currentSpeed = isMovingDown ? moveSpeedDown : moveSpeedUp;

        // 平滑移动到目标位置
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, currentSpeed * Time.deltaTime);

        // 检查是否到达目标位置，并切换移动方向
        if ((isMovingDown && rectTransform.anchoredPosition.y <= downPosition.y) ||
            (!isMovingDown && rectTransform.anchoredPosition.y >= upPosition.y))
        {
            isMovingDown = !isMovingDown; // 切换移动方向
        }
    }
}
