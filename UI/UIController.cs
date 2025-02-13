using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public List<Button> buttons; // ���а�ť���б�
    private int currentIndex = 0; // ��ǰѡ�еİ�ť����
    public Color selectedColor = Color.red; // ѡ��ʱ����ɫ
    public Color defaultColor = Color.white; // Ĭ����ɫ

    bool canChange = true;

    private void Start()
    {
        // ��ʼ��ʱѡ���б��еĵ�һ����ť
        if (buttons.Count > 0)
        {
            SelectButton(0);
        }
    }

    private void Update()
    {
        // ����ֱ���ֱ������
        float verticalInput = Input.GetAxis("Vertical");

        // ����ֱ�����볬����ֵʱ���ı䵱ǰѡ�еİ�ť
        if (Mathf.Abs(verticalInput) > 0.5f && canChange)
        {
            canChange = false;
            // �������뷽���������
            currentIndex += verticalInput > 0 ? -1 : 1;
            // ȷ�������ڰ�ť�б�ķ�Χ��
            currentIndex = Mathf.Clamp(currentIndex, 0, buttons.Count - 1);
            // ѡ���µİ�ť
            SelectButton(currentIndex);
        }
        if (Mathf.Abs(verticalInput) < 0.5f)
        {
            canChange = true;
        }


        // ����Ƿ���ȷ�ϼ������磬Xbox One�ֱ���A����
        if (Input.GetButtonDown("Submit"))
        {
            // ���ǰѡ�еİ�ť
            if (buttons[currentIndex] != null)
            {
                buttons[currentIndex].onClick.Invoke();
            }
        }
    }

    private void SelectButton(int index)
    {
        // ��ԭ֮ǰѡ�еİ�ť��ɫ
        if (currentIndex >= 0 && currentIndex < buttons.Count)
        {
            SetButtonColor(buttons[currentIndex], defaultColor);
        }

        // �����µ�ѡ�а�ť
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        SetButtonColor(buttons[index], selectedColor); // ����ѡ����ɫ
        currentIndex = index; // ���µ�ǰ����
    }

    private void SetButtonColor(Button button, Color color)
    {
        // ����ÿ����ť����һ��Image�����Ϊ���Ӷ���
        Image buttonImage = button.GetComponentInChildren<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
            Debug.Log("Button color set to " + color + " for button " + button.name);
        }
        else
        {
            Debug.LogError("No Image component found for button " + button.name);
        }
    }
}
