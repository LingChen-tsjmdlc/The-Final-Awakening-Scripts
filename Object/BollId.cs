using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BollId : MonoBehaviour
{
    public int id; // 公开的id字段
    [Tooltip("射线检测的距离")] public float raycastDistance = 0.3f;
    [Tooltip("地面检测层级")] public LayerMask groundLayer;

    void Update()
    {
        // 检测小球是否触地
        //CheckIfBallTouchesGround();
    }

    void CheckIfBallTouchesGround()
    {
        // 从小球的底部位置向下发射射线
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance, Color.green);
        // 如果射线与地面碰撞
        if (hit.collider != null)
        {
            // 在这里处理小球触地后的逻辑，例如销毁小球
            Destroy(gameObject);
        }
    }
}
