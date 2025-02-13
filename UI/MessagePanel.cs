using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public Text titleText;          // 标题Text组件
    public Text descriptionText;    // 描述Text组件
    private string fullDescription; // 描述文本的完整内容
    private float revealSpeed = 0.05f; // 私有的文字出现速度（每字符）
    private Coroutine revealCoroutine; // 协程引用

    void Start()
    {
        titleText.text = MachineCollider.messageTitleString;
        descriptionText.text = MachineCollider.messageDisCribeString;

        // 获取描述文本的完整内容
        fullDescription = descriptionText.text;
        // 清空描述文本，准备逐字显示
        descriptionText.text = "";

        // 开始逐字显示描述文本，并保存协程引用
        revealCoroutine = StartCoroutine(RevealDescription());
    }

    private void Update()
    {
        PlayrtMovement.instance.StopMovementAndJumping();
    }

    IEnumerator RevealDescription()
    {
        foreach (char letter in fullDescription)
        {
            // 逐字添加到描述文本
            descriptionText.text += letter;
            // 等待一定时间，实现文字逐字出现的效果
            yield return new WaitForSeconds(revealSpeed);
        }
    }

    public void ClosePanel()
    {
        // 停止协程
        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
        }
        UIAudioManager.GetInstance().PlaySound(UIAudioManager.GetInstance().buttonDownAudio);
        UIManager.GetInstance().DestroyPanel(this.gameObject, false);
        //Time.timeScale = 1.0f;
        GameManager.GetInstance().ispause = false;
    }
}
