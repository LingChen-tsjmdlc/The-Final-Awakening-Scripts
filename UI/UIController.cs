using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public List<Button> buttons; // 所有按钮的列表
    private int currentIndex = 0; // 当前选中的按钮索引
    public Color selectedColor = Color.red; // 选中时的颜色
    public Color defaultColor = Color.white; // 默认颜色

    bool canChange = true;

    private void Start()
    {
        // 初始化时选择列表中的第一个按钮
        if (buttons.Count > 0)
        {
            SelectButton(0);
        }
    }

    private void Update()
    {
        // 检测手柄垂直轴输入
        float verticalInput = Input.GetAxis("Vertical");

        // 当垂直轴输入超过阈值时，改变当前选中的按钮
        if (Mathf.Abs(verticalInput) > 0.5f && canChange)
        {
            canChange = false;
            // 根据输入方向更新索引
            currentIndex += verticalInput > 0 ? -1 : 1;
            // 确保索引在按钮列表的范围内
            currentIndex = Mathf.Clamp(currentIndex, 0, buttons.Count - 1);
            // 选择新的按钮
            SelectButton(currentIndex);
        }
        if (Mathf.Abs(verticalInput) < 0.5f)
        {
            canChange = true;
        }


        // 检测是否按下确认键（例如，Xbox One手柄的A键）
        if (Input.GetButtonDown("Submit"))
        {
            // 激活当前选中的按钮
            if (buttons[currentIndex] != null)
            {
                buttons[currentIndex].onClick.Invoke();
            }
        }
    }

    private void SelectButton(int index)
    {
        // 还原之前选中的按钮颜色
        if (currentIndex >= 0 && currentIndex < buttons.Count)
        {
            SetButtonColor(buttons[currentIndex], defaultColor);
        }

        // 设置新的选中按钮
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        SetButtonColor(buttons[index], selectedColor); // 设置选中颜色
        currentIndex = index; // 更新当前索引
    }

    private void SetButtonColor(Button button, Color color)
    {
        // 假设每个按钮都有一个Image组件作为其子对象
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
