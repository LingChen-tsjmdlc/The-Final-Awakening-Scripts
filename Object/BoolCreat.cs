using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolCreat : MonoBehaviour
{
    public int bollId;
    [SerializeField] private GameObject boll;
    [SerializeField] private Transform creatPoint;
    [SerializeField] private float force;
    private Rigidbody2D rb;

    private bool isPlayerInit;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    [SerializeField] private Direction ballDirection;
    private Vector2 directionVector;

    // 在Start方法中初始化方向向量
    void Start()
    {
        // 根据选定的方向设置方向向量
        switch (ballDirection)
        {
            case Direction.Up:
                directionVector = Vector2.up;
                break;
            case Direction.Down:
                directionVector = Vector2.down;
                break;
            case Direction.Left:
                directionVector = Vector2.left;
                break;
            case Direction.Right:
                directionVector = Vector2.right;
                break;
        }
    }

    private void Update()
    {
        if (InputManager.GetInstance().ObjectInteraction && isPlayerInit)
        {
            Throw();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInit = false;
        }
    }

    /// <summary>
    /// 创建激光球并扔出
    /// </summary>
    public void Throw()
    {
        // 在创建点实例化球
        GameObject instantiatedBoll = Instantiate(boll, creatPoint.position, Quaternion.identity);

        // Fx 音效
        FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().laserShoot);

        // 获取实例化球的Rigidbody2D组件
        Rigidbody2D rb = instantiatedBoll.GetComponent<Rigidbody2D>();
        // 获取球上的BollId脚本
        BollId bollIdScript = instantiatedBoll.GetComponent<BollId>();
        if (bollIdScript != null)
        {
            bollIdScript.id = bollId; // 确保设置ID在销毁其他球体之前
        }

        // 销毁相同 ID 的激光球，但排除新创建的球体
        DestroyBallsById(excludeNewBall: instantiatedBoll);

        if (rb != null)
        {
            // TODO:(调试用)设置刚体参数
            //rb.mass = ValueChange.instance.bollMass;
            //rb.drag = ValueChange.instance.bollLinearDarg;
            //rb.gravityScale = ValueChange.instance.bollGravityScale;
            Debug.Log("质量：" + rb.mass + "线性阻力：" + rb.drag + "重力大小：" + rb.gravityScale);

            // 对球施加力，使其沿着方向向量被扔出
            rb.AddForce(directionVector * force, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// 销毁相同 ID 的激光球，使得场景中只有一个相同 ID 的激光球
    /// </summary>
    public void DestroyBallsById(GameObject excludeNewBall = null)
    {
        // 获取当前场景中所有tag为"boll"的物体
        GameObject[] balls = GameObject.FindGameObjectsWithTag("boll");

        // 遍历所有找到的物体
        foreach (GameObject ball in balls)
        {
            // 跳过新创建的球体
            if (ball == excludeNewBall) continue;

            // 获取BollId脚本组件
            BollId bollIdScript = ball.GetComponent<BollId>();

            // 如果组件存在并且id值匹配，销毁这个物体
            if (bollIdScript != null && bollIdScript.id == bollId)
            {
                Destroy(ball);
            }
        }
    }
}
