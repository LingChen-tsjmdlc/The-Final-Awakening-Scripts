using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator playerAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Die")
        {
            playerAnimator.SetBool("die", true);
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void DiedAndRestart()
    {
        GameManager.GetInstance().RestartNowScene();
    }
}
