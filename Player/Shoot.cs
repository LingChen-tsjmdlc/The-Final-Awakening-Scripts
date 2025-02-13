using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float shootSpeed = 35f;
    public float offset = 0.4f; // ������ƫ����
    public GameObject player;   // ���GameObject
    public GameObject redPortalPrefab;  // ��ɫ������Ԥ����
    public GameObject bluePortalPrefab; // ��ɫ������Ԥ����

    private GameObject currentPrefab; // ��ǰѡ�е�Ԥ����
    private Camera mainCamera; // �����������ȷ���������
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private Transform shootStartPos;

    private int redPortalCloneCount = 0;
    private int bluePortalCloneCount = 0;

    private bool useMouseL;
    private bool useMouseR;

    // ���һ���ֵ����洢Э�̺����ǹ�����GameObject
    private Dictionary<GameObject, Coroutine> runningCoroutines = new Dictionary<GameObject, Coroutine>();

    private void Awake()
    {
        currentPrefab = redPortalPrefab; // Ĭ��Ϊ��ɫ

        if (redPortalPrefab == null)
        {
            Debug.LogError("Failed to load Red Portal prefab.");
        }
        if (bluePortalPrefab == null)
        {
            Debug.LogError("Failed to load Blue Portal prefab.");
        }

        mainCamera = Camera.main; // ��ȡ�����
    }

    private void Update()
    {
        useMouseL = Input.GetMouseButtonDown(0);
        useMouseR = Input.GetMouseButtonDown(1);

        Transform isHasGun = gunPosition.transform.Find("����ǹ");
        // ���
        if (InputManager.GetInstance().ShootLeftBlue)
        {
            if (isHasGun != null)
            {
                Debug.Log("�����������ɫ�Ĵ����ţ�");
                currentPrefab = bluePortalPrefab;
                ShootPortal();
            }
            else
            {
                Debug.LogWarning("��û�м�ǹ!");
                useMouseL = false;
                useMouseR = false;
            }
        }
        if (InputManager.GetInstance().ShootRightRed)
        {
            if (isHasGun != null)
            {
                Debug.Log("����Ҽ������ɫ�Ĵ����ţ�");
                currentPrefab = redPortalPrefab;
                ShootPortal();
            }
            else
            {
                Debug.LogWarning("��û�м�ǹ!");
                useMouseL = false;
                useMouseR = false;
            }
        }
    }

    /// <summary>
    /// ���������������
    /// </summary>
    private void ShootPortal()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = player.transform.position;
        // �����������
        Vector2 shootDirection = (mousePosition - playerPosition).normalized;   //���ķ���
        Vector2 rightStickAngle = InputManager.GetInstance().ShootAngle;        //�ֱ��ķ���

        // �ж��Ƿ�ʹ���ֱ�����
        if (InputManager.GetInstance().IsUsingController && !useMouseL && !useMouseR) //���ʹ���ֱ�,��û�е�������
        {
            if (InputManager.GetInstance().ShootAngle == new Vector2 (0,0)) //���ʹ���ֱ������ֱ�û������ʹ��Ĭ��ֵ
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

        // ��ȡ����"Default"��"Player"��֮������в��LayerMask
        LayerMask layerMask = ~(LayerMask.GetMask("Default", "Player"));
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, shootDirection, Mathf.Infinity, layerMask);
        // �������޳������ߣ�����ʱ������Ϊ5��
        Debug.DrawRay(playerPosition, shootDirection, Color.blue, 5.0f);

        if (hit.collider != null && Time.timeScale != 0)
        {
            Debug.Log("�����˶���" + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Ground"))
            {
                // �����ײ���Ƿ�����Ļ֮��
                Vector3 viewportPoint = mainCamera.WorldToViewportPoint(hit.point);
                bool isOffScreen = viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1;

                if (!isOffScreen)
                {
                    Debug.Log("�����˾���\"Ground\" tag�Ķ���������Ļ�ڣ������ڵĻ��еĵ��ǣ�" + hit.point);

                    //// ��������ǰ��1����λ�ĵط��������ʼλ��
                    //Vector2 shootStartPosition = playerPosition + shootDirection * 1f;

                    FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerShoot);   // ���������Ч

                    // ����ƫ�ƺ��λ��
                    Vector2 offsetPosition = hit.point - shootDirection * offset;

                    // ����Coroutine��ƽ���ƶ��������
                    StartCoroutine(MovePortalSmoothly(new Vector2(shootStartPos.position.x, shootStartPos.position.y), offsetPosition, shootDirection, shootSpeed, hit));
                }
                else
                {
                    Debug.Log("���еĵ�����Ļ֮�⡣");
                }
            }
        }
        else
        {
            Debug.Log("û�л����κζ���");
        }
    }

    /// <summary>
    /// ƽ�����ƶ������ŵ�ָ��λ��
    /// </summary>
    /// <param name="startPosition">��ʼλ��</param>
    /// <param name="endPosition">����λ��</param>
    /// <param name="shootDirection">�������</param>
    /// <param name="shootSpeed">����ٶ�</param>
    /// <returns></returns>
    private IEnumerator MovePortalSmoothly(Vector2 startPosition, Vector2 endPosition, Vector2 shootDirection, float shootSpeed, RaycastHit2D hit)
    {
        // ʵ������������ʼλ��
        GameObject portalInstance = Instantiate(currentPrefab, startPosition, Quaternion.identity);
        // ������ʵ������Portal
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

        // ɾ�������Portals
        RemoveExtraPortals();
        yield return null;

        // ʹ�û���ƽ��ķ����������������ת
        Vector2 normal = hit.normal;
        // ������ת�Ƕȣ�ʹ���崹ֱ�ڻ���ƽ��
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        portalInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // ������ʼλ�úͽ���λ��֮��ľ���
        float distanceToEnd = (endPosition - startPosition).magnitude;
        // ƽ���ƶ����嵽���е�
        float distanceTravelled = 0;
        while (distanceTravelled < distanceToEnd)
        {
            if (portalInstance == null)
            {
                yield break; // ������󲻴��ڣ��˳�Э��
            }
            // ������һ֡Ӧ���ƶ��ľ���
            float moveDistance = shootSpeed * Time.deltaTime;
            // ȷ�����ᳬ������λ��
            moveDistance = Mathf.Min(moveDistance, distanceToEnd - distanceTravelled);
            // ���������λ��
            portalInstance.transform.position += (Vector3)(shootDirection * moveDistance);
            // �������ƶ��ľ���
            distanceTravelled += moveDistance;
            yield return null;
        }

        if (portalInstance != null)
        {
            // ȷ���������յ�����е�
            portalInstance.transform.position = endPosition;
            // ����Portals
            LinkPortals();
        }
    }

    /// <summary>
    /// �Ƴ������ж�������ɳ����Ĵ�����
    /// </summary>
    private void RemoveExtraPortals()
    {
        // ����Ҫ���ҵ�PortalԤ���������ǰ׺
        string redPortalNamePrefix = "Portal Red(Clone";
        string bluePortalNamePrefix = "Portal Blue(Clone";

        // ʹ��List���洢�ҵ���Portals
        List<GameObject> redPortals = new List<GameObject>();
        List<GameObject> bluePortals = new List<GameObject>();

        // ���ҳ��������е�Portals
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

        // ֻ������ɫPortal�б��б�������Ǹ�
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

        // ֻ������ɫPortal�б��б�������Ǹ�
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
    /// ���Ӻ���������
    /// </summary>
    private void LinkPortals()
    {
        // ���ҳ�������ֵ����Portal Red(Clone{int})��Portal Blue(Clone{int})
        GameObject maxRedPortal = FindMaxClonePortal("Portal Red(Clone");
        GameObject maxBluePortal = FindMaxClonePortal("Portal Blue(Clone");

        if (maxRedPortal != null && maxBluePortal != null)
        {
            // ��ȡPortal Red(Clone{int})���ϵ�PortalTeleporter�ű�
            PortalTeleporter redPortalTeleporter = maxRedPortal.GetComponent<PortalTeleporter>();
            if (redPortalTeleporter != null)
            {
                // ����PortalTeleporter�ű��ϵ�otherPortal��otherPortalCollider
                redPortalTeleporter.otherPortal = maxBluePortal.transform;
                redPortalTeleporter.otherPortalCollider = maxBluePortal.GetComponent<Collider2D>();
            }

            // ��ȡPortal Blue(Clone{int})���ϵ�PortalTeleporter�ű�
            PortalTeleporter bluePortalTeleporter = maxBluePortal.GetComponent<PortalTeleporter>();
            if (bluePortalTeleporter != null)
            {
                // ����PortalTeleporter�ű��ϵ�otherPortal��otherPortalCollider
                bluePortalTeleporter.otherPortal = maxRedPortal.transform;
                bluePortalTeleporter.otherPortalCollider = maxRedPortal.GetComponent<Collider2D>();
            }
        }
    }

    /// <summary>
    /// �������������ڲ��ҳ�������ֵ����Portal Clone{int}
    /// </summary>
    /// <param name="portalNamePrefix">����ǰ׺</param>
    /// <returns>һ����������ֵ���� Portal Clone ��Ϸ����</returns>
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
