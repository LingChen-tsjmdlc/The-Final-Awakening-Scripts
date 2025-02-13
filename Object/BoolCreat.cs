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

    // ��Start�����г�ʼ����������
    void Start()
    {
        // ����ѡ���ķ������÷�������
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
    /// �����������ӳ�
    /// </summary>
    public void Throw()
    {
        // �ڴ�����ʵ������
        GameObject instantiatedBoll = Instantiate(boll, creatPoint.position, Quaternion.identity);

        // Fx ��Ч
        FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().laserShoot);

        // ��ȡʵ�������Rigidbody2D���
        Rigidbody2D rb = instantiatedBoll.GetComponent<Rigidbody2D>();
        // ��ȡ���ϵ�BollId�ű�
        BollId bollIdScript = instantiatedBoll.GetComponent<BollId>();
        if (bollIdScript != null)
        {
            bollIdScript.id = bollId; // ȷ������ID��������������֮ǰ
        }

        // ������ͬ ID �ļ����򣬵��ų��´���������
        DestroyBallsById(excludeNewBall: instantiatedBoll);

        if (rb != null)
        {
            // TODO:(������)���ø������
            //rb.mass = ValueChange.instance.bollMass;
            //rb.drag = ValueChange.instance.bollLinearDarg;
            //rb.gravityScale = ValueChange.instance.bollGravityScale;
            Debug.Log("������" + rb.mass + "����������" + rb.drag + "������С��" + rb.gravityScale);

            // ����ʩ������ʹ�����ŷ����������ӳ�
            rb.AddForce(directionVector * force, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// ������ͬ ID �ļ�����ʹ�ó�����ֻ��һ����ͬ ID �ļ�����
    /// </summary>
    public void DestroyBallsById(GameObject excludeNewBall = null)
    {
        // ��ȡ��ǰ����������tagΪ"boll"������
        GameObject[] balls = GameObject.FindGameObjectsWithTag("boll");

        // ���������ҵ�������
        foreach (GameObject ball in balls)
        {
            // �����´���������
            if (ball == excludeNewBall) continue;

            // ��ȡBollId�ű����
            BollId bollIdScript = ball.GetComponent<BollId>();

            // ���������ڲ���idֵƥ�䣬�����������
            if (bollIdScript != null && bollIdScript.id == bollId)
            {
                Destroy(ball);
            }
        }
    }
}
