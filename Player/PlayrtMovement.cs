using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayrtMovement : MonoBehaviour
{
    //单例模式设置
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

    public float speed;     // 移动速度
    public float jumpForce; //跳跃力量
    public Transform groundCheck;   // 一个在角色脚下的Transform，用来检测角色是否在地面上。
    public float checkDistance;       // 射线检测的距离。
    public LayerMask whatIsGround;  // 一个LayerMask，用来定义哪些层被认为是地面。

    private Rigidbody2D rb;
    private Animator playerAnimator;
    [SerializeField] private bool isGrounded;
    public static bool wasGrounded; // 用来跟踪上一帧是否在地面上

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
        // 绘制射线，仅用于可视化
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * checkDistance);
    }

    // 在类中添加一个平滑速度系数
    public float smoothSpeed = 10f;
    private void Run()
    {
        float runValue = Input.GetAxis("Horizontal");
        // 根据输入设置角色的水平速度
        if (runValue != 0)
        {
            //Debug.Log("runValue有值");
            playerAnimator.SetBool("isWalk", true);
            // 判断是否在地面上
            if (isGrounded)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                // 使用transform.position来移动角色
                Vector3 newPosition = transform.position;
                newPosition.x += runValue * speed * Time.deltaTime;
                transform.position = newPosition;
            }
            else
            {
                // 使用Rigidbody2D来平滑地移动角色
                float targetSpeed = runValue * speed;
                float smoothedSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, Time.fixedDeltaTime * smoothSpeed);
                rb.velocity = new Vector2(smoothedSpeed, rb.velocity.y);
            }
            Flip(runValue);
            // 检查是否已经在播放跑步音效，如果没有，则播放
            if (!FxAudioManager.GetInstance().playerRun.isPlaying)
            {
                FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerRun);
                FxAudioManager.GetInstance().playerRun.loop = true;
            }
        }
        else
        {
            // 当停止移动时，停止播放跑步音效
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
        // 使用射线检测是否在地面上
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, whatIsGround);
        isGrounded = hit.collider != null;
        // 当角色从空中落地时
        if (!wasGrounded && isGrounded)
        {
            playerAnimator.SetBool("isFalling", false);
            playerAnimator.SetBool("jump", false);
        }
        // 触发跳跃动画
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            playerAnimator.SetBool("jump", true);

            FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerJump);   // 播放音效
        }
        // 当角色离开地面时，且速度为正，即上升阶段
        else if (!isGrounded && rb.velocity.y > 0)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("jump", true);
            playerAnimator.SetBool("isFalling", false);
        }
        // 当角色离开地面时，且速度为负，即下落阶段
        else if (!isGrounded && rb.velocity.y <= 0)
        {
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("jump", false);
            playerAnimator.SetBool("isFalling", true);
        }

        // 更新wasGrounded变量，为下一帧做准备
        wasGrounded = isGrounded;
    }

    private void Flip(float horizontal)
    {
        // 根据水平移动方向翻转角色
        if (horizontal > 0 && transform.localScale.x < 0f || horizontal < 0 && transform.localScale.x > 0f)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }
    }

    public void Die()
    {
        Debug.Log("死亡！");
        FxAudioManager.GetInstance().PlaySound(FxAudioManager.GetInstance().playerDie);   // 播放音效
        playerAnimator.SetBool("die", true);
        rb.bodyType = RigidbodyType2D.Static;
        Invoke(nameof(DiedAndRestart), 0.5f);
    }

    public void DiedAndRestart()
    {
        GameManager.GetInstance().RestartNowScene();
    }

    /// <summary>
    /// 停止玩家的移动和跳跃
    /// </summary>
    public void StopMovementAndJumping()
    {
        // 停止移动，将Rigidbody的速度设置为0
        rb.velocity = Vector3.zero;
    }
}
