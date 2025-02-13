using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public Text titleText;          // ����Text���
    public Text descriptionText;    // ����Text���
    private string fullDescription; // �����ı�����������
    private float revealSpeed = 0.05f; // ˽�е����ֳ����ٶȣ�ÿ�ַ���
    private Coroutine revealCoroutine; // Э������

    void Start()
    {
        titleText.text = MachineCollider.messageTitleString;
        descriptionText.text = MachineCollider.messageDisCribeString;

        // ��ȡ�����ı�����������
        fullDescription = descriptionText.text;
        // ��������ı���׼��������ʾ
        descriptionText.text = "";

        // ��ʼ������ʾ�����ı���������Э������
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
            // ������ӵ������ı�
            descriptionText.text += letter;
            // �ȴ�һ��ʱ�䣬ʵ���������ֳ��ֵ�Ч��
            yield return new WaitForSeconds(revealSpeed);
        }
    }

    public void ClosePanel()
    {
        // ֹͣЭ��
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
