using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public bool isWater;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            PlayrtMovement playerMovement = collision.gameObject.GetComponent<PlayrtMovement>();
            if (isWater)
            {
                // ������Ƶ��������Э�̵ȴ���Ƶ�������
                StartCoroutine(PlaySoundAndDie(playerMovement, FxAudioManager.GetInstance().downToTheWater));
            }
            else
            {
                // �������ˮ��ֱ��ִ����������
                playerMovement.Die();
            }
        }
        if (collision.CompareTag("boll") && isWater)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            PlayrtMovement playerMovement = collision.gameObject.GetComponent<PlayrtMovement>();
            if (isWater)
            {
                // ������Ƶ��������Э�̵ȴ���Ƶ�������
                StartCoroutine(PlaySoundAndDie(playerMovement, FxAudioManager.GetInstance().downToTheWater));
            }
            else
            {
                // �������ˮ��ֱ��ִ����������
                playerMovement.Die();
            }
        }
        if (collision.CompareTag("boll"))
        {
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// �ȴ���Ƶ������ɺ����������
    /// </summary>
    /// <param name="playerMovement">����ƶ��ű�</param>
    /// <param name="audioSource">���ŵ���Ƶ</param>
    /// <returns></returns>
    private IEnumerator PlaySoundAndDie(PlayrtMovement playerMovement,AudioSource audioSource)
    {
        audioSource.Play();

        // �ȴ���Ƶ�������
        yield return new WaitForSeconds(audioSource.clip.length);

        // ��Ƶ������Ϻ�ִ����������
        playerMovement.Die();
    }
}
