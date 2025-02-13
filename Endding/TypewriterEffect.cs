using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    private Text textComponent;
    private string fullText;
    private float timePerCharacter = 0.05f;
    private string currentText = ""; // ����һ���������ۼ��ַ�

    private void Start()
    {
        textComponent = GetComponent<Text>();
        fullText = textComponent.text;
        currentText = ""; // ��ʼ���ۼ��ַ���

        if (gameObject.activeInHierarchy) // ȷ��ֻ�ڶ��󼤻�ʱ����Э��
        {
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in fullText)
        {
            currentText += letter; // �ۼ��ַ���currentText
            textComponent.text = currentText; // ����Text������ı�
            yield return new WaitForSeconds(timePerCharacter); // �ȴ�
        }
    }
}
