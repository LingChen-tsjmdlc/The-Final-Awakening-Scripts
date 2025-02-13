using System.Collections;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [Tooltip("玩家加速度倍频(默认:130f)")] public static float playerAccelerationMultiple = 1f;
    [Tooltip("激光球加速度倍频(默认:3f)")] public static float bollAccelerationMultiple = 1f;

    public Transform otherPortal; // 另一个传送门的Transform
    private Rigidbody2D playerRigidbody; // 玩家的Rigidbody2D
    private Rigidbody2D bollRigidbody;    // 激光球的Rigidbody2D

    public Collider2D otherPortalCollider; // 另一个传送门的Collider2D

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("在场景中没有找到 Tag 为 “Player” 的物体！");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && otherPortalCollider != null)
        {
            //TODO:在这里改禁使用传送门的时间长
            DisableOtherPortalColliderForDuration(1.5f);    // 禁用otherPortal上的Collider2D组件0.5秒
            StartCoroutine(DoTeleport(other));      // 执行传送逻辑
        }
        if (other.CompareTag("boll") && otherPortalCollider != null)
        {
            bollRigidbody = other.GetComponent<Rigidbody2D>();
            DisableOtherPortalColliderForDuration(0.5f);    // 禁用otherPortal上的Collider2D组件0.5秒
            StartCoroutine(DoTeleport(other));      // 执行传送逻辑
        }
    }

    /// <summary>
    /// 传送主方法
    /// </summary>
    /// <param name="other">碰撞体</param>
    /// <returns>等待一帧后给玩家一个力</returns>
    private IEnumerator DoTeleport(Collider2D other)
    {
        yield return new WaitForSeconds(0.001f); // 等待一帧，确保禁用逻辑执行

        //  ---> 传送的物体为玩家的时候
        if (playerRigidbody != null && other.CompareTag("Player"))
        {
            FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().portalRed);   // 播放音效

            Vector3 relativePosition = other.transform.position - transform.position;
            other.transform.position = otherPortal.position + relativePosition;

            // 获取玩家进入传送门时的速度
            float playerSpeed = playerRigidbody.velocity.magnitude;
            Debug.Log("玩家进入传送门时候的速度为：" + playerSpeed);

            // 计算力的方向，使其指向otherPortal的x方向
            Vector2 forceDirection = otherPortal.right; // 使用right向量来获得传送门的x方向
            forceDirection.Normalize(); // 确保方向向量的长度为1

            // 计算forceDirection与正y轴的夹角的余弦值
            float angleCosine = Vector2.Dot(forceDirection, Vector2.up);

            // 根据余弦值调整力量大小，余弦值越接近1，力量越大，越接近-1，力量越小
            float forceMultiplier = Mathf.Lerp(1f, 2f, (angleCosine + 1f) / 2f); // 将余弦值从[-1, 1]映射到[1, 3]

            // 根据玩家速度和方向计算施加的力
            float force = playerSpeed * playerAccelerationMultiple * forceMultiplier;

            // 施加一个瞬间的力来移动玩家
            playerRigidbody.AddForce(forceDirection * force, ForceMode2D.Impulse);
            // 输出刚体的速度
            Debug.Log("玩家发射的力度：" + force + "，刚体速度：" + playerRigidbody.velocity);
        }

        //  ---> 传送的物体为激光球的时候
        if (bollRigidbody != null && other.CompareTag("boll"))
        {
            Vector3 relativePosition = other.transform.position - transform.position;
            other.transform.position = otherPortal.position + relativePosition;

            // 获取玩家进入传送门时的速度
            float bollSpeed = bollRigidbody.velocity.magnitude;
            Debug.Log("激光球进入传送门时候的速度为：" + bollSpeed);

            // 计算力的方向，使其指向otherPortal的x方向
            Vector2 forceDirection = otherPortal.right; // 使用right向量来获得传送门的x方向
            forceDirection.Normalize(); // 确保方向向量的长度为1

            // 计算forceDirection与正y轴的夹角的余弦值
            float angleCosine = Vector2.Dot(forceDirection, Vector2.up);

            // 根据余弦值调整力量大小，余弦值越接近1，力量越大，越接近-1，力量越小
            float forceMultiplier = Mathf.Lerp(1f, 2f, (angleCosine + 1f) / 2f); // 将余弦值从[-1, 1]映射到[1, 3]

            // 根据玩家速度和方向计算施加的力
            float force = bollSpeed * bollAccelerationMultiple * forceMultiplier;

            // 施加一个瞬间的力来移动玩家
            bollRigidbody.AddForce(forceDirection * force, ForceMode2D.Impulse);
            // 输出刚体的速度
            Debug.Log("激光球的力度：" + force + "，激光球的刚体速度：" + bollRigidbody.velocity);
        }
    }

    /// <summary>
    /// 禁用并延迟启用碰撞体
    /// </summary>
    /// <param name="duration">延迟时间</param>
    private void DisableOtherPortalColliderForDuration(float duration)
    {
        if (otherPortalCollider != null)
        {
            otherPortalCollider.enabled = false;
            Invoke("EnableOtherPortalCollider", duration);
        }
    }

    /// <summary>
    /// 启用碰撞体
    /// </summary>
    private void EnableOtherPortalCollider()
    {
        if (otherPortalCollider != null) 
        {
            otherPortalCollider.enabled = true;
        }
    }
}
