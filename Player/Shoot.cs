using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float shootSpeed = 35f;
    public float offset = 0.4f; // 公开的偏移量
    public GameObject player;   // 玩家GameObject
    public GameObject redPortalPrefab;  // 红色传送门预制体
    public GameObject bluePortalPrefab; // 蓝色传送门预制体

    private GameObject currentPrefab; // 当前选中的预制体
    private Camera mainCamera; // 主相机，用于确定射击方向
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private Transform shootStartPos;

    private int redPortalCloneCount = 0;
    private int bluePortalCloneCount = 0;

    private bool useMouseL;
    private bool useMouseR;

    // 添加一个字典来存储协程和它们关联的GameObject
    private Dictionary<GameObject, Coroutine> runningCoroutines = new Dictionary<GameObject, Coroutine>();

    private void Awake()
    {
        currentPrefab = redPortalPrefab; // 默认为红色

        if (redPortalPrefab == null)
        {
            Debug.LogError("Failed to load Red Portal prefab.");
        }
        if (bluePortalPrefab == null)
        {
            Debug.LogError("Failed to load Blue Portal prefab.");
        }

        mainCamera = Camera.main; // 获取主相机
    }

    private void Update()
    {
        useMouseL = Input.GetMouseButtonDown(0);
        useMouseR = Input.GetMouseButtonDown(1);

        Transform isHasGun = gunPosition.transform.Find("传送枪");
        // 射击
        if (InputManager.GetInstance().ShootLeftBlue)
        {
            if (isHasGun != null)
            {
                Debug.Log("鼠标左键射击蓝色的传送门！");
                currentPrefab = bluePortalPrefab;
                ShootPortal();
            }
            else
            {
                Debug.LogWarning("还没有捡到枪!");
                useMouseL = false;
                useMouseR = false;
            }
        }
        if (InputManager.GetInstance().ShootRightRed)
        {
            if (isHasGun != null)
            {
                Debug.Log("鼠标右键射击红色的传送门！");
                currentPrefab = redPortalPrefab;
                ShootPortal();
            }
            else
            {
                Debug.LogWarning("还没有捡到枪!");
                useMouseL = false;
                useMouseR = false;
            }
        }
    }

    /// <summary>
    /// 射击传送门主方法
    /// </summary>
    private void ShootPortal()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = player.transform.position;
        // 计算射击方向
        Vector2 shootDirection = (mousePosition - playerPosition).normalized;   //鼠标的方向
        Vector2 rightStickAngle = InputManager.GetInstance().ShootAngle;        //手柄的方向

        // 判断是否使用手柄方向
        if (InputManager.GetInstance().IsUsingController && !useMouseL && !useMouseR) //如果使用手柄,且没有点击过鼠标
        {
            if (InputManager.GetInstance().ShootAngle == new Vector2 (0,0)) //如果使用手柄但是手柄没动，则使用默认值
            {
                shootDirection = PlayrtMovement.instance.gameObject.transform.forward;
            }
            else
            {
                shootDirection = rightStickAngle.normalized;
            }
        }

        useMouseL = false;
        useMouseR = false;

        // 获取除了"Default"和"Player"层之外的所有层的LayerMask
        LayerMask layerMask = ~(LayerMask.GetMask("Default", "Player"));
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, shootDirection, Mathf.Infinity, layerMask);
        // 绘制无限长的射线，持续时间设置为5秒
        Debug.DrawRay(playerPosition, shootDirection, Color.blue, 5.0f);

        if (hit.collider != null && Time.timeScale != 0)
        {
            Debug.Log("击中了对象：" + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Ground"))
            {
                // 检查碰撞点是否在屏幕之外
                Vector3 viewportPoint = mainCamera.WorldToViewportPoint(hit.point);
                bool isOffScreen = viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1;

                if (!isOffScreen)
                {
                    Debug.Log("击中了具有\"Ground\" tag的对象，且在屏幕内！，现在的击中的点是：" + hit.point);

                    //// 计算从玩家前方1个单位的地方射出的起始位置
                    //Vector2 shootStartPosition = playerPosition + shootDirection * 1f;

                    FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerShoot);   // 播放射击音效

                    // 计算偏移后的位置
                    Vector2 offsetPosition = hit.point - shootDirection * offset;

                    // 启动Coroutine来平滑移动射出物体
                    StartCoroutine(MovePortalSmoothly(new Vector2(shootStartPos.position.x, shootStartPos.position.y), offsetPosition, shootDirection, shootSpeed, hit));
                }
                else
                {
                    Debug.Log("击中的点在屏幕之外。");
                }
            }
        }
        else
        {
            Debug.Log("没有击中任何对象。");
        }
    }

    /// <summary>
    /// 平滑的移动传送门到指定位置
    /// </summary>
    /// <param name="startPosition">开始位置</param>
    /// <param name="endPosition">结束位置</param>
    /// <param name="shootDirection">射击方向</param>
    /// <param name="shootSpeed">射击速度</param>
    /// <returns></returns>
    private IEnumerator MovePortalSmoothly(Vector2 startPosition, Vector2 endPosition, Vector2 shootDirection, float shootSpeed, RaycastHit2D hit)
    {
        // 实例化物体在起始位置
        GameObject portalInstance = Instantiate(currentPrefab, startPosition, Quaternion.identity);
        // 重命名实例化的Portal
        if (currentPrefab == redPortalPrefab)
        {
            redPortalCloneCount++;
            portalInstance.name = "Portal Red(Clone" + redPortalCloneCount + ")";
        }
        else if (currentPrefab == bluePortalPrefab)
        {
            bluePortalCloneCount++;
            portalInstance.name = "Portal Blue(Clone" + bluePortalCloneCount + ")";
        }

        // 删除多余的Portals
        RemoveExtraPortals();
        yield return null;

        // 使用击中平面的法线来设置物体的旋转
        Vector2 normal = hit.normal;
        // 计算旋转角度，使物体垂直于击中平面
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        portalInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 计算起始位置和结束位置之间的距离
        float distanceToEnd = (endPosition - startPosition).magnitude;
        // 平滑移动物体到击中点
        float distanceTravelled = 0;
        while (distanceTravelled < distanceToEnd)
        {
            if (portalInstance == null)
            {
                yield break; // 如果对象不存在，退出协程
            }
            // 计算这一帧应该移动的距离
            float moveDistance = shootSpeed * Time.deltaTime;
            // 确保不会超过结束位置
            moveDistance = Mathf.Min(moveDistance, distanceToEnd - distanceTravelled);
            // 更新物体的位置
            portalInstance.transform.position += (Vector3)(shootDirection * moveDistance);
            // 更新已移动的距离
            distanceTravelled += moveDistance;
            yield return null;
        }

        if (portalInstance != null)
        {
            // 确保物体最终到达击中点
            portalInstance.transform.position = endPosition;
            // 链接Portals
            LinkPortals();
        }
    }

    /// <summary>
    /// 移除场景中多余的生成出来的传送门
    /// </summary>
    private void RemoveExtraPortals()
    {
        // 定义要查找的Portal预制体的名称前缀
        string redPortalNamePrefix = "Portal Red(Clone";
        string bluePortalNamePrefix = "Portal Blue(Clone";

        // 使用List来存储找到的Portals
        List<GameObject> redPortals = new List<GameObject>();
        List<GameObject> bluePortals = new List<GameObject>();

        // 查找场景中所有的Portals
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.name.StartsWith(redPortalNamePrefix))
            {
                redPortals.Add(obj);
            }
            else if (obj.name.StartsWith(bluePortalNamePrefix))
            {
                bluePortals.Add(obj);
            }
        }

        // 只保留红色Portal列表中编号最大的那个
        if (redPortals.Count > 1)
        {
            GameObject maxRedPortal = null;
            int maxIndex = -1;
            foreach (GameObject portal in redPortals)
            {
                string numberPart = portal.name.Substring(redPortalNamePrefix.Length, portal.name.Length - redPortalNamePrefix.Length - 1);
                if (int.TryParse(numberPart, out int index) && index > maxIndex)
                {
                    maxIndex = index;
                    maxRedPortal = portal;
                }
            }
            foreach (GameObject portal in redPortals)
            {
                if (portal != maxRedPortal)
                {
                    Destroy(portal);
                }
            }
        }

        // 只保留蓝色Portal列表中编号最大的那个
        if (bluePortals.Count > 1)
        {
            GameObject maxBluePortal = null;
            int maxIndex = -1;
            foreach (GameObject portal in bluePortals)
            {
                string numberPart = portal.name.Substring(bluePortalNamePrefix.Length, portal.name.Length - bluePortalNamePrefix.Length - 1);
                if (int.TryParse(numberPart, out int index) && index > maxIndex)
                {
                    maxIndex = index;
                    maxBluePortal = portal;
                }
            }
            foreach (GameObject portal in bluePortals)
            {
                if (portal != maxBluePortal)
                {
                    Destroy(portal);
                }
            }
        }
    }

    /// <summary>
    /// 链接红蓝传送门
    /// </summary>
    private void LinkPortals()
    {
        // 查找场景中数值最大的Portal Red(Clone{int})和Portal Blue(Clone{int})
        GameObject maxRedPortal = FindMaxClonePortal("Portal Red(Clone");
        GameObject maxBluePortal = FindMaxClonePortal("Portal Blue(Clone");

        if (maxRedPortal != null && maxBluePortal != null)
        {
            // 获取Portal Red(Clone{int})身上的PortalTeleporter脚本
            PortalTeleporter redPortalTeleporter = maxRedPortal.GetComponent<PortalTeleporter>();
            if (redPortalTeleporter != null)
            {
                // 设置PortalTeleporter脚本上的otherPortal和otherPortalCollider
                redPortalTeleporter.otherPortal = maxBluePortal.transform;
                redPortalTeleporter.otherPortalCollider = maxBluePortal.GetComponent<Collider2D>();
            }

            // 获取Portal Blue(Clone{int})身上的PortalTeleporter脚本
            PortalTeleporter bluePortalTeleporter = maxBluePortal.GetComponent<PortalTeleporter>();
            if (bluePortalTeleporter != null)
            {
                // 设置PortalTeleporter脚本上的otherPortal和otherPortalCollider
                bluePortalTeleporter.otherPortal = maxRedPortal.transform;
                bluePortalTeleporter.otherPortalCollider = maxRedPortal.GetComponent<Collider2D>();
            }
        }
    }

    /// <summary>
    /// 辅助方法，用于查找场景中数值最大的Portal Clone{int}
    /// </summary>
    /// <param name="portalNamePrefix">名字前缀</param>
    /// <returns>一个场景中数值最大的 Portal Clone 游戏物体</returns>
    private GameObject FindMaxClonePortal(string portalNamePrefix)
    {
        GameObject maxPortal = null;
        int maxIndex = -1;

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.name.StartsWith(portalNamePrefix))
            {
                string numberPart = obj.name.Substring(portalNamePrefix.Length, obj.name.Length - portalNamePrefix.Length - 1);
                if (int.TryParse(numberPart, out int index) && index > maxIndex)
                {
                    maxIndex = index;
                    maxPortal = obj;
                }
            }
        }

        return maxPortal;
    }
}
