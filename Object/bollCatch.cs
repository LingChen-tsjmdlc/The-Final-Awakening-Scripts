using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bollCatch : MonoBehaviour
{
    [SerializeField] private Transform catchPoint;
    [SerializeField] private float moveSpeed = 3f; // 你可以根据需要调整速度
    [Header("相对应的发射器")] [SerializeField] private GameObject correspondShooter;
    [Header("要触发事件的物体")] [SerializeField]  private GameObject triggerEventsObject;
    [Header("")][SerializeField] private GameObject endPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞物体的tag是否为"boll"
        if (collision.CompareTag("boll"))
        {
            bool bollIsRightId = correspondShooter.GetComponent<BoolCreat>().bollId == collision.GetComponent<BollId>().id ? true :false;
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (rb != null && bollIsRightId)
            {
                // 禁用Rigidbody2D，以防止物理影响
                rb.isKinematic = true;

                // 使用协程平滑地移动物体到catchPoint
                StartCoroutine(MoveObjectToCatchPoint(rb.gameObject));
            }
        }
    }

    private IEnumerator MoveObjectToCatchPoint(GameObject obj)
    {
        if (obj != null)
        {
            // 当物体不在catchPoint位置时，持续移动它
            while (Vector2.Distance(obj.transform.position, catchPoint.position) > 0.3f && obj != null)
            {
                obj.transform.position = Vector2.MoveTowards(obj.transform.position, catchPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 当物体到达catchPoint时，销毁它
            Destroy(obj);
            // 移动传送门
            triggerEventsObject.transform.position = endPoint.transform.position;
            // 结束协程，防止后续的迭代尝试访问已销毁的对象
            yield break;
        }
        else
        {
            Debug.LogWarning("激光球物体为空！！！");
        }
    }
}
