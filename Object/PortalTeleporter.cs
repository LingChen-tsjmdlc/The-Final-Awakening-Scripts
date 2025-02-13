using System.Collections;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [Tooltip("��Ҽ��ٶȱ�Ƶ(Ĭ��:130f)")] public static float playerAccelerationMultiple = 1f;
    [Tooltip("��������ٶȱ�Ƶ(Ĭ��:3f)")] public static float bollAccelerationMultiple = 1f;

    public Transform otherPortal; // ��һ�������ŵ�Transform
    private Rigidbody2D playerRigidbody; // ��ҵ�Rigidbody2D
    private Rigidbody2D bollRigidbody;    // �������Rigidbody2D

    public Collider2D otherPortalCollider; // ��һ�������ŵ�Collider2D

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("�ڳ�����û���ҵ� Tag Ϊ ��Player�� �����壡");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && otherPortalCollider != null)
        {
            //TODO:������Ľ�ʹ�ô����ŵ�ʱ�䳤
            DisableOtherPortalColliderForDuration(1.5f);    // ����otherPortal�ϵ�Collider2D���0.5��
            StartCoroutine(DoTeleport(other));      // ִ�д����߼�
        }
        if (other.CompareTag("boll") && otherPortalCollider != null)
        {
            bollRigidbody = other.GetComponent<Rigidbody2D>();
            DisableOtherPortalColliderForDuration(0.5f);    // ����otherPortal�ϵ�Collider2D���0.5��
            StartCoroutine(DoTeleport(other));      // ִ�д����߼�
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="other">��ײ��</param>
    /// <returns>�ȴ�һ֡������һ����</returns>
    private IEnumerator DoTeleport(Collider2D other)
    {
        yield return new WaitForSeconds(0.001f); // �ȴ�һ֡��ȷ�������߼�ִ��

        //  ---> ���͵�����Ϊ��ҵ�ʱ��
        if (playerRigidbody != null && other.CompareTag("Player"))
        {
            FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().portalRed);   // ������Ч

            Vector3 relativePosition = other.transform.position - transform.position;
            other.transform.position = otherPortal.position + relativePosition;

            // ��ȡ��ҽ��봫����ʱ���ٶ�
            float playerSpeed = playerRigidbody.velocity.magnitude;
            Debug.Log("��ҽ��봫����ʱ����ٶ�Ϊ��" + playerSpeed);

            // �������ķ���ʹ��ָ��otherPortal��x����
            Vector2 forceDirection = otherPortal.right; // ʹ��right��������ô����ŵ�x����
            forceDirection.Normalize(); // ȷ�����������ĳ���Ϊ1

            // ����forceDirection����y��ļнǵ�����ֵ
            float angleCosine = Vector2.Dot(forceDirection, Vector2.up);

            // ��������ֵ����������С������ֵԽ�ӽ�1������Խ��Խ�ӽ�-1������ԽС
            float forceMultiplier = Mathf.Lerp(1f, 2f, (angleCosine + 1f) / 2f); // ������ֵ��[-1, 1]ӳ�䵽[1, 3]

            // ��������ٶȺͷ������ʩ�ӵ���
            float force = playerSpeed * playerAccelerationMultiple * forceMultiplier;

            // ʩ��һ��˲��������ƶ����
            playerRigidbody.AddForce(forceDirection * force, ForceMode2D.Impulse);
            // ���������ٶ�
            Debug.Log("��ҷ�������ȣ�" + force + "�������ٶȣ�" + playerRigidbody.velocity);
        }

        //  ---> ���͵�����Ϊ�������ʱ��
        if (bollRigidbody != null && other.CompareTag("boll"))
        {
            Vector3 relativePosition = other.transform.position - transform.position;
            other.transform.position = otherPortal.position + relativePosition;

            // ��ȡ��ҽ��봫����ʱ���ٶ�
            float bollSpeed = bollRigidbody.velocity.magnitude;
            Debug.Log("��������봫����ʱ����ٶ�Ϊ��" + bollSpeed);

            // �������ķ���ʹ��ָ��otherPortal��x����
            Vector2 forceDirection = otherPortal.right; // ʹ��right��������ô����ŵ�x����
            forceDirection.Normalize(); // ȷ�����������ĳ���Ϊ1

            // ����forceDirection����y��ļнǵ�����ֵ
            float angleCosine = Vector2.Dot(forceDirection, Vector2.up);

            // ��������ֵ����������С������ֵԽ�ӽ�1������Խ��Խ�ӽ�-1������ԽС
            float forceMultiplier = Mathf.Lerp(1f, 2f, (angleCosine + 1f) / 2f); // ������ֵ��[-1, 1]ӳ�䵽[1, 3]

            // ��������ٶȺͷ������ʩ�ӵ���
            float force = bollSpeed * bollAccelerationMultiple * forceMultiplier;

            // ʩ��һ��˲��������ƶ����
            bollRigidbody.AddForce(forceDirection * force, ForceMode2D.Impulse);
            // ���������ٶ�
            Debug.Log("����������ȣ�" + force + "��������ĸ����ٶȣ�" + bollRigidbody.velocity);
        }
    }

    /// <summary>
    /// ���ò��ӳ�������ײ��
    /// </summary>
    /// <param name="duration">�ӳ�ʱ��</param>
    private void DisableOtherPortalColliderForDuration(float duration)
    {
        if (otherPortalCollider != null)
        {
            otherPortalCollider.enabled = false;
            Invoke("EnableOtherPortalCollider", duration);
        }
    }

    /// <summary>
    /// ������ײ��
    /// </summary>
    private void EnableOtherPortalCollider()
    {
        if (otherPortalCollider != null) 
        {
            otherPortalCollider.enabled = true;
        }
    }
}
