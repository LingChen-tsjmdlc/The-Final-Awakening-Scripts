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
                // 播放音频，并启动协程等待音频播放完成
                StartCoroutine(PlaySoundAndDie(playerMovement, FxAudioManager.GetInstance().downToTheWater));
            }
            else
            {
                // 如果不是水，直接执行死亡方法
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
                // 播放音频，并启动协程等待音频播放完成
                StartCoroutine(PlaySoundAndDie(playerMovement, FxAudioManager.GetInstance().downToTheWater));
            }
            else
            {
                // 如果不是水，直接执行死亡方法
                playerMovement.Die();
            }
        }
        if (collision.CompareTag("boll"))
        {
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// 等待音频播放完成后让玩家死亡
    /// </summary>
    /// <param name="playerMovement">玩家移动脚本</param>
    /// <param name="audioSource">播放的音频</param>
    /// <returns></returns>
    private IEnumerator PlaySoundAndDie(PlayrtMovement playerMovement,AudioSource audioSource)
    {
        audioSource.Play();

        // 等待音频播放完成
        yield return new WaitForSeconds(audioSource.clip.length);

        // 音频播放完毕后，执行死亡方法
        playerMovement.Die();
    }
}
