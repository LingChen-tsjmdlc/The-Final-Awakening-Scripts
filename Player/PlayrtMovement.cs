using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayrtMovement : MonoBehaviour
{
    //����ģʽ����
    public static PlayrtMovement instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }

    public float speed;     // �ƶ��ٶ�
    public float jumpForce; //��Ծ����
    public Transform groundCheck;   // һ���ڽ�ɫ���µ�Transform����������ɫ�Ƿ��ڵ����ϡ�
    public float checkDistance;       // ���߼��ľ��롣
    public LayerMask whatIsGround;  // һ��LayerMask������������Щ�㱻��Ϊ�ǵ��档

    private Rigidbody2D rb;
    private Animator playerAnimator;
    [SerializeField] private bool isGrounded;
    public static bool wasGrounded; // ����������һ֡�Ƿ��ڵ�����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //Run();
        //Jump();
    }

    private void Update()
    {
        Run();
        Jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // �������ߣ������ڿ��ӻ�
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * checkDistance);
    }

    // ���������һ��ƽ���ٶ�ϵ��
    public float smoothSpeed = 10f;
    private void Run()
    {
        float runValue = Input.GetAxis("Horizontal");
        // �����������ý�ɫ��ˮƽ�ٶ�
        if (runValue != 0)
        {
            //Debug.Log("runValue��ֵ");
            playerAnimator.SetBool("isWalk", true);
            // �ж��Ƿ��ڵ�����
            if (isGrounded)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                // ʹ��transform.position���ƶ���ɫ
                Vector3 newPosition = transform.position;
                newPosition.x += runValue * speed * Time.deltaTime;
                transform.position = newPosition;
            }
            else
            {
                // ʹ��Rigidbody2D��ƽ�����ƶ���ɫ
                float targetSpeed = runValue * speed;
                float smoothedSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, Time.fixedDeltaTime * smoothSpeed);
                rb.velocity = new Vector2(smoothedSpeed, rb.velocity.y);
            }
            Flip(runValue);
            // ����Ƿ��Ѿ��ڲ����ܲ���Ч�����û�У��򲥷�
            if (!FxAudioManager.GetInstance().playerRun.isPlaying)
            {
                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerRun);
                FxAudioManager.GetInstance().playerRun.loop = true;
            }
        }
        else
        {
            // ��ֹͣ�ƶ�ʱ��ֹͣ�����ܲ���Ч
            if (FxAudioManager.GetInstance().playerRun.isPlaying)
            {
                FxAudioManager.GetInstance().StopSound(FxAudioManager.GetInstance().playerRun);
                FxAudioManager.GetInstance().playerRun.loop = false;
            }
            playerAnimator.SetBool("isWalk", false);
        }
    }

    private void Jump()
    {
        // ʹ�����߼���Ƿ��ڵ�����
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, whatIsGround);
        isGrounded = hit.collider != null;
        // ����ɫ�ӿ������ʱ
        if (!wasGrounded && isGrounded)
        {
            playerAnimator.SetBool("isFalling", false);
            playerAnimator.SetBool("jump", false);
        }
        // ������Ծ����
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            playerAnimator.SetBool("jump", true);

            FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerJump);   // ������Ч
        }
        // ����ɫ�뿪����ʱ�����ٶ�Ϊ�����������׶�
        else if (!isGrounded && rb.velocity.y > 0)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("jump", true);
            playerAnimator.SetBool("isFalling", false);
        }
        // ����ɫ�뿪����ʱ�����ٶ�Ϊ����������׶�
        else if (!isGrounded && rb.velocity.y <= 0)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("jump", false);
            playerAnimator.SetBool("isFalling", true);
        }

        // ����wasGrounded������Ϊ��һ֡��׼��
        wasGrounded = isGrounded;
    }

    private void Flip(float horizontal)
    {
        // ����ˮƽ�ƶ�����ת��ɫ
        if (horizontal > 0 && transform.localScale.x < 0f || horizontal < 0 && transform.localScale.x > 0f)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }
    }

    public void Die()
    {
        Debug.Log("������");
        FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerDie);   // ������Ч
        playerAnimator.SetBool("die", true);
        rb.bodyType = RigidbodyType2D.Static;
        Invoke(nameof(DiedAndRestart), 0.5f);
    }

    public void DiedAndRestart()
    {
        GameManager.GetInstance().RestartNowScene();
    }

    /// <summary>
    /// ֹͣ��ҵ��ƶ�����Ծ
    /// </summary>
    public void StopMovementAndJumping()
    {
        // ֹͣ�ƶ�����Rigidbody���ٶ�����Ϊ0
        rb.velocity = Vector3.zero;
    }
}
