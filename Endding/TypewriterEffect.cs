using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    private Text textComponent;
    private string fullText;
    private float timePerCharacter = 0.05f;
    private string currentText = ""; // 新增一个变量来累加字符

    private void Start()
    {
        textComponent = GetComponent<Text>();
        fullText = textComponent.text;
        currentText = ""; // 初始化累加字符串

        if (gameObject.activeInHierarchy) // 确保只在对象激活时启动协程
        {
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in fullText)
        {
            currentText += letter; // 累加字符到currentText
            textComponent.text = currentText; // 更新Text组件的文本
            yield return new WaitForSeconds(timePerCharacter); // 等待
        }
    }
}
