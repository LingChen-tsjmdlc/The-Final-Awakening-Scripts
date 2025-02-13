using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    private Animator pressureSwitchAnimator;

    private void Start()
    {
        // ��ȡ�������Animator���
        pressureSwitchAnimator = transform.parent.GetComponent<Animator>();

        // ����Ƿ�ɹ���ȡ��Animator���
        if (pressureSwitchAnimator == null)
        {
            Debug.LogError("PressureSwitch ��������û��Animator�����");
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
