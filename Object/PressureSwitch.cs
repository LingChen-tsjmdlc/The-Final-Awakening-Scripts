using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    private Animator pressureSwitchAnimator;

    private void Start()
    {
        // 获取父物体的Animator组件
        pressureSwitchAnimator = transform.parent.GetComponent<Animator>();

        // 检查是否成功获取到Animator组件
        if (pressureSwitchAnimator == null)
        {
            Debug.LogError("PressureSwitch 父物体上没有Animator组件！");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pressureSwitchAnimator.SetBool("Press", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pressureSwitchAnimator.SetBool("Press", false);
            Invoke("ToIdelState", 0.5f);
        }
    }

    private void ToIdelState()
    {
        pressureSwitchAnimator.SetTrigger("CanIdel");
    }
}
