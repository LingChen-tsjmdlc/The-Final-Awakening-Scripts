using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformMove : MonoBehaviour
{
    [SerializeField] private GameObject[] points;   //点的设置
    [SerializeField] private float speed = 2f;  // 平台移动的速度

    private bool disableCollision = false; // 用于跟踪是否应该禁用碰撞
    private float disableCollisionTimer = 0f; // 用于计时禁用碰撞的时间

    private int pointNumber = 1; //点的取值
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

        // 按下 'R' 键时禁用碰撞
        if (Input.GetKeyDown(KeyCode.R))
        {
            disableCollision = true;
            disableCollisionTimer = 1f; // 设置禁用时间为 1 秒
        }

        // 更新禁用碰撞计时器
        if (disableCollision)
        {
            if (disableCollisionTimer > 0)
            {
                disableCollisionTimer -= Time.deltaTime;
            }
            else
            {
                disableCollision = false; // 时间到了，重新启用碰撞
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
        // 等待一帧，确保不在激活或禁用过程中设置父级
        yield return null;
        childTransform.SetParent(newParent);
    }

    private void OnDestroy()
    {
        // 停止所有协程
        StopAllCoroutines();
    }

}
